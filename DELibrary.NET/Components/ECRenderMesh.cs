using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECRenderMesh : EntityComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_RENDER_MESH_TEST", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ECRenderMesh_Test(IntPtr renderMesh);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_RENDER_MESH_GETTER_FLAGS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_ECRenderMesh_Getter_Flags(IntPtr renderMesh);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_RENDER_MESH_SETTER_FLAGS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_ECRenderMesh_Setter_Flags(IntPtr renderMesh, uint flag);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_RENDER_MESH_GETTER_BOUNDING_BOX_LOCAL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_ECRenderMesh_Getter_LocalBoundingBox(IntPtr renderMesh);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_RENDER_MESH_SET_BOUNDING_BOX_LOCAL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ECRenderMesh_SetBoundingBoxLocal(IntPtr renderMesh, IntPtr orbox);


        public uint Flags
        {
            get
            {
                return DELib_ECRenderMesh_Getter_Flags(_objectAddress);
            }
            set
            {
                DELib_ECRenderMesh_Setter_Flags(_objectAddress, value);
            }
        }

        public OrBox LocalBoundingBox
        {
            get
            {
                IntPtr ptr = DELib_ECRenderMesh_Getter_LocalBoundingBox(_objectAddress);

                if (ptr == IntPtr.Zero)
                    return new OrBox(); //return a default one to avoid crash

                OrBox  box = Marshal.PtrToStructure<OrBox>(ptr);
                return  box;
            }
            set
            {
                SetBoundingBoxLocal(value);
            }
        }

        ///<summary>Set the bounding box of this mesh.</summary>
        public void SetBoundingBoxLocal(OrBox box)
        {
            IntPtr boxPtr = box.ToIntPtr();
            DELib_ECRenderMesh_SetBoundingBoxLocal(_objectAddress, boxPtr);

            Marshal.FreeHGlobal(boxPtr);
     
        }
    }
}
