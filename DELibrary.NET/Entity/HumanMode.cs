using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class HumanMode
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODE_GETTER_PARENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HumanMode_Getter_Parent(IntPtr mode);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODE_GETTER_MODENAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HumanMode_Getter_ModeName(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODE_GET_COMMAND_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern FighterCommandID DELib_HumanMode_GetCommandID(IntPtr mode);

        public IntPtr m_pointer;

        public string ModeName
        {
            get
            {
                IntPtr strPtr = DELib_HumanMode_Getter_ModeName(m_pointer);

                if (strPtr == IntPtr.Zero)
                    return "";
                else
                    return Marshal.PtrToStringAnsi(strPtr);
            }
        }

        public HumanModeManager Parent
        {
            get
            {
                return new HumanModeManager() { Pointer = DELib_HumanMode_Getter_Parent(m_pointer) };
            }
        }

        public FighterCommandID GetCommandID()
        {
            return DELib_HumanMode_GetCommandID(m_pointer);
        }
    }
}
