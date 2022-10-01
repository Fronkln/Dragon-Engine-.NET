using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class BattleResult
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLE_RESULT_MANAGER_FORCE_FINISH", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ForceFinish();
    }
}
