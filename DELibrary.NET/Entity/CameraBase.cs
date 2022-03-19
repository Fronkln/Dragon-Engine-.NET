using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class CameraBase : EntityBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCAMERA_BASE_SLEEP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_CameraBase_Sleep(IntPtr camera);

        public void Sleep()
        {
            DELib_CameraBase_Sleep(Pointer);
        }
    }
}
