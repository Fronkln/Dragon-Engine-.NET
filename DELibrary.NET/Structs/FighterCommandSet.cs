using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class FighterCommandSet
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTERCOMMANDSET_FINDCOMMANDINFO", CallingConvention = CallingConvention.Cdecl)]
        public static extern short DELib_FighterCommandSet_FindCommandInfo(IntPtr set, string name);

        public IntPtr Pointer;

        public short FindCommandInfo(string name)
        {
            return DELib_FighterCommandSet_FindCommandInfo(Pointer, name);
        }
    }
}
