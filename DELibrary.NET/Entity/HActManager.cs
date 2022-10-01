using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class HActManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_REQUESTHACTPROC", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_RequestHActProc(ref HActRequestOptions opt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_REQUESTNEXTHACT", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_RequestNextHAct(ref HActRequestOptions opt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_REQUESTHACT", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_RequestHAct(ref HActRequestOptions opt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_SKIP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HActManager_Skip();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_ISPLAYING", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_IsPlaying();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_FINDRANGE", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_FindRange(Vector4 pos, HActRangeType type, ref HActRangeInfo outInf);


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
        /// Request HAct.
        /// </summary>
        public static bool RequestNextHAct(HActRequestOptions opt)
        {
            return DELib_HActManager_RequestNextHAct(ref opt);
        }

        ///<summary>Are any HActs playing?</summary>
        public static bool IsPlaying()
        {
            return DELib_HActManager_IsPlaying();
        }

        /// <summary>
        /// Skip the current HAct that is playing.
        /// </summary>
        public static void Skip()
        {
            DELib_HActManager_Skip();
        }

        public static bool FindRange(Vector4 pos, HActRangeType rangeType, ref HActRangeInfo outInf)
        {
            return DELib_HActManager_FindRange(pos, rangeType, ref outInf);
        }
    }
}
