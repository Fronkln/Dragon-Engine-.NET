using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class CTask
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CTASK_GETTER_UID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_CTask_UID_Getter(IntPtr ctask);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HANDLES_COMMON_VALIDITY_CHECK", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_CTask_Common_Validity_Check(uint UID);

        public enum Kind : ushort
        {
            Unknown,        // constant 0x0
            Generic,        // constant 0x1
            Service,        // constant 0x2
            Entity,         // constant 0x3
            Component,       // constant 0x4
            FilePort,      // constant 0x5
            End,        // constant 0x6
            Begin,      // constant 0x1
            Num,        // constant 0x5
        };


        /// <summary>
        /// Unimplemented for now, it usually doesnt matter
        /// </summary>
        public Kind TaskKind
        {
            get
            {
                return Kind.Unknown;
            }
        }

        /// <summary>
        /// UID's are unique for each entity which can be used for handles to get a pointer to the actual object
        /// </summary>
        public uint UID
        {
            get
            {
                return DELib_CTask_UID_Getter(_objectAddress);
            }
        }   

        /// <summary>
        /// Advanced 
        /// </summary>
        public IntPtr Pointer
        {
            get
            {
                return _objectAddress;
            }
            internal set { _objectAddress = value; }
        }

        /// <summary>
        /// Is this object valid? (UID is not zero + UID dereferencing points to a valid memory address)
        /// </summary>
        public virtual bool IsValid()
        {
            return DELib_CTask_Common_Validity_Check(UID);
           // return UID != 0 && ((EntityHandle<CTask>)UID).IsValid();
        }

        /// <summary>
        /// Memory address of the object. Gets passed to CPP library for object operations
        /// </summary>
        internal IntPtr _objectAddress;
    }
}
