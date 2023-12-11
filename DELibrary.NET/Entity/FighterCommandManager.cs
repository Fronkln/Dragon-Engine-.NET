using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public static class FighterCommandManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTERCOMMANDMANAGER_FINDSETID1", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FindSetID(string name);
    }
}
