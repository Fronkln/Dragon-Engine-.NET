using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class TurnCommandDecideManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_TURNCOMMANDDECIDEMANAGER_HANDLEENEMYAI", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        public static extern bool DELib_TurnCommandDecideManager_HandleEnemyAI(IntPtr manager, IntPtr dest, IntPtr opt);

        public IntPtr Pointer;

    }
}
