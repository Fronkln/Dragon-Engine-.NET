using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;
using System.Security;
using System.IO;


//Advanced because the mere use of this can introduce decent amount of bugs
namespace DragonEngineLibrary.Advanced
{
    public static class ImGui
    {
        internal delegate void DX11Present();
        private static List<DX11Present> _dx11Delegates = new List<DX11Present>();

        public static bool toInit = false;

        public static void RegisterUIUpdate(Action func)
        {
            DragonEngine.Log($"[ImGui] RegisterUIUpdate: {func.Method.Name}");
            // Wrap in try/catch so exceptions don't propagate into native SEH
            // (which swallows them with no diagnostic info on DX12)
            string funcName = func.Method.Name;
            Action safeFunc = CreateSafeCallback(func, funcName);
            DX11Present del = new DX11Present(safeFunc);
            _dx11Delegates.Add(del);
            // JIT-compile the target on this thread so type initializers don't run
            // on the DX11 render thread when the native callback fires.
            try { System.Runtime.CompilerServices.RuntimeHelpers.PrepareDelegate(del); } catch { }

            DXHook.DELibrary_DXHook_RegisterPresentFunc(Marshal.GetFunctionPointerForDelegate(del));
            DragonEngine.Log("[ImGui] RegisterUIUpdate done");
        }

        /// <summary>
        /// Register a callback that fires after ImGui context creation but before the first NewFrame.
        /// Font atlas is unlocked at this point - add custom fonts here.
        /// </summary>
        public static void RegisterPreFirstFrame(Action func)
        {
            DragonEngine.Log($"[ImGui] RegisterPreFirstFrame: {func.Method.Name}");
            DX11Present del = new DX11Present(func);
            _dx11Delegates.Add(del);
            // Same JIT pre-warm for pre-first-frame callbacks.
            try { System.Runtime.CompilerServices.RuntimeHelpers.PrepareDelegate(del); } catch { }

            DXHook.DELibrary_DXHook_RegisterPreFirstFrameFunc(Marshal.GetFunctionPointerForDelegate(del));
            DragonEngine.Log("[ImGui] RegisterPreFirstFrame done");
        }

        internal delegate void WndProcDelegate(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        private static List<WndProcDelegate> _wndProcDelegates = new List<WndProcDelegate>();

        /// <summary>
        /// Register a WndProc callback through cimgui's window subclass.
        /// The DX11 hook subclasses the game window and forwards messages here.
        /// ImGui_ImplWin32_WndProcHandler is called automatically before these callbacks.
        /// </summary>
        public static void RegisterWndProc(Action<IntPtr, int, IntPtr, IntPtr> func)
        {
            WndProcDelegate del = new WndProcDelegate(func);
            _wndProcDelegates.Add(del);

            DXHook.DELibrary_DXHook_RegisterWndProcFunc(Marshal.GetFunctionPointerForDelegate(del));
        }

        public static void Init()
        {
            DragonEngine.Log("[ImGui] Init() called");
            string libPath = Path.Combine(Library.Root, "Y7Internal.dll");
            string cimguiPath = Path.Combine(new FileInfo(libPath).Directory.FullName, "cimgui.dll");

            DragonEngine.Log($"[ImGui] cimgui path: {cimguiPath} exists={File.Exists(cimguiPath)}");

            if (File.Exists(cimguiPath))
            {
                DragonEngine.Log("[ImGui] Loading cimgui.dll");
                IntPtr h = DragonEngine.LoadLibrary(cimguiPath);
                DragonEngine.Log($"[ImGui] cimgui.dll handle: {h}");
                VerifyModules(h);
            }

            DragonEngine.Log("[ImGui] Calling DXHook.Init()");
            DXHook.Init();
            DragonEngine.Log("[ImGui] DXHook.Init() returned");

            // Fix Hexa.NET-style bindings that loaded a wrong cimgui.dll copy
            FixupBindings();
        }

        // Check that each compiled-in module exported at least one known symbol
        private static void VerifyModules(IntPtr hCimgui)
        {
            Check(hCimgui, "core",     "igBegin");
            Check(hCimgui, "hook",     "InitDX11Hook");
            Check(hCimgui, "ImGuizmo", "ImGuizmo_BeginFrame");
            Check(hCimgui, "implot",   "ImPlot_BeginPlot");
            Check(hCimgui, "imnodes",  "ImNodes_BeginNodeEditor");
        }

        private static Action CreateSafeCallback(Action func, string name)
        {
            return () => SafeInvoke(func, name);
        }

        private static int s_fixupCalls = 0;

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private static void SafeInvoke(Action func, string name)
        {
            // Re-run fixup for the first ~20 callback invocations to catch mods
            // whose type initializers fire on the first frame's func() call.
            // Frame 1: cctors fire, HexaGen.Runtime loads. Frame 2+: fixup finds and patches.
            if (s_fixupCalls < 20)
            {
                s_fixupCalls++;
                s_fixupDone = false;
                FixupBindings();
            }
            try { func(); }
            catch (Exception ex)
            {
                DragonEngine.Log($"[ImGui] Callback '{name}' exception: {ex}");
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetModuleHandleA(string lpModuleName);

        private static void Check(IntPtr hModule, string label, string symbol)
        {
            bool ok = DXHook.GetProcAddress(hModule, symbol) != IntPtr.Zero;
            DragonEngine.Log($"[ImGui] {(ok ? "✓" : "✗")} {label} ({symbol})");
        }

        // Hexa.NET-style bindings (ImGui, ImPlot, ImNodes, ImGuizmo, ImPlot3D) load
        // cimgui.dll via LibraryLoader which probes NuGet runtime dirs. In a game mod
        // environment those dirs don't exist, so the cctor either fails silently or
        // loads a second copy of cimgui.dll from a stale NuGet cache. Either way the
        // FunctionTable ends up null or pointing at the wrong module.
        //
        // This method scans all loaded assemblies for Hexa-style binding classes and
        // re-initializes their function tables against the cimgui.dll that DELibrary
        // already loaded (the one with the active ImGui context).
        private static bool s_fixupDone = false;
        private static void FixupBindings()
        {
            if (s_fixupDone) return;
            s_fixupDone = true;

            try
            {
                // Find HexaGen.Runtime assembly and the NativeLibraryContext type.
                // It may not be loaded yet (CLR loads on demand), so try loading it
                // from the same directory as DELibrary.NET if not found.
                System.Type ctxType = null;
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (asm.GetName().Name != "HexaGen.Runtime") continue;
                    ctxType = asm.GetType("HexaGen.Runtime.NativeLibraryContext");
                    break;
                }

                if (ctxType == null)
                {
                    // Try loading from our directory (srmm-libs ships HexaGen.Runtime.dll)
                    string ourDir = Path.GetDirectoryName(typeof(ImGui).Assembly.Location);
                    string hexaPath = Path.Combine(ourDir, "HexaGen.Runtime.dll");
                    if (File.Exists(hexaPath))
                    {
                        try
                        {
                            var hexaAsm = System.Reflection.Assembly.LoadFrom(hexaPath);
                            ctxType = hexaAsm.GetType("HexaGen.Runtime.NativeLibraryContext");
                            DragonEngine.Log("[ImGui] FixupBindings: loaded HexaGen.Runtime from " + hexaPath);
                        }
                        catch (Exception loadEx)
                        {
                            DragonEngine.Log("[ImGui] FixupBindings: failed to load HexaGen.Runtime: " + loadEx.Message);
                        }
                    }
                }

                if (ctxType == null)
                {
                    DragonEngine.Log("[ImGui] FixupBindings: HexaGen.Runtime not available, skipping");
                    return;
                }

                // NativeLibraryContext(string name) - uses kernel32!LoadLibrary which finds
                // the already-loaded cimgui.dll by name
                var ctxCtor = ctxType.GetConstructor(new[] { typeof(string) });
                if (ctxCtor == null)
                {
                    DragonEngine.Log("[ImGui] FixupBindings: NativeLibraryContext(string) ctor not found");
                    return;
                }

                object cimguiCtx = ctxCtor.Invoke(new object[] { "cimgui" });

                // Names of static classes that have InitApi(INativeContext) and funcTable
                string[] targetNames = { "ImGui", "ImPlot", "ImNodes", "ImGuizmo", "ImPlot3D" };
                var bindingFlags = System.Reflection.BindingFlags.Static |
                                   System.Reflection.BindingFlags.NonPublic |
                                   System.Reflection.BindingFlags.Public;

                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        // GetTypes() can throw ReflectionTypeLoadException if the assembly
                        // references a different version of HexaGen.Runtime than what's loaded.
                        // Use the partial results from the exception.
                        System.Type[] types;
                        try
                        {
                            types = asm.GetTypes();
                        }
                        catch (System.Reflection.ReflectionTypeLoadException rtle)
                        {
                            types = rtle.Types;
                        }

                        foreach (var t in types)
                        {
                            if (t == null) continue;
                            if (!t.IsAbstract || !t.IsSealed) continue;
                            if (Array.IndexOf(targetNames, t.Name) < 0) continue;

                            var ft = t.GetField("funcTable", bindingFlags);
                            if (ft == null) continue;

                            // Find InitApi method
                            System.Reflection.MethodInfo initApi = null;
                            foreach (var m in t.GetMethods(bindingFlags))
                            {
                                if (m.Name != "InitApi") continue;
                                var p = m.GetParameters();
                                if (p.Length == 1 && p[0].ParameterType.IsInterface)
                                {
                                    initApi = m;
                                    break;
                                }
                            }

                            if (initApi == null) continue;

                            try
                            {
                                initApi.Invoke(null, new[] { cimguiCtx });
                                DragonEngine.Log($"[ImGui] FixupBindings: {t.FullName} re-initialized");
                            }
                            catch (Exception invokeEx)
                            {
                                DragonEngine.Log($"[ImGui] FixupBindings: {t.FullName} InitApi failed: {invokeEx.InnerException?.Message ?? invokeEx.Message}");
                            }
                        }
                    }
                    catch (Exception asmEx)
                    {
                        DragonEngine.Log($"[ImGui] FixupBindings: {asm.GetName().Name} scan failed: {asmEx.GetType().Name}: {asmEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                DragonEngine.Log($"[ImGui] FixupBindings error: {ex.Message}");
            }
        }
    }
}
