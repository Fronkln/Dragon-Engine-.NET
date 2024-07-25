using PIBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using DragonEngineLibrary;

namespace Y7DebugTools
{
    internal static class PIBEditorFlagWindow
    {
        private static PibVersion pibVersion { get { return PIBEditorMainWindow.Pib.Version; } }
        private static string[] m_curList;
        private static bool[] m_checkedList;
        private static Action<long> m_doneDeleg;

        public static bool Open;

        public enum ActiveFlagsWindow
        {
            None,
            Flag1,
            Flag2,
            Flag3,
            OE_Flag4
        }

        public static ActiveFlagsWindow ActiveWindow = ActiveFlagsWindow.None;

        public static void Init(string[] list, long startVal, Action<long> finishedDeleg)
        {
            Open = true;
            m_curList = list;
            m_checkedList = new bool[list.Length];
            m_doneDeleg = finishedDeleg;

            for (int i = 0; i < list.Length; i++)
            {
                if ((startVal & ((long)1 << i)) != 0)
                    m_checkedList[i] = true;
            }
        }


        public static void Apply()
        {
            long finalVal = 0;

            for (int i = 0; i < m_checkedList.Length; i++)
            {
                if (m_checkedList[i])
                    finalVal += (long)1 << i;
            }

            m_doneDeleg?.Invoke(finalVal);
        }

        public static void Draw()
        {
            if (!Open)
            {
                ActiveWindow = ActiveFlagsWindow.None;
                return;
            }

            if (ImGui.Begin("Flag Editor", ref Open))
            {
                for (int i = 0; i < m_curList.Length; i++)
                {
                    if (ImGui.Checkbox(m_curList[i] + "##" + i, ref m_checkedList[i]))
                        Apply();
                }

                ImGui.End();
            }
        }

        public static string[] GetFlag1List()
        {
            string[] values = null;

            if (pibVersion >= PibVersion.YLAD)
                values = Enum.GetNames(typeof(EmitterFlag1v52));
            if (pibVersion == PibVersion.Y0 || pibVersion == PibVersion.Ishin)
                values = Enum.GetNames(typeof(EmitterFlag1v27));
            if (pibVersion == PibVersion.Y3)
                values = Enum.GetNames(typeof(EmitterFlag1v19));
            if (pibVersion == PibVersion.Y5)
                values = Enum.GetNames(typeof(EmitterFlag1v21));
            if (pibVersion == PibVersion.Y6)
                values = Enum.GetNames(typeof(EmitterFlag1v29));
            if (pibVersion == PibVersion.YK2)
                values = Enum.GetNames(typeof(EmitterFlag1v43));
            if (pibVersion == PibVersion.JE)
                values = Enum.GetNames(typeof(EmitterFlag1v45));

            if (values == null)
            {
                List<string> strings = new List<string>();

                for (int i = 0; i < GetFlagLength(); i++)
                    strings.Add("Flag " + (i + 1));

                values = strings.ToArray();
            }

            return values;
        }

        public static string[] GetFlag2List()
        {
            string[] values = null;

            if (pibVersion >= PibVersion.Y6)
                values = Enum.GetNames(typeof(EmitterFlag2v52));

            if (pibVersion <= PibVersion.Y5)
                values = Enum.GetNames(typeof(EmitterFlag2v21));

            if (values == null)
            {
                List<string> strings = new List<string>();

                for (int i = 0; i < GetFlagLength(); i++)
                    strings.Add("Flag " + (i + 1));

                values = strings.ToArray();
            }

            return values;
        }

        public static string[] GetFlag3List()
        {
            string[] values = null;
            if (pibVersion == PibVersion.Y0 || pibVersion == PibVersion.Ishin)
                values = Enum.GetNames(typeof(EmitterFlag3v27));

            if (pibVersion >= PibVersion.YLAD)
                values = Enum.GetNames(typeof(EmitterFlag3v52));
            else if (pibVersion > PibVersion.Y6)
                values = Enum.GetNames(typeof(EmitterFlag3v43));
            else if (pibVersion == PibVersion.Y6)
                values = Enum.GetNames(typeof(EmitterFlag3v29));

            if (values == null)
            {
                List<string> strings = new List<string>();

                for (int i = 0; i < GetFlagLength(); i++)
                    strings.Add("Flag " + (i + 1));

                values = strings.ToArray();
            }

            return values;
        }

        public static string[] GetOEFlag4List()
        {
            string[] values = null;

            if (pibVersion == PibVersion.Y5)
                values = Enum.GetNames(typeof(EmitterFlag4v21));
            else if (pibVersion >= PibVersion.Ishin)
                values = Enum.GetNames(typeof(EmitterFlag4v27));

            if (values == null)
            {
                List<string> strings = new List<string>();

                for (int i = 0; i < GetFlagLength(); i++)
                    strings.Add("Flag " + (i + 1));

                values = strings.ToArray();
            }

            return values;
        }

        private static int GetFlagLength()
        {
            if (PIBEditorMainWindow.IsDEPib())
                return 64; //long
            else
                return 32; //int
        }
    }
}
