using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class HumanMode
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODE_GETTER_PARENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HumanMode_Getter_Parent(IntPtr mode);

        public IntPtr m_pointer;


        public HumanModeManager Parent
        {
            get
            {
                return new HumanModeManager() { Pointer = DELib_HumanMode_Getter_Parent(m_pointer) };
            }
        }
    }
}
