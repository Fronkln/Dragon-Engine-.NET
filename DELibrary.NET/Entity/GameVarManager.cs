using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class GameVarManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CGAME_VAR_MANAGER_GET_CURR_VALUE_BOOL", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_GameVarManager_GetValueBool(uint gameVar);

        public static bool GetValueBool(GameVarID varVal)
        {
            return DELib_GameVarManager_GetValueBool((uint)varVal);
        }
    }
}
