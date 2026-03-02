using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public class ElementBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "CAUTH_ELEMENT_CREATE", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_AuthElement_Create(IntPtr element, IntPtr buf);

        public IntPtr Pointer;

        public void Create(IntPtr buffer)
        {
            DELib_AuthElement_Create(Pointer, buffer);
        }
    }
}
