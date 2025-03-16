using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{

    public class BattleDamageInfoSafe
    {
        public IntPtr _ptr;


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_GETTER_HIT_POS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern Vector4 DELib_BattleDamageInfo_Getter_HitPos(IntPtr dmgInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_GETTER_ATTACKER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_BattleDamageInfo_Getter_Attacker(IntPtr dmgInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_GETTER_WEAPON", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_BattleDamageInfo_Getter_Weapon(IntPtr dmgInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_GETTER_BASE_DAMAGE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DELib_BattleDamageInfo_Getter_BaseDamage(IntPtr dmgInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_SETTER_BASE_DAMAGE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleDamageInfo_Setter_BaseDamage(IntPtr dmgInf, int damage);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_GETTER_DAMAGE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DELib_BattleDamageInfo_Getter_Damage(IntPtr dmgInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_SETTER_DAMAGE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleDamageInfo_Setter_Damage(IntPtr dmgInf, int damage);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_GETTER_MISS", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleDamageInfo_Getter_Miss(IntPtr dmgInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_SETTER_MISS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BattleDamageInfo_Setter_Miss(IntPtr dmgInf, bool miss);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_GETTER_SYNC_MTX", CallingConvention = CallingConvention.Cdecl)]
        internal static extern Matrix4x4 DELib_BattleDamageInfo_Getter_SyncMtx(IntPtr dmgInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_GETTER_IS_SYNC_START_DMG", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleDamageInfo_Getter_IsSyncStartDmg(IntPtr dmgInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_GETTER_IS_DIRECT", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleDamageInfo_Getter_IsDirect(IntPtr dmgInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_GETTER_IS_GUARD", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleDamageInfo_Getter_IsGuard(IntPtr dmgInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLEDAMAGEINFO_GETTER_IS_JUST_GUARD", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleDamageInfo_Getter_IsJustGuard(IntPtr dmgInf);

        public EntityHandle<Character> Attacker { get { return new EntityHandle<Character>(DELib_BattleDamageInfo_Getter_Attacker(_ptr)); } }

        public EntityHandle<AssetUnit> Weapon { get { return DELib_BattleDamageInfo_Getter_Weapon(_ptr); } }

        public int BaseDamage { get { return DELib_BattleDamageInfo_Getter_BaseDamage(_ptr); } set { DELib_BattleDamageInfo_Setter_BaseDamage(_ptr, value); } }
        public int Damage { get { return DELib_BattleDamageInfo_Getter_Damage(_ptr); } set { DELib_BattleDamageInfo_Setter_Damage(_ptr, value); } }
        public bool Miss { get { return DELib_BattleDamageInfo_Getter_Miss(_ptr); } set { DELib_BattleDamageInfo_Setter_Miss(_ptr, value); } }
        public Vector4 HitPosition { get { return DELib_BattleDamageInfo_Getter_HitPos(_ptr); } }

        public Matrix4x4 SyncMatrix { get { return DELib_BattleDamageInfo_Getter_SyncMtx(_ptr); } }

        public bool IsSyncStartDmg { get { return DELib_BattleDamageInfo_Getter_IsSyncStartDmg(_ptr); } }
        public bool IsDirect { get { return DELib_BattleDamageInfo_Getter_IsDirect(_ptr); } }

        public bool IsGuard { get { return DELib_BattleDamageInfo_Getter_IsGuard(_ptr); } }
        public bool IsJustGuard { get { return DELib_BattleDamageInfo_Getter_IsJustGuard(_ptr); } }

        public BattleDamageInfoSafe(IntPtr addr)
        {
            _ptr = addr;
        }
    }
}
