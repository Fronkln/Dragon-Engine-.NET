using System;
using System.Security.Cryptography.X509Certificates;
using DragonEngineLibrary;
using ImGuiNET;
using PIBLib;

namespace Y7DebugTools
{
    internal static class PIBEditorMainWindow
    {
        public static BasePib Pib;
        public static PIBEditorNodePib PibRoot;

        public static bool WaitingUnload;
        public static bool WaitingLoad;

        public static string TestSavePath = "mods/Pib/particle/yazawa/raw_event/yjh0014.pib";
        public static string TestWorkFile = "mods/Pib/particle/yazawa/raw_event/yjh0014.pib";
        public const ParticleID TestID = (ParticleID)8671;

        public static EntityHandle<ParticleInterface> PlayingPib = new EntityHandle<ParticleInterface>();

        static PIBEditorMainWindow()
        {
            Ini ini = new Ini("mods/DebugTools/particle.ini");

            TestSavePath = ini.GetValue("ParticlePath");
        }

        public static void InputThread()
        {
        }

        public static void GameUpdate()
        {
            if (WaitingUnload)
                if (!ParticleManager.IsLoadedRaw(TestID))
                {
                    //ParticleManager.UnloadRaw(TestID);
                    WaitingUnload = false;
                    WaitingLoad = true;

                    ParticleManager.LoadRaw(TestID, true);

                    return;
                }

            if (WaitingLoad)
            {
                if (ParticleManager.IsLoadedRaw(TestID))
                {
                    SoundManager.PlayCue(1, 1, 0);
                    WaitingLoad = false;
                    WaitingUnload = false;

                    return;
                }
            }
        }

        public static void Draw()
        {
            if(ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    ImGui.Button("Load");
                    PIBEditorFileWindow.Open = true;
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar(); 
            }

            PIBEditorFileWindow.Draw();
            PIBEditorPibWindow.Draw();
            PIBEditorEmitterWindow.Draw();
            PIBEditorFlagWindow.Draw();
        }

        public static void ClearPib()
        {
            if (PlayingPib.IsValid())
                PlayingPib.Get().DestroyEntity();
        }

        public static void Play()
        {
            ClearPib();

            Character player = DragonEngine.GetHumanPlayer();

            /*
            DragonEngineLibrary.Matrix4x4 mtx = new DragonEngineLibrary.Matrix4x4();
            mtx.Position = player.Transform.Position + (player.Transform.forwardDirection * 0.5f) + (player.Transform.upDirection * 0.1f);
            PlayingPib = ParticleManager.Play(TestID, mtx, ParticleType.None);
            */

            DragonEngineLibrary.Matrix4x4 playPos = DragonEngine.GetHumanPlayer().GetMatrix();
            playPos.Position = new DragonEngineLibrary.Vector4(playPos.Position.x, playPos.Position.y + 1.5f, playPos.Position.z, 0);
            PlayingPib = ParticleManager.Play(TestID, playPos, ParticleType.None);


            DragonEngine.Log("PTC Interface" + PlayingPib);
        }

        public static void Reload()
        {
            ClearPib();

            PIB.Write(Pib, TestSavePath);

            if(ParticleManager.IsLoadedRaw(TestID))
                ParticleManager.UnloadRaw(TestID);

            WaitingUnload = true;
        }

        public static void OpenPIB(string path)
        {
            Pib = PIB.Read(path);

            PibRoot = new PIBEditorNodePib(Pib);
            PIBEditorPibWindow.OnLoad();
            WaitingLoad = true;
            ParticleManager.LoadRaw(TestID, true);
        }

        public static bool IsDEPib()
        {
            return Pib.Version > PibVersion.Y0;
        }
    }
}
