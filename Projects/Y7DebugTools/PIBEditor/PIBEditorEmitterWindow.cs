using System;
using ImGuiNET;
using PIBLib;

namespace Y7DebugTools
{
    internal class PIBEditorEmitterWindow
    {
        public static bool Open;
        public static PIBEditorNodePibEmitter EditingEmitter;

        public static void Draw()
        {
            if (!Open || EditingEmitter == null)
                return;

            if(ImGui.Begin("Emitter", ref Open))
            {
                if (ImGui.Button("Flag1"))
                {
                    ShowActiveEmitterFlag1();
                }

                if (ImGui.Button("Flag2"))
                {
                    ShowActiveEmitterFlag2();
                }

                if (ImGui.Button("Flag3"))
                {
                    ShowActiveEmitterFlag3();
                }

                ImGui.End();
            }
        }

        public static void ShowActiveEmitterFlag1()
        {
            long flag1 = 0;

            PibEmitterv52 v52Emitter = EditingEmitter.Emitter as PibEmitterv52;

            if (v52Emitter != null)
                flag1 = (long)v52Emitter.Flags;
            else
                flag1 = EditingEmitter.Emitter.Flags;

            PIBEditorFlagWindow.Init(PIBEditorFlagWindow.GetFlag1List(), flag1, delegate (long val)
            {
                if (v52Emitter != null)
                    v52Emitter.Flags = (ulong)val;
                else
                    EditingEmitter.Emitter.Flags = (int)val;
            });

            PIBEditorFlagWindow.ActiveWindow = PIBEditorFlagWindow.ActiveFlagsWindow.Flag1;
        }

        public static void ShowActiveEmitterFlag2()
        {
            long flag2 = 0;

            PibEmitterv52 v52Emitter = EditingEmitter.Emitter as PibEmitterv52;

            if (v52Emitter != null)
                flag2 = (long)v52Emitter.Flags2;
            else
                flag2 = EditingEmitter.Emitter.Flags2;

            PIBEditorFlagWindow.Init(PIBEditorFlagWindow.GetFlag2List(), flag2, delegate (long val)
            {
                if (v52Emitter != null)
                    v52Emitter.Flags2 = (ulong)val;
                else
                    EditingEmitter.Emitter.Flags2 = (int)val;
            });

            PIBEditorFlagWindow.ActiveWindow = PIBEditorFlagWindow.ActiveFlagsWindow.Flag2;
        }

        public static void ShowActiveEmitterFlag3()
        {
            long flag3 = 0;

            PibEmitterv52 v52Emitter = EditingEmitter.Emitter as PibEmitterv52;

            if (v52Emitter != null)
                flag3 = (long)v52Emitter.Flags3;
            else
                flag3 = EditingEmitter.Emitter.Flags3;

            PIBEditorFlagWindow.Init(PIBEditorFlagWindow.GetFlag3List(), flag3, delegate (long val)
            {
                if (v52Emitter != null)
                    v52Emitter.Flags3 = (ulong)val;
                else
                    EditingEmitter.Emitter.Flags3 = (int)val;
            });

            PIBEditorFlagWindow.ActiveWindow = PIBEditorFlagWindow.ActiveFlagsWindow.Flag3;
        }
    }
}
