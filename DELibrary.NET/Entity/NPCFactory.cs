using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    //Static but silently accessed through CharacterManager
    public static class NPCFactory
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CNPC_FACTORY_REQUEST_CREATE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_NPCFactory_RequestCreate(IntPtr requestMaterial);

        ///<summary>Request a NPC to be created.</summary>
        public static EntityHandle<Character> RequestCreate(NPCRequestMaterial mat)
        {
            IntPtr matPtr = mat.ToIntPtr();
            EntityHandle<Character> result = DELibrary_NPCFactory_RequestCreate(matPtr);
            Marshal.FreeHGlobal(matPtr);

            return result;
        }
    }
}
