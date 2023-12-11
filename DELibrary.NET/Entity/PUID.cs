using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class PUID
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_PUID_GET_BINARY_PTR", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_PUID_GetBinaryPointer(uint id);

        public static IntPtr GetBinaryPointer(uint puidID)
        {
            return DELib_PUID_GetBinaryPointer(puidID);
        }
    }
}
