using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;
using MinHook.NET;


namespace TestProject
{

    public unsafe class Mod : DragonEngineMod
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate uint SpawnHomelessPlayer(IntPtr ptr, uint parent);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void GetPlayerAndCharaID(IntPtr thisObj, IntPtr _r_arg, uint* _r_chara_id, uint* _r_player_id);

        private bool m_hookTest = false;

        private static GetPlayerAndCharaID m_deleg;

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

            m_deleg = new GetPlayerAndCharaID(GetPlayerAndCharaIDEYYY);
            MinHookHelper.createHook<GetPlayerAndCharaID>((IntPtr)0x0000000141018F60, m_deleg);
            MinHookHelper.enableAllHook();

            Thread thread = new Thread(InputThread);
            thread.Start();
        }
            

        public static void GetPlayerAndCharaIDEYYY(IntPtr thisObj, IntPtr _r_arg, uint* _r_chara_id, uint* _r_player_id)
        {
            *_r_chara_id = 3;
            *_r_player_id = 9;
            return;
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
            }
        }
    }
}
