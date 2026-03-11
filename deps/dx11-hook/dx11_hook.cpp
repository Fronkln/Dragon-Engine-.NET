#include <d3d11.h>
#include <d3d12.h>
#include <dxgi.h>
#include <dxgi1_4.h>
#include <cstdio>

#include "imgui.h"
#include "imgui_impl_dx11.h"
#include "imgui_impl_dx12.h"
#include "imgui_impl_win32.h"
#include "MinHook.h"

#pragma comment(lib, "d3d11.lib")
#pragma comment(lib, "d3d12.lib")
#pragma comment(lib, "dxgi.lib")

typedef HRESULT(WINAPI* PFN_Present)(IDXGISwapChain*, UINT, UINT);
typedef HRESULT(WINAPI* PFN_ResizeBuffers)(IDXGISwapChain*, UINT, UINT, UINT, DXGI_FORMAT, UINT);
typedef void(WINAPI* PFN_ExecuteCommandLists)(ID3D12CommandQueue*, UINT, ID3D12CommandList* const*);
typedef void(__stdcall* ManagedCallback)();

static PFN_Present g_OriginalPresent = nullptr;
static PFN_ResizeBuffers g_OriginalResizeBuffers = nullptr;
static PFN_ExecuteCommandLists g_OriginalExecuteCommandLists = nullptr;

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

enum GfxBackend { GFX_NONE, GFX_DX11, GFX_DX12 };
static GfxBackend g_Backend = GFX_NONE;

static bool g_Initialized = false;
static bool g_InitFailed = false;
static HWND g_GameHwnd = nullptr;
static WNDPROC g_OriginalWndProc = nullptr;

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

static void HookLog(const char* fmt, ...)
{
    char buf[512];
    va_list args;
    va_start(args, fmt);
    vsnprintf(buf, sizeof(buf), fmt, args);
    va_end(args);
    OutputDebugStringA(buf);
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

    return true;
}

// ---- ExecuteCommandLists hook (captures the game's command queue) ----

static void WINAPI HookedExecuteCommandLists(ID3D12CommandQueue* queue, UINT numCmdLists, ID3D12CommandList* const* cmdLists)
{
    if (!g_CmdQueue12 && queue) {
        D3D12_COMMAND_QUEUE_DESC desc = queue->GetDesc();
        if (desc.Type == D3D12_COMMAND_LIST_TYPE_DIRECT) {
            g_CmdQueue12 = queue;
            HookLog("[DXHook] Captured game command queue: %p\n", queue);
        }
    }
    g_OriginalExecuteCommandLists(queue, numCmdLists, cmdLists);
}

// ---- Common ----

extern "C" LRESULT ImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);

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
            HookLog("[DXHook] Firing pre-first-frame callback %d\n", i);
            g_PreFirstFrameCallbacks[i]();
        }
    }
}

static bool InitDX11(IDXGISwapChain* swapChain)
{
    HRESULT hr = swapChain->GetDevice(__uuidof(ID3D11Device), (void**)&g_Device11);
    if (FAILED(hr) || !g_Device11)
        return false;

    g_Device11->GetImmediateContext(&g_Context11);

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
    HookLog("[DXHook] DX11 init complete\n");
    return true;
}

static bool InitDX12(IDXGISwapChain* swapChain)
{
    HRESULT hr = swapChain->GetDevice(__uuidof(ID3D12Device), (void**)&g_Device12);
    if (FAILED(hr) || !g_Device12)
        return false;

    HookLog("[DXHook] DX12 device found\n");

    // Wait for command queue capture from ExecuteCommandLists hook
    if (!g_CmdQueue12) {
        HookLog("[DXHook] DX12 waiting for command queue capture...\n");
        g_Device12->Release();
        g_Device12 = nullptr;
        return false; // will retry next Present
    }

    DXGI_SWAP_CHAIN_DESC desc;
    swapChain->GetDesc(&desc);
    HookLog("[DXHook] DX12 init - HWND=%p, %ux%u, BufferCount=%u\n",
        desc.OutputWindow, desc.BufferDesc.Width, desc.BufferDesc.Height, desc.BufferCount);

    // SRV descriptor heap for ImGui font texture
    D3D12_DESCRIPTOR_HEAP_DESC srvHeapDesc = {};
    srvHeapDesc.Type = D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV;
    srvHeapDesc.NumDescriptors = 1;
    srvHeapDesc.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE;
    hr = g_Device12->CreateDescriptorHeap(&srvHeapDesc, IID_PPV_ARGS(&g_SrvDescHeap12));
    if (FAILED(hr)) {
        HookLog("[DXHook] DX12 CreateDescriptorHeap(SRV) failed: 0x%08X\n", hr);
        return false;
    }

    if (!CreateDX12RenderTargets(swapChain))
        return false;

    ImGui::CreateContext();
    ImGuiIO& io = ImGui::GetIO();
    io.ConfigFlags |= ImGuiConfigFlags_NavEnableKeyboard;

    ImGui_ImplWin32_Init(desc.OutputWindow);
    ImGui_ImplDX12_Init(g_Device12,
        (int)g_BufferCount12,
        DXGI_FORMAT_R8G8B8A8_UNORM,
        g_SrvDescHeap12,
        g_SrvDescHeap12->GetCPUDescriptorHandleForHeapStart(),
        g_SrvDescHeap12->GetGPUDescriptorHandleForHeapStart());

    g_GameHwnd = desc.OutputWindow;
    g_OriginalWndProc = (WNDPROC)SetWindowLongPtrW(g_GameHwnd, GWLP_WNDPROC, (LONG_PTR)HookedWndProc);

    FirePreFirstFrameCallbacks();

    g_Backend = GFX_DX12;
    g_Initialized = true;
    HookLog("[DXHook] DX12 init complete\n");
    return true;
}

static HRESULT WINAPI HookedPresent(IDXGISwapChain* swapChain, UINT syncInterval, UINT flags)
{
    if (!g_Initialized && !g_InitFailed) {
        // Try DX12 first (returns false if waiting for command queue), then DX11
        if (!InitDX12(swapChain) && !g_CmdQueue12) {
            // No DX12 device at all, try DX11
            if (!InitDX11(swapChain)) {
                HookLog("[DXHook] Both DX11 and DX12 init failed\n");
                g_InitFailed = true;
            }
        }
        // If DX12 device was found but waiting for queue, just pass through
    }

    if (g_Initialized && g_Backend == GFX_DX11) {
        if (!g_RTV11)
            CreateRTV11(swapChain);

        ImGui_ImplDX11_NewFrame();
        ImGui_ImplWin32_NewFrame();
        ImGui::NewFrame();

        for (int i = 0; i < g_PresentCallbackCount; i++) {
            if (g_PresentCallbacks[i])
                g_PresentCallbacks[i]();
        }

        ImGui::Render();
        g_Context11->OMSetRenderTargets(1, &g_RTV11, nullptr);
        ImGui_ImplDX11_RenderDrawData(ImGui::GetDrawData());
    }
    else if (g_Initialized && g_Backend == GFX_DX12) {
        UINT frameIdx = 0;
        IDXGISwapChain3* sc3 = nullptr;
        if (SUCCEEDED(swapChain->QueryInterface(IID_PPV_ARGS(&sc3)))) {
            frameIdx = sc3->GetCurrentBackBufferIndex();
            sc3->Release();
        }

        g_CmdAlloc12[frameIdx]->Reset();
        g_CmdList12->Reset(g_CmdAlloc12[frameIdx], nullptr);

        D3D12_RESOURCE_BARRIER barrier = {};
        barrier.Type = D3D12_RESOURCE_BARRIER_TYPE_TRANSITION;
        barrier.Transition.pResource = g_BackBuffers12[frameIdx];
        barrier.Transition.StateBefore = D3D12_RESOURCE_STATE_PRESENT;
        barrier.Transition.StateAfter = D3D12_RESOURCE_STATE_RENDER_TARGET;
        barrier.Transition.Subresource = D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES;
        g_CmdList12->ResourceBarrier(1, &barrier);

        g_CmdList12->OMSetRenderTargets(1, &g_RtvHandles12[frameIdx], FALSE, nullptr);
        g_CmdList12->SetDescriptorHeaps(1, &g_SrvDescHeap12);

        ImGui_ImplDX12_NewFrame();
        ImGui_ImplWin32_NewFrame();
        ImGui::NewFrame();

        for (int i = 0; i < g_PresentCallbackCount; i++) {
            if (g_PresentCallbacks[i])
                g_PresentCallbacks[i]();
        }

        ImGui::Render();
        ImGui_ImplDX12_RenderDrawData(ImGui::GetDrawData(), g_CmdList12);

        barrier.Transition.StateBefore = D3D12_RESOURCE_STATE_RENDER_TARGET;
        barrier.Transition.StateAfter = D3D12_RESOURCE_STATE_PRESENT;
        g_CmdList12->ResourceBarrier(1, &barrier);

        g_CmdList12->Close();
        ID3D12CommandList* cmdLists[] = { g_CmdList12 };
        g_CmdQueue12->ExecuteCommandLists(1, cmdLists);
    }

    return g_OriginalPresent(swapChain, syncInterval, flags);
}

static HRESULT WINAPI HookedResizeBuffers(IDXGISwapChain* swapChain, UINT bufferCount, UINT width, UINT height, DXGI_FORMAT format, UINT swapChainFlags)
{
    HookLog("[DXHook] ResizeBuffers %ux%u\n", width, height);

    if (g_Backend == GFX_DX11) {
        CleanupRTV11();
    }
    else if (g_Backend == GFX_DX12) {
        ImGui_ImplDX12_InvalidateDeviceObjects();
        CleanupDX12Buffers();
        if (g_CmdList12) { g_CmdList12->Release(); g_CmdList12 = nullptr; }
        if (g_CmdAlloc12) {
            for (UINT i = 0; i < g_BufferCount12; i++) {
                if (g_CmdAlloc12[i]) { g_CmdAlloc12[i]->Release(); g_CmdAlloc12[i] = nullptr; }
            }
        }
        if (g_RtvDescHeap12) { g_RtvDescHeap12->Release(); g_RtvDescHeap12 = nullptr; }
    }

    HRESULT hr = g_OriginalResizeBuffers(swapChain, bufferCount, width, height, format, swapChainFlags);

    if (g_Backend == GFX_DX12 && SUCCEEDED(hr)) {
        CreateDX12RenderTargets(swapChain);
        ImGui_ImplDX12_CreateDeviceObjects();
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
    HookLog("[DXHook] InitDX11Hook called\n");

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

    // Hook ExecuteCommandLists for DX12 command queue capture
    void* exeCmdAddr = nullptr;
    if (GetExecuteCommandListsAddr(&exeCmdAddr)) {
        status = MH_CreateHook(exeCmdAddr, (void*)HookedExecuteCommandLists, (void**)&g_OriginalExecuteCommandLists);
        HookLog("[DXHook] MH_CreateHook(ExecuteCommandLists): %s\n", MHStatusToString(status));
        // non-fatal if this fails, DX11 games don't need it
    } else {
        HookLog("[DXHook] DX12 not available, skipping ExecuteCommandLists hook\n");
    }

    status = MH_EnableHook(MH_ALL_HOOKS);
    HookLog("[DXHook] MH_EnableHook(ALL): %s\n", MHStatusToString(status));

    HookLog("[DXHook] Hooks installed\n");
}

__declspec(dllexport) void Register_Present_Function(void* callback)
{
    if (g_PresentCallbackCount < MAX_PRESENT_CALLBACKS)
        g_PresentCallbacks[g_PresentCallbackCount++] = (ManagedCallback)callback;
}

__declspec(dllexport) void Register_PreFirstFrame_Function(void* callback)
{
    if (g_PreFirstFrameCallbackCount < MAX_PRE_FRAME_CALLBACKS)
        g_PreFirstFrameCallbacks[g_PreFirstFrameCallbackCount++] = (ManagedCallback)callback;
}

__declspec(dllexport) void Register_WndProc_Function(void* callback)
{
    if (g_WndProcCallbackCount < MAX_WNDPROC_CALLBACKS)
        g_WndProcCallbacks[g_WndProcCallbackCount++] = (WndProcCallback)callback;
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
    return (void*)atlas->AddFontFromMemoryTTF(font_data, font_size, size_pixels, (ImFontConfig*)font_cfg, (const ImWchar*)glyph_ranges);
}

} // extern "C"
