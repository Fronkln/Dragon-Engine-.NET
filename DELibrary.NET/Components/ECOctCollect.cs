using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public unsafe class ECOctTreeCollect : EntityComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_OCT_TREE_GETTER_COLLECTED", CallingConvention = CallingConvention.Cdecl)]
        internal static extern PXDStaticVector* DELibrary_ECOctTreeCollect_Getter_Collected(IntPtr component);

        public PXDStaticVector* Collected { get { return DELibrary_ECOctTreeCollect_Getter_Collected(Pointer); } }
    }
}
