using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECControllerPadViewLocal : ECGameComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CONTROLLER_PAD_VIEW_LOCAL_GETTER_PAD_LISTENER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_ECControllerPadViewLocal_Getter_PadListener(IntPtr pad);

        public PadListener PadListener
        {
            get
            {
                return new PadListener() { Pointer = DELib_ECControllerPadViewLocal_Getter_PadListener(Pointer) };
            }
        }
    }
}
