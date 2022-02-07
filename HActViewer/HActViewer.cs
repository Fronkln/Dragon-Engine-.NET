using System;
using System.Threading;
using DragonEngineLibrary;
using ImGuiNET;

namespace HActViewer
{
    public class Mod : DragonEngineMod
    {

        public void InputThread()
        {

            //A while(true) loop is not dangerous here because it's a seperate thread.
            //It does not block any other functions
            while (true)
            {
                AuthPlay currentHAct = AuthManager.PlayingScene;

                if (currentHAct.IsValid())
                {
                    if (DragonEngine.IsKeyHeld(VirtualKey.LeftShift))
                    {
                        if (DragonEngine.IsKeyDown(VirtualKey.P))
                            currentHAct.SetSpeed(1);

                        if (DragonEngine.IsKeyDown(VirtualKey.S))
                            currentHAct.SetSpeed(0);

                        if (DragonEngine.IsKeyDown(VirtualKey.T))
                            currentHAct.Restart();
                    }
                }

            }
        }

        public override void OnModInit()
        {
            base.OnModInit();

            DragonEngine.Initialize();
            DragonEngine.RegisterJob(ModUpdate, DEJob.Update);

            DragonEngineLibrary.Advanced.ImGui.Init();
            DragonEngineLibrary.Advanced.ImGui.RegisterUIUpdate(ModDrawUI);

            Thread thread = new Thread(InputThread);
            thread.Start();

            DragonEngine.Log("Post init");
        }

        public static void ModDrawUI()
        {
            AuthPlay currentHAct = AuthManager.PlayingScene;

            //doesnt seem to change anything
            bool open = currentHAct.IsValid();
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, 300), ImGuiCond.FirstUseEver);

            if (open)
            {
                if (ImGui.Begin("HAct Timeline", ref open, ImGuiWindowFlags.MenuBar))
                {
                    int test = (int)currentHAct.GetGameTick();
                    ImGui.SliderInt("Test", ref test, 0, (int)currentHAct.GetEndTick());

                    if (ImGui.Button("Stop"))
                        currentHAct.SetSpeed(0);

                    ImGui.SameLine(0, 30);

                    if (ImGui.Button("Play"))
                        currentHAct.SetSpeed(1);

                    ImGui.SameLine(0, 45);

                    if (ImGui.Button("Reset"))
                        currentHAct.Restart();

                    ImGui.End();
                }
            }
        }

        public static void ModUpdate()
        {
        }
    }
}
