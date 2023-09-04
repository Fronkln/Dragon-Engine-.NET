using System;
using System.Runtime.InteropServices;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;
using ImGuiNET;
using MinHook.NET;

namespace Y7DebugTools
{
    public static class CameraMenu
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate void CameraSetInfo(IntPtr cam, IntPtr inf);

        public static bool Open;
        public static bool Freecam;

        private static CameraSetInfo _setInfDeleg;
        private static CameraSetInfo _setInfTrampoline;

        private static Vector3 m_pos;

        private static bool getPosDoOnce = false;

        private static float m_rotUp;
        private static float m_rotRight;

        static CameraMenu()
        {
            _setInfDeleg = new CameraSetInfo(SetInfo);

            MinHookHelper.createHook((IntPtr)0x1402A4BF0, _setInfDeleg, out _setInfTrampoline);
            MinHookHelper.enableAllHook();
        }

        private static void SetInfo(IntPtr camPtr, IntPtr inf)
        {
            if(!Freecam)
            {
                _setInfTrampoline(camPtr, inf);
                return;
            }

            DragonEngine.GetHumanPlayer().Status.SetNoInputTemporary();


            CameraBase cam = new CameraBase() { Pointer = camPtr };
            float speed = 0.05f;

            Vector3 add = Vector3.zero;

            if (DragonEngine.IsKeyHeld(VirtualKey.W))
                add += cam.Transform.forwardDirection;
            if (DragonEngine.IsKeyHeld(VirtualKey.S))
                add += -cam.Transform.forwardDirection;
            if (DragonEngine.IsKeyHeld(VirtualKey.A))
                add += -cam.Transform.rightDirection;
            if (DragonEngine.IsKeyHeld(VirtualKey.D))
                add += cam.Transform.rightDirection;

            if (DragonEngine.IsKeyHeld(VirtualKey.Q))
                add += -cam.Transform.upDirection;
            if (DragonEngine.IsKeyHeld(VirtualKey.E))
                add += cam.Transform.upDirection;

            m_pos += add * speed;


            if(DragonEngine.IsKeyHeld(VirtualKey.Up))
                m_rotUp += speed;

            if (DragonEngine.IsKeyHeld(VirtualKey.Down))
                m_rotUp -= speed;


            if (DragonEngine.IsKeyHeld(VirtualKey.Left))
                m_rotRight -= speed;

            if (DragonEngine.IsKeyHeld(VirtualKey.Right))
                m_rotRight += speed;

            const float min = -1.5f;
            const float max = 1.5f;

            if (m_rotUp < min)
                m_rotUp = min;
            else if (m_rotUp > max)
                m_rotUp = max;

            unsafe
            {
                Vector4* pos =  (Vector4*)(camPtr.ToInt64() + 0x80);
                Vector4* focusPos = (Vector4*)(camPtr.ToInt64() + 0x90);
                uint* lookatEnt = (uint*)(camPtr.ToInt64() + 0xC0);

                DragonEngine.Log(*lookatEnt);

                if (getPosDoOnce)
                {
                    m_pos = (Vector3)(*pos);
                    getPosDoOnce = false;
                }

                *pos = m_pos;
                *focusPos = m_pos + cam.Transform.forwardDirection + new Vector3(m_rotRight, m_rotUp, 0); 
            }
        }




        public static void Draw()
        {
            ImGui.Begin("Camera");

            if(ImGui.Button("Create Static Camera Test"))
            {
                CameraStatic newCam = CameraStatic.Create(DragonEngineLibrary.Service.SceneService.CurrentScene.UID);
                DragonEngine.Log("New Cam: " + newCam.UID + " " + newCam.Pointer.ToString("x"));
            }

            if(ImGui.Checkbox("Freecam", ref Freecam))
            {
                m_pos = SceneService.CurrentScene.Get().GetSceneEntity<CameraBase>(SceneEntity.camera_free).Get().GetPosCenter();

                if (Freecam)
                    getPosDoOnce = true;
            }

            ImGui.End();
        }
    }
}
