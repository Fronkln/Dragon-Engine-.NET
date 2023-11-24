using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{

#if YLAD
    [StructLayout(LayoutKind.Explicit, Pack = 16, Size = 0x1120)]
#endif
#if IW_AND_UP
    [StructLayout(LayoutKind.Explicit, Size = 0x1120)]
#endif
    public struct HActRequestOptions
    {

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTREQUESTOPTIONS_INIT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HActRequestOptions_INIT(ref HActRequestOptions opt);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTREQUESTOPTIONS_REGISTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HActRequestOptions_Register(ref HActRequestOptions opt, HActReplaceID id, uint charid);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTREQUESTOPTIONS_REGISTERFIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HActRequestOptions_RegisterFighter(ref HActRequestOptions opt, HActReplaceID id, uint fid);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTREQUESTOPTIONS_REGISTERFIGHTERANDWEAPON", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HActRequestOptions_RegisterFighterAndWeapon(ref HActRequestOptions opt, HActReplaceID id, uint fid);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTREQUESTOPTIONS_REGISTERWEAPON", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HActRequestOptions_RegisterWeapon(ref HActRequestOptions opt, AuthAssetReplaceID id, AssetID asset);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTREQUESTOPTIONS_REGISTERWEAPON2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HActRequestOptions_RegisterWeapon2(ref HActRequestOptions opt, AuthAssetReplaceID id, uint wep);


#if YLAD
        [FieldOffset(0x0)] public DynamicsMatrix base_mtx;
        [FieldOffset(0x70)] public DynamicsMatrix base_mtx_sub;
        [FieldOffset(0xE0)] public TalkParamID id;
        [FieldOffset(0xE4)] public uint id_param;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x1028)]
        [FieldOffset(0xE8)] public byte[] unmarshalled_puid_lists;
        //  t_puid_array<entity_uid, enum hact::replace::e_id, 171> replace_character_list;
        //  t_puid_array<entity_uid, enum auth::asset_replace::e_id, 70> replace_asset_list;
        //   t_puid_array<enum asset::e_id, enum auth::asset_replace::e_id, 70> replace_asset_id_list;
        [FieldOffset(0x1110)] public byte range_type;
        [FieldOffset(0x1111)] [MarshalAs(UnmanagedType.U1)] public bool is_player_action;
        [FieldOffset(0x1112)] [MarshalAs(UnmanagedType.U1)] public bool is_large_ok;
        [FieldOffset(0x1113)] [MarshalAs(UnmanagedType.U1)] public bool is_fixed;
        [FieldOffset(0x1114)] [MarshalAs(UnmanagedType.U1)] public bool is_warp_return;
        [FieldOffset(0x1115)] [MarshalAs(UnmanagedType.U1)] public bool is_force_play;
        [FieldOffset(0x1116)] [MarshalAs(UnmanagedType.U1)] public bool can_skip;
        [FieldOffset(0x1117)] [MarshalAs(UnmanagedType.U1)] public bool is_last_frame_draw_stop;
#endif

#if IW_AND_UP
        [FieldOffset(0x0)] public DynamicsMatrix base_mtx;
        [FieldOffset(0x70)] public DynamicsMatrix base_mtx_sub;
        [FieldOffset(0xE0)] public TalkParamID id;
        [FieldOffset(0xE4)] public uint id_param;
#endif


        public void Init()
        {
            DELib_HActRequestOptions_INIT(ref this);
        }

        public void Register(HActReplaceID id, EntityHandle<CharacterBase> chara)
        {
            DELib_HActRequestOptions_Register(ref this, id, chara.UID);
        }

        public void RegisterFighter(HActReplaceID id, FighterID fighterID)
        {
            DELib_HActRequestOptions_RegisterFighter(ref this, id, fighterID.Handle);
        }

        public void RegisterFighterAndWeapon(HActReplaceID id, FighterID fighterID)
        {
            DELib_HActRequestOptions_RegisterFighterAndWeapon(ref this, id, fighterID.Handle);
        }

        public void RegisterFighterAndWeapon(HActReplaceID id, Fighter fighter, AuthAssetReplaceID wepReplaceID)
        {
            Register(id, fighter.Character.UID);
            RegisterWeapon(wepReplaceID, fighter.GetWeapon(AttachmentCombinationID.right_weapon2).Unit.Get().AssetID);
        }

        public void RegisterWeapon(AuthAssetReplaceID id, AssetID asset)
        {
            DELib_HActRequestOptions_RegisterWeapon(ref this, id, asset);
        }

        public void RegisterWeapon(AuthAssetReplaceID id, Weapon wep)
        {
            DELib_HActRequestOptions_RegisterWeapon2(ref this, id, wep.Unit.UID);
        }
    }
}
