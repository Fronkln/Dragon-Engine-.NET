using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    //https://stackoverflow.com/a/52103996/14569631
    public static class ObjectHandleExtensions
    {
        public static IntPtr ToIntPtr(this object target)
        {
            IntPtr allocedObj = Marshal.AllocHGlobal(Marshal.SizeOf(target));
            Marshal.StructureToPtr(target, allocedObj, false);

            return allocedObj;
        }

        public static GCHandle ToGcHandle(this object target)
        {
            return GCHandle.Alloc(target);
        }

        public static IntPtr ToIntPtr(this GCHandle target)
        {
            return GCHandle.ToIntPtr(target);
        }

        public static T[] ToTypeArray<T>(this IntPtr unmanagedArray, int length)
        {
            T[] array = new T[length];
            var size = Marshal.SizeOf(typeof(T));

            for (int i = 0; i < length; i++)
            {
                IntPtr ins = new IntPtr(unmanagedArray.ToInt64() + i * size);
                array[i] = Marshal.PtrToStructure<T>(ins);
            }

            return array;
        }
    }
}
