using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary.Entity
{
    public class UnsafeObject
    {
        public IntPtr Pointer;

        public UnsafeObject()
        {

        }

        public UnsafeObject(IntPtr ptr)
        {
            Pointer = ptr;
        }
    }
}
