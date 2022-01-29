using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class EntityBase : CTask
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GET_POS_CENTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern Vector4 DELibrary_EntityBase_GetPosCenter(IntPtr entity);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_SET_POS_CENTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_EntityBase_SetPosCenter(IntPtr entity, Vector4 pos);

        /// <summary>
        /// Get the entity's center position.
        /// </summary>
        public Vector4 GetPosCenter()
        {
            return DELibrary_EntityBase_GetPosCenter(_objectAddress);
        }

        /// <summary>
        /// Set entity position.
        /// </summary>
        public void SetPosCenter(Vector4 position)
        {
            DELibrary_EntityBase_SetPosCenter(_objectAddress, position);
        }
    }
}
