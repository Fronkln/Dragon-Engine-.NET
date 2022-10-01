using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary.Callbacks
{
    public static class PXD
    {
        public delegate void AuthElementCallbackDelegate(IntPtr element);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_PXD_CREATE_AUTH_NODE_CALLBACK", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Pxd_CreateAuthNodeCallback(IntPtr deleg);

        public static IntPtr CreateAuthElementCallback(AuthElementCallbackDelegate deleg)
        {
            AuthElementCallbackDelegate del = new AuthElementCallbackDelegate(deleg);
            return Marshal.GetFunctionPointerForDelegate(del);
        }
    }
}
