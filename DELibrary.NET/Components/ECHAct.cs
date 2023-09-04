using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECHAct : ECCharaComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_HACT_GET_PLAY_INFO", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELibrary_ECHAct_GetPlayInfo(IntPtr hact, uint type, ref HActRangeInfo inInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_HACT_GET_PLAY_INFO", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELibrary_ECHAct_GetPlayInfo2(IntPtr hact, uint type, IntPtr in_ptr);

        public unsafe bool GetPlayInfo(ref HActRangeInfo input, HActRangeType type)
        {
            bool result = DELibrary_ECHAct_GetPlayInfo(Pointer, (uint)type, ref input);
            return result;
        }

        public unsafe bool GetPlayInfo(IntPtr in_ptr, HActRangeType type)
        {
            bool result = DELibrary_ECHAct_GetPlayInfo2(Pointer, (uint)type, in_ptr);
            return result;
        }
    }
}
