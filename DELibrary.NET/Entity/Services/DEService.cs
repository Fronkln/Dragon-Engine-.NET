using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class DEService
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSERVICE_SERVICE_NAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Service_GetServiceName(uint id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSERVICE_SERVICE_POINTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Service_GetServicePointer(uint id);

        public static string GetServiceName(uint id)
        {
            return Marshal.PtrToStringAnsi(DELib_Service_GetServiceName(id));
        }

        public static IntPtr GetServicePointer(uint id)
        {
            return DELib_Service_GetServicePointer(id);
        }
    }
}
