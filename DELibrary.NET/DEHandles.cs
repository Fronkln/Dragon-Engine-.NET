using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{

    //DLLImport cant be in a generic class, this is necessary evil
    static class HandleBindings
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_HANDLE_DEREFERENCE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_EntityHandle_Dereference(uint handle);
    }

    //Used on functions or structures that takes handles as parameters
    //EntityHandle is a generic class and cannot be marshalled.
    public class DEHandleMarshal
    {
        public uint uid;

        public T ToHandle<T>() where T : CTask, new()
        {
            EntityHandle<T> handle = new EntityHandle<T>();
            handle.UID = uid;

            return handle;
        }


        public static implicit operator DEHandleMarshal(EntityHandle<CTask> handle)
        {
            DEHandleMarshal marshal = new DEHandleMarshal();
            marshal.uid = handle.UID;

            return marshal;
        }
    }

    public class EntityHandle<T> where T : CTask, new()
    {
        internal uint UID;

        /// <summary>
        /// Get the reference to the entity from this handle.
        /// </summary>
        public T Get()
        {
            T obj = new T();
            obj._objectAddress = HandleBindings.DELib_EntityHandle_Dereference(UID);

            return obj;
        }

        public bool IsValid()
        {
            return UID != 0 && Get()._objectAddress != IntPtr.Zero;
        }

        public static implicit operator T(EntityHandle<T> handle)
        {
            return handle.Get();
        }

        public static implicit operator EntityHandle<T>(uint uid)
        {
            EntityHandle<T> handle = new EntityHandle<T>();
            handle.UID = uid;

            return handle;
        }

        public static implicit operator EntityHandle<T>(T ent)
        {
            EntityHandle<T> handle = new EntityHandle<T>();
            handle.UID = ent.UID;

            return handle;
        }
    }
}
