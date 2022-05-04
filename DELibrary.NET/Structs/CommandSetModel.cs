using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class CommandSetModel
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_COMMANDSETMODEL_SETCOMMANDSET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_CommandSetModel_SetCommandSet(IntPtr model, uint type, BattleCommandSetID set);

        internal IntPtr _ptr;

        public void SetCommandSet(uint type, BattleCommandSetID set)
        {
            DELib_CommandSetModel_SetCommandSet(_ptr, type, set);
        }
    }
}
