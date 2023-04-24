using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;
using MinHook.NET;


namespace TestProject
{

    public class Mod : DragonEngineMod
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate uint SpawnHomelessPlayer(IntPtr ptr, uint parent);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private unsafe delegate uint* EntityBaseGetSceneEntity(IntPtr ent, uint* dest, uint type);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private unsafe delegate void HomelessManagerSetup(IntPtr ent);

        private bool m_hookTest = false;

        private static SpawnHomelessPlayer SpawnHomelessPlayerFunc = (SpawnHomelessPlayer)Marshal.GetDelegateForFunctionPointer((IntPtr)0x14191FD10, typeof(SpawnHomelessPlayer));

        public unsafe override void OnModInit()
        {
            base.OnModInit();

            DragonEngine.Initialize();
            DragonEngine.RegisterJob(Update, DEJob.Update);

            DragonEngine.Log("Hello World!");

            try
            {
                MinHookHelper.initialize();
            }
            catch { }

            _getSceneEntDelegate = new EntityBaseGetSceneEntity(GetSceneEnityHook);
            _homelessSetupDelegate = new HomelessManagerSetup(HomelessManagerSetupHook);

            MinHookHelper.createHook((IntPtr)0x141ECE700, _getSceneEntDelegate, out _getSceneEntTrampoline);
           // MinHookHelper.createHook((IntPtr)0x141900C10, _homelessSetupDelegate, out _homelessSetupTrampoline);

            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F9632, 5);
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F963D, 5);
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F9657, 5);
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F966D, 5);

            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F967A, 3);
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F967D, 4);
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F9681, 4);

            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F9685, 5);
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F968A, 6);

            
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F96C7, 5);
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F96D2, 5);
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F96EC, 5);
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F9702, 5);

            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F9719, 11);


            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F9762, 5);
            DragonEngineLibrary.Unsafe.CPP.NopMemory((IntPtr)0x1418F976D, 5);
            

            MinHookHelper.enableAllHook();

            Thread thread = new Thread(InputThread);
            thread.Start();
        }
            

        public void InputThread()
        {
            while (true)
            {
                if (DragonEngine.IsKeyDown(VirtualKey.Numpad1))
                {
                    m_hookTest = !m_hookTest;
                    DragonEngine.Log("Toggle hook " + m_hookTest);
                }
            }
        }

        public void Update()
        {

            if (DragonEngine.IsKeyHeld(VirtualKey.Numpad2))
            {
                DragonEngine.Log("You should spawn homeless player NOW");

                SpawnHomelessPlayer resFunc = (SpawnHomelessPlayer)Marshal.GetDelegateForFunctionPointer((IntPtr)0x14191FD10, typeof(SpawnHomelessPlayer));
                uint res = resFunc.Invoke(Marshal.AllocHGlobal(8), DragonEngine.GetHumanPlayer().UID);

                new EntityHandle<Character>(res).Get().RequestWarpPose(new PoseInfo(DragonEngine.GetHumanPlayer().Transform.Position, 0));

                DragonEngine.Log("Its joever. " + res);
            }
        }

        private static EntityBaseGetSceneEntity _getSceneEntDelegate;
        private static EntityBaseGetSceneEntity _getSceneEntTrampoline;
        public unsafe uint* GetSceneEnityHook(IntPtr ent, uint* dest, uint type)
        {
            if (m_hookTest)
            {
                uint UID = SceneService.CurrentScene.Get().GetSceneEntity((SceneEntity)type).UID;
                *dest = UID;

                return dest;
            }
            else
                return _getSceneEntTrampoline(ent, dest, type);
        }

        private static HomelessManagerSetup _homelessSetupDelegate;
        private static HomelessManagerSetup _homelessSetupTrampoline;
        public unsafe void HomelessManagerSetupHook(IntPtr mng)
        {
            EntityBase homelessManager = new EntityBase() { Pointer = mng };


            DragonEngine.Log("about to be homeresu");

            uint homeless = SpawnHomelessPlayerFunc(mng, homelessManager.UID);

            DragonEngine.Log("now we are homeresu...");

            _homelessSetupTrampoline(mng);
        }
    }
}
