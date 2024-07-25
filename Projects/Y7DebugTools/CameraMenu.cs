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
        public static bool Open;
        public static bool Freecam;

        private static Vector3 m_pos;

        private static bool getPosDoOnce = false;

        private static float m_rotUp;
        private static float m_rotRight;

        private static void SetInfo(IntPtr camPtr, IntPtr inf)
        { 

        }

        public static void Draw()
        {
            ImGui.Begin("Camera");

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
