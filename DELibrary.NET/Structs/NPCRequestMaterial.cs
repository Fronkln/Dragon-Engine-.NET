using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{

    [StructLayout(LayoutKind.Sequential, Size = 0x20)]
    public struct MaterialResult
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        private byte[] result_handle_list;
        // pxd::t_vector<entity_handle<ccharacter>, 0, pxd::t_aligned_allocator<entity_handle<ccharacter>, 0>> result_handle_list_;

        uint request_id_;
        byte result;
        bool is_wish_get_after_;
        bool is_wish_fast_generate_;

        private IntPtr creating_team_;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NPCRequestMaterial
    {
        public NPCMaterial Material;
        private MaterialResult Result;
    }
}
