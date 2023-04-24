using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public unsafe class UIPlayerBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_PLAYER_CBASE_GETTER_SCALE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern Vector4 DELib_UIHandleBase_Getter_Scale(IntPtr player);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_PLAYER_CBASE_SETTER_SCALE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_Setter_Scale(IntPtr player, Vector4 vec);

        public IntPtr Pointer;

        public Vector4 Scale
        {
            get
            {
                return DELib_UIHandleBase_Getter_Scale(Pointer);
            }
            set
            {
                DELib_UIHandleBase_Setter_Scale(Pointer, value);
            }
        }


        public UIPlayerBase(IntPtr pointer)
        {
            Pointer = pointer;
        }
    }
}
