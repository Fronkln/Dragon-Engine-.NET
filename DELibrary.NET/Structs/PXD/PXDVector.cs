using DragonEngineLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary.Structs.PXD
{
    public class PXDVectorPtr<T> where T : UnsafeObject, new()
    {
        public IntPtr Pointer;

        public int Count()
        {
            return 0;
        }
    }
}
