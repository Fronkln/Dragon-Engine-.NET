using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class HActManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_TEST", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern void DELib_HActManager_Test();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_REQUESTHACTPROC", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_RequestHActProc(ref HActRequestOptions opt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_REQUESTHACT", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_RequestHAct(ref HActRequestOptions opt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_SKIP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HActManager_Skip();


        /// <summary>
        /// Request HAct.
        /// </summary>
        public static bool RequestHActProc(HActRequestOptions opt)
        {
            return DELib_HActManager_RequestHActProc(ref opt);
        }

        /// <summary>
        /// Request HAct.
        /// </summary>
        public static bool RequestHAct(HActRequestOptions opt)
        {
            return DELib_HActManager_RequestHAct(ref opt);
        }

        /// <summary>
        /// Skip HAct.
        /// </summary>
        public static void Skip()
        {
            DELib_HActManager_Skip();
        }
    }
}
