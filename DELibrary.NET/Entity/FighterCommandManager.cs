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

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTERCOMMANDMANAGER_GETSET", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_FighterCommandManager_GetSet(uint setID);

        public static FighterCommandSet GetSet(uint setID)
        {
            FighterCommandSet set = new FighterCommandSet();
            set.Pointer = DELib_FighterCommandManager_GetSet(setID);

            return set;
        }

        public static short GetCommandID(uint setID, string commandName)
        {
            FighterCommandSet set = GetSet(setID);
            return set.FindCommandInfo(commandName);
        }
    }
}
