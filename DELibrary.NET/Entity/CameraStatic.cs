using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public class CameraStatic : CameraBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCAMERA_STATIC_CREATE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_CameraStatic_Create(uint parent);

        public static CameraStatic Create(EntityHandle<EntityBase> parent)
        {
            return new EntityHandle<CameraStatic>(DELibrary_CameraStatic_Create(parent.UID));
        }
    }
}
