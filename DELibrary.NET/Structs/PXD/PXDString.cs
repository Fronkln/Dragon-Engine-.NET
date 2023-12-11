using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public class PXDString
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_PXD_STRING_GET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_PXDString_Getter_String(IntPtr ptr);

        public IntPtr Pointer;

        public string String { get { return Marshal.PtrToStringAnsi( DELib_PXDString_Getter_String(Pointer)); } }
    }
}
