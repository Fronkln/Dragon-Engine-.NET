using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    //These only exist for interop purposes
    //And as such, this doesnt have same parameters as CPP api
    //Still. These are not the easiest thing to marshal.

    [StructLayout(LayoutKind.Sequential)]
    public class PXDBaseVector<T>
    {
        IntPtr mp_element;
        uint m_vector_size;
        uint m_element_size;
    }

    public class PXDVector<T> : PXDBaseVector<T>
    {

    }

    public class PXDFixedVector<T> : PXDBaseVector<T>
    {

    }
}
