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

        public unsafe bool GetPlayInfo(ref HActRangeInfo input, HActRangeType type)
        {
            bool result = DELibrary_ECHAct_GetPlayInfo(Pointer, (uint)type, ref input);
            return result;
        }
    }
}
