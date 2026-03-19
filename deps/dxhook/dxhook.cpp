#include <d3d11.h>
#include <d3d11_4.h>
#include <d3d12.h>
#include <dxgi.h>
#include <dxgi1_4.h>
#include <cstdio>

#include "imgui.h"
#include "imgui_impl_dx11.h"
#include "imgui_impl_dx12.h"
#include "imgui_impl_win32.h"
#include "MinHook.h"
#include "ImGuizmo.h"
#pragma comment(lib, "d3d11.lib")
#pragma comment(lib, "d3d12.lib")
#pragma comment(lib, "dxgi.lib")

static const char* MHStatusToString(MH_STATUS status);

typedef HRESULT(WINAPI* PFN_Present)(IDXGISwapChain*, UINT, UINT);
typedef HRESULT(WINAPI* PFN_ResizeBuffers)(IDXGISwapChain*, UINT, UINT, UINT, DXGI_FORMAT, UINT);
typedef void(WINAPI* PFN_ExecuteCommandLists)(ID3D12CommandQueue*, UINT, ID3D12CommandList* const*);
typedef void(__stdcall* ManagedCallback)();

static PFN_Present g_OriginalPresent = nullptr;
static PFN_ResizeBuffers g_OriginalResizeBuffers = nullptr;
static PFN_ExecuteCommandLists g_OriginalExecuteCommandLists = nullptr;
static void* g_ExeCmdAddr = nullptr; // stored for deferred enable in InitDX12

// Private heap - isolates imgui allocations from game CRT heap
static HANDLE g_ImGuiHeap = nullptr;
static void* ImGuiMalloc(size_t sz, void*) { return HeapAlloc(g_ImGuiHeap, 0, sz); }
static void  ImGuiFree(void* ptr, void*)   { if (ptr) HeapFree(g_ImGuiHeap, 0, ptr); }

// DX11 state
static ID3D11Device* g_Device11 = nullptr;
static ID3D11DeviceContext* g_Context11 = nullptr;
static ID3D11RenderTargetView* g_RTV11 = nullptr;

// DX12 state
static ID3D12Device* g_Device12 = nullptr;
static ID3D12DescriptorHeap* g_SrvDescHeap12 = nullptr;
static ID3D12DescriptorHeap* g_RtvDescHeap12 = nullptr;
static ID3D12CommandAllocator** g_CmdAlloc12 = nullptr;
static ID3D12GraphicsCommandList* g_CmdList12 = nullptr;
static ID3D12CommandQueue* g_CmdQueue12 = nullptr;
static ID3D12Resource** g_BackBuffers12 = nullptr;
static D3D12_CPU_DESCRIPTOR_HANDLE* g_RtvHandles12 = nullptr;
static UINT g_BufferCount12 = 0;
static ID3D12Fence* g_Fence12 = nullptr;
static HANDLE g_FenceEvent12 = nullptr;
static UINT64* g_FenceValues12 = nullptr;  // per-buffer fence values
static UINT64 g_FenceCounter12 = 0;
static DXGI_FORMAT g_SwapchainFormat12 = DXGI_FORMAT_R8G8B8A8_UNORM;

enum GfxBackend { GFX_NONE, GFX_DX11, GFX_DX12 };
static GfxBackend g_Backend = GFX_NONE;

static bool g_Initialized = false;
static bool g_InitFailed = false;
static bool g_DX12Detected = false; // true once we see a DX12 device on the swapchain
static int g_PresentCallCount = 0;
static int g_ECLCallCount = 0;
static HWND g_GameHwnd = nullptr;
static WNDPROC g_OriginalWndProc = nullptr;
static int s_DX11Frame = 0;
static int s_ManagedCrashCount = 0;
static bool s_ManagedCallbacksDisabled = false;

// DX12 threaded callback dispatch:
// GC crashes when scanning the DX12 render thread's deep native stack.
// Fix: run managed callbacks on a dedicated .NET thread with a clean stack.
// Native side signals "go" after NewFrame(), waits for "done" before Render().
static HANDLE g_DX12GoEvent = nullptr;    // auto-reset: native -> managed "run callbacks"
static HANDLE g_DX12DoneEvent = nullptr;  // auto-reset: managed -> native "callbacks finished"
static volatile bool g_DX12ThreadedMode = false;
static volatile bool g_DX12ThreadReady = false;  // managed thread has entered callback loop
static volatile bool g_DX12Shutdown = false;
static volatile bool g_DX12NeedPreFirstFrame = false;  // pre-first-frame callbacks pending for managed thread
static volatile bool g_DX12InternalThreadExit = false;  // internal thread should exit (managed poller taking over)
static HANDLE g_DX12InternalThreadHandle = nullptr;

#define MAX_PRESENT_CALLBACKS 32
static ManagedCallback g_PresentCallbacks[MAX_PRESENT_CALLBACKS];
static int g_PresentCallbackCount = 0;

#define MAX_PRE_FRAME_CALLBACKS 8
static ManagedCallback g_PreFirstFrameCallbacks[MAX_PRE_FRAME_CALLBACKS];
static int g_PreFirstFrameCallbackCount = 0;

typedef void(__stdcall* WndProcCallback)(HWND, int, WPARAM, LPARAM);
#define MAX_WNDPROC_CALLBACKS 8
static WndProcCallback g_WndProcCallbacks[MAX_WNDPROC_CALLBACKS];
static int g_WndProcCallbackCount = 0;

static FILE* g_LogFile = nullptr;

static void OpenLogFile()
{
    if (g_LogFile) return;
    // resolve path relative to the exe, not cwd
    char exePath[MAX_PATH] = {};
    GetModuleFileNameA(nullptr, exePath, MAX_PATH);
    char* slash = strrchr(exePath, '\\');
    if (slash) *(slash + 1) = '\0';
    char logPath[MAX_PATH];
    snprintf(logPath, sizeof(logPath), "%scimgui_hook.log", exePath);
    g_LogFile = fopen(logPath, "w");
    if (!g_LogFile) {
        // fallback: cwd
        g_LogFile = fopen("cimgui_hook.log", "w");
    }
}

static void HookLog(const char* fmt, ...)
{
    char buf[512];
    va_list args;
    va_start(args, fmt);
    vsnprintf(buf, sizeof(buf), fmt, args);
    va_end(args);
    OutputDebugStringA(buf);

    OpenLogFile();
    if (g_LogFile) {
        fputs(buf, g_LogFile);
        fflush(g_LogFile);
    }
}

// ---- DX11 helpers ----

static void CreateRTV11(IDXGISwapChain* swapChain)
{
    ID3D11Texture2D* backBuffer = nullptr;
    HRESULT hr = swapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (void**)&backBuffer);
    if (FAILED(hr)) {
        HookLog("[DXHook] DX11 GetBuffer failed: 0x%08X\n", hr);
        return;
    }
    if (backBuffer) {
        hr = g_Device11->CreateRenderTargetView(backBuffer, nullptr, &g_RTV11);
        if (FAILED(hr))
            HookLog("[DXHook] DX11 CreateRenderTargetView failed: 0x%08X\n", hr);
        backBuffer->Release();
    }
}

static void CleanupRTV11()
{
    if (g_RTV11) {
        g_RTV11->Release();
        g_RTV11 = nullptr;
    }
}

// ---- DX12 helpers ----

static void CleanupDX12Buffers()
{
    if (g_BackBuffers12) {
        for (UINT i = 0; i < g_BufferCount12; i++) {
            if (g_BackBuffers12[i]) {
                g_BackBuffers12[i]->Release();
                g_BackBuffers12[i] = nullptr;
            }
        }
    }
}

static bool CreateDX12RenderTargets(IDXGISwapChain* swapChain)
{
    DXGI_SWAP_CHAIN_DESC desc;
    swapChain->GetDesc(&desc);
    g_BufferCount12 = desc.BufferCount;
    if (g_BufferCount12 == 0) g_BufferCount12 = 2;

    // RTV descriptor heap
    D3D12_DESCRIPTOR_HEAP_DESC rtvHeapDesc = {};
    rtvHeapDesc.Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV;
    rtvHeapDesc.NumDescriptors = g_BufferCount12;
    rtvHeapDesc.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE;
    HRESULT hr = g_Device12->CreateDescriptorHeap(&rtvHeapDesc, IID_PPV_ARGS(&g_RtvDescHeap12));
    if (FAILED(hr)) {
        HookLog("[DXHook] DX12 CreateDescriptorHeap(RTV) failed: 0x%08X\n", hr);
        return false;
    }

    UINT rtvSize = g_Device12->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
    D3D12_CPU_DESCRIPTOR_HANDLE rtvHandle = g_RtvDescHeap12->GetCPUDescriptorHandleForHeapStart();

    g_BackBuffers12 = new ID3D12Resource*[g_BufferCount12]();
    g_RtvHandles12 = new D3D12_CPU_DESCRIPTOR_HANDLE[g_BufferCount12];
    g_CmdAlloc12 = new ID3D12CommandAllocator*[g_BufferCount12]();

    for (UINT i = 0; i < g_BufferCount12; i++) {
        hr = swapChain->GetBuffer(i, IID_PPV_ARGS(&g_BackBuffers12[i]));
        if (FAILED(hr)) {
            HookLog("[DXHook] DX12 GetBuffer(%u) failed: 0x%08X\n", i, hr);
            return false;
        }
        g_RtvHandles12[i] = rtvHandle;
        g_Device12->CreateRenderTargetView(g_BackBuffers12[i], nullptr, rtvHandle);
        rtvHandle.ptr += rtvSize;

        hr = g_Device12->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, IID_PPV_ARGS(&g_CmdAlloc12[i]));
        if (FAILED(hr)) {
            HookLog("[DXHook] DX12 CreateCommandAllocator(%u) failed: 0x%08X\n", i, hr);
            return false;
        }
    }

    hr = g_Device12->CreateCommandList(0, D3D12_COMMAND_LIST_TYPE_DIRECT, g_CmdAlloc12[0],
        nullptr, IID_PPV_ARGS(&g_CmdList12));
    if (FAILED(hr)) {
        HookLog("[DXHook] DX12 CreateCommandList failed: 0x%08X\n", hr);
        return false;
    }
    g_CmdList12->Close();

    // Fence for GPU/CPU synchronization - wait before reusing command allocators
    g_FenceValues12 = new UINT64[g_BufferCount12]();
    g_FenceCounter12 = 0;
    hr = g_Device12->CreateFence(0, D3D12_FENCE_FLAG_NONE, IID_PPV_ARGS(&g_Fence12));
    if (FAILED(hr)) {
        HookLog("[DXHook] DX12 CreateFence failed: 0x%08X\n", hr);
        return false;
    }
    g_FenceEvent12 = CreateEventW(nullptr, FALSE, FALSE, nullptr);

    return true;
}

// ---- ExecuteCommandLists hook (captures the game's command queue) ----

static void WINAPI HookedExecuteCommandLists(ID3D12CommandQueue* queue, UINT numCmdLists, ID3D12CommandList* const* cmdLists)
{
    g_ECLCallCount++;
    if (g_ECLCallCount <= 20) {
        HookLog("[DXHook] ECL call #%d queue=%p numLists=%u g_CmdQueue12=%p\n",
            g_ECLCallCount, queue, numCmdLists, g_CmdQueue12);
    }
    if (!g_CmdQueue12 && queue) {
        D3D12_COMMAND_QUEUE_DESC desc = queue->GetDesc();
        HookLog("[DXHook] ECL: queue %p type=%d (DIRECT=0) flags=%u\n",
            queue, (int)desc.Type, desc.Flags);
        if (desc.Type == D3D12_COMMAND_LIST_TYPE_DIRECT) {
            g_CmdQueue12 = queue;
            HookLog("[DXHook] *** Captured game command queue: %p at ECL call #%d ***\n", queue, g_ECLCallCount);
        }
    }
    g_OriginalExecuteCommandLists(queue, numCmdLists, cmdLists);
}

// ---- Common ----

extern LRESULT ImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);

static LRESULT WINAPI HookedWndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    if (g_Initialized) {
        if (ImGui_ImplWin32_WndProcHandler(hwnd, msg, wParam, lParam))
            return 1;

        for (int i = 0; i < g_WndProcCallbackCount; i++) {
            if (g_WndProcCallbacks[i])
                g_WndProcCallbacks[i](hwnd, (int)msg, wParam, lParam);
        }
    }
    return CallWindowProcW(g_OriginalWndProc, hwnd, msg, wParam, lParam);
}

static void FirePreFirstFrameCallbacks()
{
    for (int i = 0; i < g_PreFirstFrameCallbackCount; i++) {
        if (g_PreFirstFrameCallbacks[i]) {
            HookLog("[DXHook] pre-first-frame callback %d enter\n", i);
            g_PreFirstFrameCallbacks[i]();
            HookLog("[DXHook] pre-first-frame callback %d exit\n", i);
        }
    }
}

static bool InitDX11(IDXGISwapChain* swapChain)
{
    HRESULT hr = swapChain->GetDevice(__uuidof(ID3D11Device), (void**)&g_Device11);
    if (FAILED(hr) || !g_Device11)
        return false;

    g_Device11->GetImmediateContext(&g_Context11);

    // Serialize DX11 immediate context access across threads
    ID3D11Multithread* mt = nullptr;
    if (SUCCEEDED(g_Context11->QueryInterface(IID_PPV_ARGS(&mt)))) {
        mt->SetMultithreadProtected(TRUE);
        mt->Release();
        HookLog("[DXHook] DX11 multithread protection enabled\n");
    }

    DXGI_SWAP_CHAIN_DESC desc;
    swapChain->GetDesc(&desc);
    HookLog("[DXHook] DX11 init - HWND=%p, %ux%u\n", desc.OutputWindow, desc.BufferDesc.Width, desc.BufferDesc.Height);

    ImGui::CreateContext();
    ImGuiIO& io = ImGui::GetIO();
    io.ConfigFlags |= ImGuiConfigFlags_NavEnableKeyboard;

    ImGui_ImplWin32_Init(desc.OutputWindow);
    ImGui_ImplDX11_Init(g_Device11, g_Context11);

    g_GameHwnd = desc.OutputWindow;
    g_OriginalWndProc = (WNDPROC)SetWindowLongPtrW(g_GameHwnd, GWLP_WNDPROC, (LONG_PTR)HookedWndProc);

    FirePreFirstFrameCallbacks();

    g_Backend = GFX_DX11;
    g_Initialized = true;

    // DX11 confirmed - disable ECL hook to prevent DX11-on-12 interception (vertex explosions)
    if (g_ExeCmdAddr) {
        MH_DisableHook(g_ExeCmdAddr);
        g_CmdQueue12 = nullptr;
        HookLog("[DXHook] ECL hook disabled (DX11 game)\n");
    }

    HookLog("[DXHook] DX11 init complete (device=%p ctx=%p)\n", (void*)g_Device11, (void*)g_Context11);
    return true;
}

// Vectored exception handler - fires before SEH, captures .NET exception info
static LONG CALLBACK DX12VectoredHandler(EXCEPTION_POINTERS* ep)
{
    if (ep->ExceptionRecord->ExceptionCode == 0xE0434352 && s_ManagedCrashCount < 5) {
        // 0xE0434352 = CLR exception. ExceptionInformation[0] = HRESULT, [4] = managed object ptr
        DWORD numParams = ep->ExceptionRecord->NumberParameters;
        HookLog("[DXHook] .NET exception: params=%lu", numParams);
        for (DWORD i = 0; i < numParams && i < 5; i++) {
            HookLog(" [%lu]=0x%llX", i, (unsigned long long)ep->ExceptionRecord->ExceptionInformation[i]);
        }
        HookLog("\n");
    }
    return EXCEPTION_CONTINUE_SEARCH;
}

// Internal callback thread for DX12: runs managed callbacks off the render thread.
// The render thread signals g_DX12GoEvent after NewFrame(), this thread runs callbacks,
// then signals g_DX12DoneEvent so the render thread can Render().
static DWORD WINAPI DX12CallbackThread(LPVOID)
{
    // COM init - some managed code expects STA/MTA
    CoInitializeEx(nullptr, COINIT_MULTITHREADED);
    AddVectoredExceptionHandler(1, DX12VectoredHandler);
    HookLog("[DXHook] Callback thread started (tid=%lu)\n", GetCurrentThreadId());
    g_DX12ThreadReady = true;

    while (!g_DX12Shutdown && !g_DX12InternalThreadExit) {
        DWORD wait = WaitForSingleObject(g_DX12GoEvent, 500);
        if (g_DX12Shutdown || g_DX12InternalThreadExit) {
            // If we consumed a go-event, signal done so the render thread isn't blocked
            if (wait == WAIT_OBJECT_0) SetEvent(g_DX12DoneEvent);
            break;
        }
        if (wait != WAIT_OBJECT_0) continue;

        // Fire pre-first-frame callbacks if pending
        if (g_DX12NeedPreFirstFrame) {
            HookLog("[DXHook] Callback thread: firing %d pre-first-frame callbacks\n", g_PreFirstFrameCallbackCount);
            for (int i = 0; i < g_PreFirstFrameCallbackCount; i++) {
                if (g_PreFirstFrameCallbacks[i]) g_PreFirstFrameCallbacks[i]();
            }
            g_DX12NeedPreFirstFrame = false;
        }

        // Run present callbacks with SEH to prevent thread death
        for (int i = 0; i < g_PresentCallbackCount; i++) {
            if (!g_PresentCallbacks[i]) continue;
            __try {
                g_PresentCallbacks[i]();
            } __except(EXCEPTION_EXECUTE_HANDLER) {
                DWORD code = GetExceptionCode();
                if (s_ManagedCrashCount < 5) {
                    HookLog("[DXHook] *** Callback %d/%d crash (code=0x%08X) ***\n",
                        i, g_PresentCallbackCount, code);
                }
                s_ManagedCrashCount++;
            }
        }

        if (s_ManagedCrashCount > 0 && s_ManagedCrashCount <= 5) {
            HookLog("[DXHook] Callback thread: crashes so far=%d\n", s_ManagedCrashCount);
        }
        SetEvent(g_DX12DoneEvent);
    }

    HookLog("[DXHook] Callback thread exiting\n");
    g_DX12ThreadReady = false;
    return 0;
}

static bool InitDX12(IDXGISwapChain* swapChain)
{
    HRESULT hr = swapChain->GetDevice(__uuidof(ID3D12Device), (void**)&g_Device12);
    if (FAILED(hr) || !g_Device12) {
        HookLog("[DXHook] InitDX12: GetDevice(ID3D12Device) failed hr=0x%08X dev=%p\n", hr, g_Device12);
        return false;
    }

    HookLog("[DXHook] InitDX12: DX12 device found %p (Present #%d, ECL #%d)\n",
        g_Device12, g_PresentCallCount, g_ECLCallCount);

    g_DX12Detected = true;

    // Wait for command queue capture from ExecuteCommandLists hook
    if (!g_CmdQueue12) {
        HookLog("[DXHook] InitDX12: waiting for command queue (g_ExeCmdAddr=%p, g_OrigECL=%p)\n",
            g_ExeCmdAddr, g_OriginalExecuteCommandLists);
        g_Device12->Release();
        g_Device12 = nullptr;
        return false; // will retry next Present
    }

    HookLog("[DXHook] InitDX12: command queue ready %p, proceeding with init\n", g_CmdQueue12);

    DXGI_SWAP_CHAIN_DESC desc;
    swapChain->GetDesc(&desc);
    g_SwapchainFormat12 = desc.BufferDesc.Format;
    HookLog("[DXHook] InitDX12: HWND=%p, %ux%u, BufferCount=%u, Format=%d\n",
        desc.OutputWindow, desc.BufferDesc.Width, desc.BufferDesc.Height,
        desc.BufferCount, (int)g_SwapchainFormat12);

    // SRV descriptor heap for ImGui font texture
    D3D12_DESCRIPTOR_HEAP_DESC srvHeapDesc = {};
    srvHeapDesc.Type = D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV;
    srvHeapDesc.NumDescriptors = 1;
    srvHeapDesc.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE;
    hr = g_Device12->CreateDescriptorHeap(&srvHeapDesc, IID_PPV_ARGS(&g_SrvDescHeap12));
    if (FAILED(hr)) {
        HookLog("[DXHook] InitDX12: CreateDescriptorHeap(SRV) failed: 0x%08X\n", hr);
        return false;
    }
    HookLog("[DXHook] InitDX12: SRV heap created\n");

    if (!CreateDX12RenderTargets(swapChain)) {
        HookLog("[DXHook] InitDX12: CreateDX12RenderTargets FAILED\n");
        return false;
    }
    HookLog("[DXHook] InitDX12: render targets created (%u buffers)\n", g_BufferCount12);

    ImGui::CreateContext();
    HookLog("[DXHook] InitDX12: ImGui context created\n");
    ImGuiIO& io = ImGui::GetIO();
    io.ConfigFlags |= ImGuiConfigFlags_NavEnableKeyboard;

    ImGui_ImplWin32_Init(desc.OutputWindow);
    HookLog("[DXHook] InitDX12: ImGui_ImplWin32_Init done\n");

    // Use struct-based init (required for imgui 1.92+ font atlas / RendererHasTextures)
    ImGui_ImplDX12_InitInfo dx12_init = {};
    dx12_init.Device = g_Device12;
    dx12_init.CommandQueue = g_CmdQueue12;
    dx12_init.NumFramesInFlight = (int)g_BufferCount12;
    dx12_init.RTVFormat = g_SwapchainFormat12;
    dx12_init.SrvDescriptorHeap = g_SrvDescHeap12;
    dx12_init.LegacySingleSrvCpuDescriptor = g_SrvDescHeap12->GetCPUDescriptorHandleForHeapStart();
    dx12_init.LegacySingleSrvGpuDescriptor = g_SrvDescHeap12->GetGPUDescriptorHandleForHeapStart();
    ImGui_ImplDX12_Init(&dx12_init);
    HookLog("[DXHook] InitDX12: ImGui_ImplDX12_Init done (struct-based, RendererHasTextures)\n");

    g_GameHwnd = desc.OutputWindow;
    g_OriginalWndProc = (WNDPROC)SetWindowLongPtrW(g_GameHwnd, GWLP_WNDPROC, (LONG_PTR)HookedWndProc);
    HookLog("[DXHook] InitDX12: WndProc subclassed (orig=%p)\n", g_OriginalWndProc);

    // Do NOT call FirePreFirstFrameCallbacks() here - that would run managed code
    // on the render thread, permanently attaching it to the CLR. GC would then try
    // to walk this thread's deep DX12 native frames and crash.
    // Instead, defer to the managed poller thread via signal/wait.
    g_DX12NeedPreFirstFrame = true;
    HookLog("[DXHook] InitDX12: pre-first-frame callbacks deferred to managed thread\n");

    g_Backend = GFX_DX12;
    g_Initialized = true;

    // Enable threaded callback dispatch for DX12.
    // Managed code must NOT run on this render thread - CLR attaches it permanently
    // and GC crashes walking the deep native DX12 frames.
    // Auto-spawn a native helper thread that runs the managed callbacks.
    if (!g_DX12GoEvent) {
        g_DX12GoEvent = CreateEventW(nullptr, FALSE, FALSE, nullptr);
        g_DX12DoneEvent = CreateEventW(nullptr, FALSE, FALSE, nullptr);
    }
    g_DX12ThreadedMode = true;

    // Spawn internal callback thread (runs managed callbacks off the render thread)
    HANDLE hThread = CreateThread(nullptr, 0, DX12CallbackThread, nullptr, 0, nullptr);
    if (hThread) {
        g_DX12InternalThreadHandle = hThread;
        HookLog("[DXHook] DX12 auto-spawned callback thread\n");
    } else {
        HookLog("[DXHook] Failed to spawn callback thread: %lu\n", GetLastError());
    }

    // Disable ECL hook - we've captured the queue, no longer need it
    if (g_ExeCmdAddr) {
        MH_DisableHook(g_ExeCmdAddr);
        HookLog("[DXHook] ECL hook disabled (queue captured)\n");
    }

    HookLog("[DXHook] *** DX12 init complete (took %d Present calls) ***\n", g_PresentCallCount);
    return true;
}

static HRESULT WINAPI HookedPresent(IDXGISwapChain* swapChain, UINT syncInterval, UINT flags)
{
    g_PresentCallCount++;
    if (g_PresentCallCount <= 5) {
        HookLog("[DXHook] Present #%d: init=%d failed=%d dx12det=%d backend=%d queue=%p ECLcalls=%d\n",
            g_PresentCallCount, g_Initialized, g_InitFailed, g_DX12Detected,
            (int)g_Backend, g_CmdQueue12, g_ECLCallCount);
    }

    if (!g_Initialized && !g_InitFailed) {
        // Try DX12 first (returns false if waiting for command queue), then DX11
        if (!InitDX12(swapChain)) {
            if (!g_DX12Detected) {
                // No DX12 device at all, try DX11
                HookLog("[DXHook] Present #%d: no DX12 device, trying DX11\n", g_PresentCallCount);
                if (!InitDX11(swapChain)) {
                    HookLog("[DXHook] Both DX11 and DX12 init failed\n");
                    g_InitFailed = true;
                }
            } else {
                // DX12 detected but waiting for command queue
                if (g_PresentCallCount <= 30 || g_PresentCallCount % 300 == 0) {
                    HookLog("[DXHook] Present #%d: DX12 detected, still waiting for ECL queue capture\n",
                        g_PresentCallCount);
                }
            }
        }
    }

    if (g_Initialized && g_Backend == GFX_DX11) {
        if (!g_RTV11) {
            HookLog("[DXHook] RTV null, recreating\n");
            CreateRTV11(swapChain);
            HookLog("[DXHook] RTV after recreate: %p\n", (void*)g_RTV11);
        }

        s_DX11Frame++;
        bool early = (s_DX11Frame <= 10);
        if (early) HookLog("[DXHook] dx11 frame %d: NewFrame\n", s_DX11Frame);
        ImGui_ImplDX11_NewFrame();
        ImGui_ImplWin32_NewFrame();
        ImGui::NewFrame();
        ImGuizmo::BeginFrame();
        if (early) HookLog("[DXHook] dx11 frame %d: NewFrame ok, callbacks\n", s_DX11Frame);

        if (!s_ManagedCallbacksDisabled) {
            __try {
                for (int i = 0; i < g_PresentCallbackCount; i++) {
                    if (g_PresentCallbacks[i]) {
                        if (early) HookLog("[DXHook] dx11 frame %d: callback %d enter\n", s_DX11Frame, i);
                        g_PresentCallbacks[i]();
                        if (early) HookLog("[DXHook] dx11 frame %d: callback %d exit\n", s_DX11Frame, i);
                    }
                }
            } __except(EXCEPTION_EXECUTE_HANDLER) {
                s_ManagedCrashCount++;
                HookLog("[DXHook] *** Managed callback SEH crash #%d (code=0x%08X) ***\n",
                    s_ManagedCrashCount, GetExceptionCode());
                if (s_ManagedCrashCount >= 3) {
                    s_ManagedCallbacksDisabled = true;
                    HookLog("[DXHook] *** Managed callbacks disabled after %d crashes ***\n", s_ManagedCrashCount);
                }
            }
        }

        if (early) HookLog("[DXHook] dx11 frame %d: Render\n", s_DX11Frame);
        ImGui::Render();
        g_Context11->OMSetRenderTargets(1, &g_RTV11, nullptr);
        if (early) HookLog("[DXHook] dx11 frame %d: RenderDrawData rtv=%p\n", s_DX11Frame, (void*)g_RTV11);
        ImGui_ImplDX11_RenderDrawData(ImGui::GetDrawData());
        if (early) HookLog("[DXHook] dx11 frame %d: done\n", s_DX11Frame);
    }
    else if (g_Initialized && g_Backend == GFX_DX12) {
        static int s_DX12Frame = 0;
        s_DX12Frame++;
        bool diag = (s_DX12Frame <= 300 || s_DX12Frame % 50 == 0);

        if (!g_CmdList12 || !g_CmdAlloc12 || !g_BackBuffers12) {
            if (diag) HookLog("[DXHook] dx12 #%d: resources null, skip\n", s_DX12Frame);
            return g_OriginalPresent(swapChain, syncInterval, flags);
        }

        UINT frameIdx = 0;
        IDXGISwapChain3* sc3 = nullptr;
        if (SUCCEEDED(swapChain->QueryInterface(IID_PPV_ARGS(&sc3)))) {
            frameIdx = sc3->GetCurrentBackBufferIndex();
            sc3->Release();
        }

        if (frameIdx >= g_BufferCount12)
            return g_OriginalPresent(swapChain, syncInterval, flags);

        // Check if device is still alive
        HRESULT hrRemoved = g_Device12->GetDeviceRemovedReason();
        if (FAILED(hrRemoved)) {
            HookLog("[DXHook] dx12 #%d: DEVICE REMOVED reason=0x%08X, skipping frame\n", s_DX12Frame, hrRemoved);
            return g_OriginalPresent(swapChain, syncInterval, flags);
        }

        // Wait for GPU to finish with this buffer's command allocator before reusing it
        if (g_Fence12 && g_FenceValues12 && g_FenceValues12[frameIdx] > 0) {
            UINT64 completed = g_Fence12->GetCompletedValue();
            if (completed < g_FenceValues12[frameIdx]) {
                if (diag) HookLog("[DXHook] dx12 #%d: fence wait (completed=%llu need=%llu)\n",
                    s_DX12Frame, completed, g_FenceValues12[frameIdx]);
                g_Fence12->SetEventOnCompletion(g_FenceValues12[frameIdx], g_FenceEvent12);
                WaitForSingleObject(g_FenceEvent12, 1000);
            }
        }

        if (diag) HookLog("[DXHook] dx12 #%d: frameIdx=%u reset\n", s_DX12Frame, frameIdx);
        HRESULT hr1 = g_CmdAlloc12[frameIdx]->Reset();
        HRESULT hr2 = g_CmdList12->Reset(g_CmdAlloc12[frameIdx], nullptr);
        if (FAILED(hr1) || FAILED(hr2)) {
            HookLog("[DXHook] dx12 #%d: Reset FAILED alloc=0x%08X list=0x%08X\n", s_DX12Frame, hr1, hr2);
            return g_OriginalPresent(swapChain, syncInterval, flags);
        }

        D3D12_RESOURCE_BARRIER barrier = {};
        barrier.Type = D3D12_RESOURCE_BARRIER_TYPE_TRANSITION;
        barrier.Transition.pResource = g_BackBuffers12[frameIdx];
        barrier.Transition.StateBefore = D3D12_RESOURCE_STATE_PRESENT;
        barrier.Transition.StateAfter = D3D12_RESOURCE_STATE_RENDER_TARGET;
        barrier.Transition.Subresource = D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES;
        g_CmdList12->ResourceBarrier(1, &barrier);

        g_CmdList12->OMSetRenderTargets(1, &g_RtvHandles12[frameIdx], FALSE, nullptr);
        g_CmdList12->SetDescriptorHeaps(1, &g_SrvDescHeap12);

        // Pre-first-frame callbacks are handled by the callback thread
        // (it checks g_DX12NeedPreFirstFrame on its first signal)

        if (diag) HookLog("[DXHook] dx12 #%d: NewFrame\n", s_DX12Frame);
        ImGui_ImplDX12_NewFrame();
        ImGui_ImplWin32_NewFrame();
        ImGui::NewFrame();
        ImGuizmo::BeginFrame();

        if (g_DX12ThreadedMode && g_DX12ThreadReady) {
            if (diag) HookLog("[DXHook] dx12 #%d: signal managed\n", s_DX12Frame);
            SetEvent(g_DX12GoEvent);
            DWORD result = WaitForSingleObject(g_DX12DoneEvent, 5000);
            if (result == WAIT_TIMEOUT) {
                HookLog("[DXHook] DX12 callback thread timeout (5s)!\n");
            }
            if (diag) HookLog("[DXHook] dx12 #%d: managed done\n", s_DX12Frame);
        } else if (g_DX12ThreadedMode && !g_DX12ThreadReady) {
            if (diag) HookLog("[DXHook] dx12 #%d: skip (not ready)\n", s_DX12Frame);
        } else if (!s_ManagedCallbacksDisabled) {
            // Fallback: direct call (before managed thread starts, or DX11)
            __try {
                for (int i = 0; i < g_PresentCallbackCount; i++) {
                    if (g_PresentCallbacks[i])
                        g_PresentCallbacks[i]();
                }
            } __except(EXCEPTION_EXECUTE_HANDLER) {
                s_ManagedCrashCount++;
                HookLog("[DXHook] *** Managed callback SEH crash #%d (code=0x%08X) ***\n",
                    s_ManagedCrashCount, GetExceptionCode());
                if (s_ManagedCrashCount >= 3) {
                    s_ManagedCallbacksDisabled = true;
                    HookLog("[DXHook] *** Managed callbacks disabled after %d crashes ***\n", s_ManagedCrashCount);
                }
            }
        }

        if (diag) HookLog("[DXHook] dx12 #%d: Render\n", s_DX12Frame);
        ImGui::Render();
        ImGui_ImplDX12_RenderDrawData(ImGui::GetDrawData(), g_CmdList12);

        barrier.Transition.StateBefore = D3D12_RESOURCE_STATE_RENDER_TARGET;
        barrier.Transition.StateAfter = D3D12_RESOURCE_STATE_PRESENT;
        g_CmdList12->ResourceBarrier(1, &barrier);

        HRESULT hrClose = g_CmdList12->Close();
        if (FAILED(hrClose)) {
            HookLog("[DXHook] dx12 #%d: Close FAILED 0x%08X\n", s_DX12Frame, hrClose);
        }
        ID3D12CommandList* cmdLists[] = { g_CmdList12 };
        g_CmdQueue12->ExecuteCommandLists(1, cmdLists);

        // Signal fence so we know when GPU finishes with this frame's allocator
        if (g_Fence12 && g_FenceValues12) {
            g_FenceCounter12++;
            g_FenceValues12[frameIdx] = g_FenceCounter12;
            g_CmdQueue12->Signal(g_Fence12, g_FenceCounter12);
        }
        if (diag) HookLog("[DXHook] dx12 #%d: submit done\n", s_DX12Frame);
    }

    return g_OriginalPresent(swapChain, syncInterval, flags);
}

static HRESULT WINAPI HookedResizeBuffers(IDXGISwapChain* swapChain, UINT bufferCount, UINT width, UINT height, DXGI_FORMAT format, UINT swapChainFlags)
{
    HookLog("[DXHook] ResizeBuffers %ux%u\n", width, height);

    if (g_Backend == GFX_DX11) {
        ImGui_ImplDX11_InvalidateDeviceObjects();
        CleanupRTV11();
    }
    else if (g_Backend == GFX_DX12) {
        // Wait for all GPU work before tearing down
        if (g_Fence12 && g_CmdQueue12) {
            g_FenceCounter12++;
            g_CmdQueue12->Signal(g_Fence12, g_FenceCounter12);
            if (g_Fence12->GetCompletedValue() < g_FenceCounter12) {
                g_Fence12->SetEventOnCompletion(g_FenceCounter12, g_FenceEvent12);
                WaitForSingleObject(g_FenceEvent12, 5000);
            }
        }
        ImGui_ImplDX12_InvalidateDeviceObjects();
        CleanupDX12Buffers();
        if (g_CmdList12) { g_CmdList12->Release(); g_CmdList12 = nullptr; }
        if (g_CmdAlloc12) {
            for (UINT i = 0; i < g_BufferCount12; i++) {
                if (g_CmdAlloc12[i]) { g_CmdAlloc12[i]->Release(); g_CmdAlloc12[i] = nullptr; }
            }
        }
        if (g_RtvDescHeap12) { g_RtvDescHeap12->Release(); g_RtvDescHeap12 = nullptr; }
        if (g_Fence12) { g_Fence12->Release(); g_Fence12 = nullptr; }
        delete[] g_FenceValues12; g_FenceValues12 = nullptr;
    }

    HRESULT hr = g_OriginalResizeBuffers(swapChain, bufferCount, width, height, format, swapChainFlags);

    if (g_Backend == GFX_DX12 && SUCCEEDED(hr)) {
        CreateDX12RenderTargets(swapChain);
        ImGui_ImplDX12_CreateDeviceObjects();
        if (!g_FenceEvent12)
            g_FenceEvent12 = CreateEventW(nullptr, FALSE, FALSE, nullptr);
    }

    return hr;
}

static const char* MHStatusToString(MH_STATUS status)
{
    switch (status) {
        case MH_OK: return "OK";
        case MH_ERROR_ALREADY_INITIALIZED: return "ALREADY_INITIALIZED";
        case MH_ERROR_NOT_INITIALIZED: return "NOT_INITIALIZED";
        case MH_ERROR_ALREADY_CREATED: return "ALREADY_CREATED";
        case MH_ERROR_NOT_CREATED: return "NOT_CREATED";
        case MH_ERROR_ENABLED: return "ENABLED";
        case MH_ERROR_DISABLED: return "DISABLED";
        case MH_ERROR_NOT_EXECUTABLE: return "NOT_EXECUTABLE";
        case MH_ERROR_UNSUPPORTED_FUNCTION: return "UNSUPPORTED_FUNCTION";
        case MH_ERROR_MEMORY_ALLOC: return "MEMORY_ALLOC";
        case MH_ERROR_MEMORY_PROTECT: return "MEMORY_PROTECT";
        case MH_ERROR_MODULE_NOT_FOUND: return "MODULE_NOT_FOUND";
        case MH_ERROR_FUNCTION_NOT_FOUND: return "FUNCTION_NOT_FOUND";
        default: return "UNKNOWN";
    }
}

// get Present/ResizeBuffers vtable addresses via dummy DX11 swapchain
static bool GetSwapChainVtable(void** outPresent, void** outResizeBuffers)
{
    WNDCLASSEXW wc = {};
    wc.cbSize = sizeof(wc);
    wc.style = CS_HREDRAW | CS_VREDRAW;
    wc.lpfnWndProc = DefWindowProcW;
    wc.hInstance = GetModuleHandleW(nullptr);
    wc.lpszClassName = L"DXHookDummy";
    RegisterClassExW(&wc);

    HWND hwnd = CreateWindowExW(0, wc.lpszClassName, L"", WS_OVERLAPPEDWINDOW,
        0, 0, 100, 100, nullptr, nullptr, wc.hInstance, nullptr);

    if (!hwnd) {
        HookLog("[DXHook] CreateWindowExW failed: %lu\n", GetLastError());
        UnregisterClassW(wc.lpszClassName, wc.hInstance);
        return false;
    }

    DXGI_SWAP_CHAIN_DESC sd = {};
    sd.BufferCount = 1;
    sd.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
    sd.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
    sd.OutputWindow = hwnd;
    sd.SampleDesc.Count = 1;
    sd.Windowed = TRUE;

    IDXGISwapChain* sc = nullptr;
    ID3D11Device* dev = nullptr;
    ID3D11DeviceContext* ctx = nullptr;

    D3D_DRIVER_TYPE driverTypes[] = {
        D3D_DRIVER_TYPE_HARDWARE,
        D3D_DRIVER_TYPE_WARP,
        D3D_DRIVER_TYPE_NULL,
    };

    HRESULT hr = E_FAIL;
    for (int i = 0; i < 3; i++) {
        hr = D3D11CreateDeviceAndSwapChain(
            nullptr, driverTypes[i], nullptr, 0,
            nullptr, 0, D3D11_SDK_VERSION,
            &sd, &sc, &dev, nullptr, &ctx);
        if (SUCCEEDED(hr)) {
            HookLog("[DXHook] Dummy swapchain created with driver type %d\n", (int)driverTypes[i]);
            break;
        }
        HookLog("[DXHook] D3D11CreateDeviceAndSwapChain failed (driver type %d): 0x%08X\n", (int)driverTypes[i], hr);
    }

    if (FAILED(hr)) {
        HookLog("[DXHook] All driver types failed\n");
        DestroyWindow(hwnd);
        UnregisterClassW(wc.lpszClassName, wc.hInstance);
        return false;
    }

    void** vtable = *(void***)sc;
    *outPresent = vtable[8];
    *outResizeBuffers = vtable[13];

    HookLog("[DXHook] Present=%p, ResizeBuffers=%p\n", *outPresent, *outResizeBuffers);

    sc->Release();
    dev->Release();
    ctx->Release();
    DestroyWindow(hwnd);
    UnregisterClassW(wc.lpszClassName, wc.hInstance);
    HookLog("[DXHook] GetSwapChainVtable cleanup done\n");
    return true;
}

// get ExecuteCommandLists vtable address via dummy DX12 command queue
static bool GetExecuteCommandListsAddr(void** outAddr)
{
    ID3D12Device* tmpDev = nullptr;
    HRESULT hr = D3D12CreateDevice(nullptr, D3D_FEATURE_LEVEL_11_0, IID_PPV_ARGS(&tmpDev));
    if (FAILED(hr) || !tmpDev) {
        HookLog("[DXHook] D3D12CreateDevice failed: 0x%08X\n", hr);
        return false;
    }

    D3D12_COMMAND_QUEUE_DESC qDesc = {};
    qDesc.Type = D3D12_COMMAND_LIST_TYPE_DIRECT;
    ID3D12CommandQueue* tmpQueue = nullptr;
    hr = tmpDev->CreateCommandQueue(&qDesc, IID_PPV_ARGS(&tmpQueue));
    if (FAILED(hr) || !tmpQueue) {
        HookLog("[DXHook] CreateCommandQueue failed: 0x%08X\n", hr);
        tmpDev->Release();
        return false;
    }

    void** vtable = *(void***)tmpQueue;
    *outAddr = vtable[10]; // ID3D12CommandQueue::ExecuteCommandLists

    HookLog("[DXHook] ExecuteCommandLists=%p\n", *outAddr);

    tmpQueue->Release();
    tmpDev->Release();
    return true;
}

extern "C" {

__declspec(dllexport) void InitDX11Hook()
{
    OpenLogFile();
    char cwd[MAX_PATH] = {};
    GetCurrentDirectoryA(MAX_PATH, cwd);
    HookLog("[DXHook] InitDX11Hook called (cwd=%s)\n", cwd);

    // Private heap - routes imgui allocs away from game/CRT heap to avoid corruption
    if (!g_ImGuiHeap) {
        g_ImGuiHeap = HeapCreate(0, 0, 0);
        if (g_ImGuiHeap) {
            ImGui::SetAllocatorFunctions(ImGuiMalloc, ImGuiFree, nullptr);
            HookLog("[DXHook] Private imgui heap: %p\n", g_ImGuiHeap);
        } else {
            HookLog("[DXHook] HeapCreate failed: %lu\n", GetLastError());
        }
    }

    void* presentAddr = nullptr;
    void* resizeAddr = nullptr;

    if (!GetSwapChainVtable(&presentAddr, &resizeAddr)) {
        HookLog("[DXHook] GetSwapChainVtable failed\n");
        return;
    }

    MH_STATUS status = MH_Initialize();
    if (status != MH_OK && status != MH_ERROR_ALREADY_INITIALIZED) {
        HookLog("[DXHook] MH_Initialize failed: %s\n", MHStatusToString(status));
        return;
    }
    HookLog("[DXHook] MH_Initialize: %s\n", MHStatusToString(status));

    // Hook Present
    status = MH_CreateHook(presentAddr, (void*)HookedPresent, (void**)&g_OriginalPresent);
    HookLog("[DXHook] MH_CreateHook(Present): %s\n", MHStatusToString(status));
    if (status != MH_OK) return;

    // Hook ResizeBuffers
    status = MH_CreateHook(resizeAddr, (void*)HookedResizeBuffers, (void**)&g_OriginalResizeBuffers);
    HookLog("[DXHook] MH_CreateHook(ResizeBuffers): %s\n", MHStatusToString(status));
    if (status != MH_OK) return;

    // Hook ExecuteCommandLists for DX12 command queue capture.
    // Required for DX12 games (LJ). For DX11 games (YLAD) the hook fires briefly
    // but is disabled inside InitDX11() once DX11 backend is confirmed.
    if (GetExecuteCommandListsAddr(&g_ExeCmdAddr)) {
        status = MH_CreateHook(g_ExeCmdAddr, (void*)HookedExecuteCommandLists, (void**)&g_OriginalExecuteCommandLists);
        HookLog("[DXHook] MH_CreateHook(ExecuteCommandLists): %s\n", MHStatusToString(status));
        if (status != MH_OK) g_ExeCmdAddr = nullptr;
    } else {
        HookLog("[DXHook] DX12 not available, skipping ExecuteCommandLists hook\n");
    }

    status = MH_EnableHook(MH_ALL_HOOKS);
    HookLog("[DXHook] MH_EnableHook(ALL): %s\n", MHStatusToString(status));

    // Verify hooks are actually enabled
    MH_STATUS presentStatus = MH_EnableHook(presentAddr);
    HookLog("[DXHook] Verify Present hook: %s (expect ENABLED)\n", MHStatusToString(presentStatus));
    if (g_ExeCmdAddr) {
        MH_STATUS eclStatus = MH_EnableHook(g_ExeCmdAddr);
        HookLog("[DXHook] Verify ECL hook: %s (expect ENABLED)\n", MHStatusToString(eclStatus));
    }

    HookLog("[DXHook] Hooks installed: Present=%p->%p, Resize=%p->%p, ECL=%p->%p\n",
        presentAddr, (void*)g_OriginalPresent,
        resizeAddr, (void*)g_OriginalResizeBuffers,
        g_ExeCmdAddr, (void*)g_OriginalExecuteCommandLists);
}

__declspec(dllexport) void Register_Present_Function(void* callback)
{
    HookLog("[DXHook] Register_Present_Function(%p) count=%d->%d\n",
        callback, g_PresentCallbackCount, g_PresentCallbackCount + 1);
    if (g_PresentCallbackCount < MAX_PRESENT_CALLBACKS)
        g_PresentCallbacks[g_PresentCallbackCount++] = (ManagedCallback)callback;
}

__declspec(dllexport) void Register_PreFirstFrame_Function(void* callback)
{
    HookLog("[DXHook] Register_PreFirstFrame_Function(%p) count=%d->%d\n",
        callback, g_PreFirstFrameCallbackCount, g_PreFirstFrameCallbackCount + 1);
    if (g_PreFirstFrameCallbackCount < MAX_PRE_FRAME_CALLBACKS)
        g_PreFirstFrameCallbacks[g_PreFirstFrameCallbackCount++] = (ManagedCallback)callback;
}

__declspec(dllexport) void Register_WndProc_Function(void* callback)
{
    if (g_WndProcCallbackCount < MAX_WNDPROC_CALLBACKS)
        g_WndProcCallbacks[g_WndProcCallbackCount++] = (WndProcCallback)callback;
}

__declspec(dllexport) int CheckImGuiContext()
{
    return ImGui::GetCurrentContext() ? 1 : 0;
}

__declspec(dllexport) void* GetGameHwnd()
{
    return (void*)g_GameHwnd;
}

__declspec(dllexport) void* GetFontAtlas()
{
    ImGuiContext* ctx = ImGui::GetCurrentContext();
    if (!ctx) return nullptr;
    return (void*)ImGui::GetIO().Fonts;
}

__declspec(dllexport) void* AddFontFromMemoryTTF(void* font_data, int font_size, float size_pixels, void* font_cfg, void* glyph_ranges)
{
    ImGuiContext* ctx = ImGui::GetCurrentContext();
    if (!ctx) return nullptr;
    ImFontAtlas* atlas = ImGui::GetIO().Fonts;
    if (!atlas) return nullptr;
    // Copy caller's config (or use defaults) and force FontDataOwnedByAtlas=false
    // so ImGui does not call free() on the buffer - the caller owns its memory.
    ImFontConfig cfg;
    if (font_cfg) cfg = *(ImFontConfig*)font_cfg;
    cfg.FontDataOwnedByAtlas = false;
    return (void*)atlas->AddFontFromMemoryTTF(font_data, font_size, size_pixels, &cfg, (const ImWchar*)glyph_ranges);
}

// Custom export: SetWindowFontScale is a C++ method, not emitted by cimgui generator.
// Hexa.NET.ImGui doesn't expose it; client P/Invokes it via ImGuiEx.
__declspec(dllexport) void igSetWindowFontScale(float scale)
{
    if (!ImGui::GetCurrentContext()) return;
    ImGui::SetWindowFontScale(scale);
}

// --- DX12 threaded callback dispatch exports ---
// Managed code creates a .NET thread, calls DX12_WaitForNewFrame() in a loop.
// Each iteration: wait for signal -> run ImGui callbacks -> call DX12_SignalFrameDone().
// This keeps managed code off the DX12 render thread so GC can scan it safely.

__declspec(dllexport) void DX12_EnableThreadedCallbacks()
{
    if (!g_DX12GoEvent) {
        g_DX12GoEvent = CreateEventW(nullptr, FALSE, FALSE, nullptr);   // auto-reset
        g_DX12DoneEvent = CreateEventW(nullptr, FALSE, FALSE, nullptr); // auto-reset
    }
    g_DX12ThreadedMode = true;
    HookLog("[DXHook] DX12 threaded callbacks enabled (go=%p done=%p)\n", g_DX12GoEvent, g_DX12DoneEvent);
}

__declspec(dllexport) void DX12_SignalThreadReady()
{
    // Tell internal native callback thread to exit - the managed poller
    // takes over to keep managed code off native threads (GC-safe).
    g_DX12InternalThreadExit = true;
    if (g_DX12InternalThreadHandle) {
        DWORD wait = WaitForSingleObject(g_DX12InternalThreadHandle, 3000);
        HookLog("[DXHook] Internal thread join: %s\n",
            wait == WAIT_OBJECT_0 ? "exited" : "timeout");
        CloseHandle(g_DX12InternalThreadHandle);
        g_DX12InternalThreadHandle = nullptr;
    }
    g_DX12ThreadReady = true;
    HookLog("[DXHook] DX12 managed thread ready (internal thread retired)\n");
}

// Returns: 1 = frame ready, 0 = shutdown, -1 = timeout (no frame yet)
__declspec(dllexport) int DX12_WaitForNewFrame()
{
    if (g_DX12Shutdown) return 0;
    DWORD result = WaitForSingleObject(g_DX12GoEvent, 2);
    if (g_DX12Shutdown) return 0;
    return (result == WAIT_OBJECT_0) ? 1 : -1;
}

__declspec(dllexport) void DX12_SignalFrameDone()
{
    SetEvent(g_DX12DoneEvent);
}

__declspec(dllexport) bool DX12_NeedPreFirstFrame()
{
    return g_DX12NeedPreFirstFrame;
}

__declspec(dllexport) void DX12_FirePreFirstFrame()
{
    HookLog("[DXHook] DX12_FirePreFirstFrame: firing %d callbacks on managed thread\n", g_PreFirstFrameCallbackCount);
    FirePreFirstFrameCallbacks();
    g_DX12NeedPreFirstFrame = false;
    HookLog("[DXHook] DX12_FirePreFirstFrame: done\n");
}

__declspec(dllexport) int GetGfxBackend()
{
    return (int)g_Backend;
}

} // extern "C"
