using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace DragonEngineLibrary
{

    [StructLayout(LayoutKind.Explicit, Pack = 4)]
    public struct DropWeaponOption
    {
        [FieldOffset(0x0)]
        public AttachmentCombinationID attachmentID;
        [FieldOffset(0x4)]
        public bool isVanish;

        public DropWeaponOption(AttachmentCombinationID id, bool vanish)
        {
            attachmentID = id;
            isVanish = vanish;
        }
    }


    /// <summary>
    /// Fighter objects only exist on combat.
    /// </summary>
    public unsafe struct Fighter
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_PLAY_VOICE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Fighter_PlayVoice(IntPtr fighterPtr, uint id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_GET_INFO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Fighter_GetInfo(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_SCOMMANDAI", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Fighter_GetCommandAI(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_GETTER_CHARACTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Fighter_Getter_Character(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_EQUIP", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_Equip(IntPtr fighterPtr, AssetID assetid, AttachmentCombinationID combinationid, ItemID itemid, RPGSkillID skillid);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_EQUIP2", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_Equip2(IntPtr fighterPtr, ItemID itemid, AttachmentCombinationID combinationid);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_GETWEAPON", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_Fighter_GetWeapon(IntPtr fighterPtr, AttachmentCombinationID attachment);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_DROPWEAPON", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_DropWeapon(IntPtr fighterPtr, ref DropWeaponOption opt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_THROWEQUIPASSET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Fighter_ThrowEquipAsset(IntPtr fighterPtr, bool leftHand, bool rightHand);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_GETREACTIONTYPE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_Fighter_GetReactionType(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_GETSYNCPAIR", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_Fighter_GetSyncPair(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_GETID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_Fighter_GetID(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_GETNAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Fighter_GetName(IntPtr fighterPtr, uint id);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_ISPLAYER", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_IsPlayer(IntPtr fighterPtr);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_ISENEMY", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_IsEnemy(IntPtr fighterPtr);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_IS_BOSS", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_IsBoss(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_IS_DOWN", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_IsDown(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_IS_MUTEKI", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_IsMuteki(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_IS_CAN_STAND_ACTION", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_IsCanStandAction(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_CALC_ROOT_MATRIX", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Fighter_CalcRootMatrix(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_SETUPWEAPON", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Fighter_SetupWeapon(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_REQUEST_DAMAGE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Fighter_RequestDamage(IntPtr fighterPtr, IntPtr dmgInf);

        public Character Character { get; set; }
        public IntPtr _ptr;

        public bool IsValid()
        {
            if (_ptr == IntPtr.Zero)
                return false;

            if (Character == null)
                return false;

            if (!Character.IsValid())
                return false;

            return true;
        }

        public Fighter(IntPtr pointer)
        {
            _ptr = pointer;

            //do PInvoke once. i doubt the character pointer of a fighter will ever change.
            Character = new Character();
            Character._objectAddress = DELib_Fighter_Getter_Character(_ptr);
        }

        public void PlayVoice(uint label)
        {
            DELib_Fighter_PlayVoice(_ptr, label);
        }

        /// <summary>
        /// Get the battle component of this fighter.
        /// </summary>
        public ECBattleStatus GetStatus()
        {
            return Character.GetBattleStatus();
        }

        ///<summary>Get the weapon held at the specified attachment point of this character.</summary>
        public Weapon GetWeapon(AttachmentCombinationID attachment)
        {
            return new Weapon() { Unit = DELib_Fighter_GetWeapon(_ptr, attachment) };
        }

        ///<summary>Setup the weapons of this fighter.</summary>
        public void SetupWeapon()
        {
            DELib_Fighter_SetupWeapon(_ptr);
        }

        public string GetName(uint id)
        {
            return Marshal.PtrToStringAnsi(DELib_Fighter_GetName(_ptr, id));
        }


        ///<summary>Get battle info of this fighter.</summary>
        public BattleFighterInfo GetInfo()
        {
            IntPtr inf = DELib_Fighter_GetInfo(_ptr);

            if (inf == IntPtr.Zero)
                return new BattleFighterInfo();
            else
                return Marshal.PtrToStructure<BattleFighterInfo>(inf);
        }

        /// <summary>
        /// Is the fighter knocked down?
        /// </summary>
        public bool IsDown()
        {
         //   return DELib_Fighter_IsDown(_ptr);
            return Character.HumanModeManager.IsDown();
            //  return Character.IsRagdoll();
            //  return inf.is_stand_up_;
        }

        public bool IsFaceDown()
        {
            uint reactionType = GetReactionType();

            return IsDown() && (reactionType == 7 || reactionType == 49);
        }

        ///<summary>Is the fighter in a sync move?</summary>
        public bool IsSync()
        {
            BattleFighterInfo inf = GetInfo();
            return inf.is_sync_;
        }

        ///<summary>Is the fighter dead?</summary>
        public bool IsDead()
        {
            return Character.IsDead();
        }

        /// <summary>
        /// Is the fighter a player?
        /// </summary>
        public bool IsPlayer()
        {
            return DELib_Fighter_IsPlayer(_ptr);
        }



        /// <summary>
        /// Is the fighter an enemy?
        /// </summary>
        public bool IsEnemy()
        {
            return DELib_Fighter_IsEnemy(_ptr);
        }

        /// <summary>
        /// Is the fighter a boss?
        /// </summary>
        public bool IsBoss()
        {
            return DELib_Fighter_IsBoss(_ptr);
        }

        //might be incorrect?
        /// <summary>
        /// Is the fighter an ally?
        /// </summary>
        public bool IsAlly()
        {
            return !IsPlayer() && !IsEnemy();
        }

        /// <summary>
        /// Are we invincible? (Includes hyperarmor frames)
        /// </summary>
        /// <returns></returns>
        public bool IsInvincible()
        {
            return DELib_Fighter_IsMuteki(_ptr);
        }

        public bool IsCanStandAction()
        {
            return DELib_Fighter_IsCanStandAction(_ptr);
        }

        /// <summary>
        /// Get's the fighter ID of the fighter.
        /// </summary>
        public FighterID GetID()
        {
            FighterID id = new FighterID();
            id.Handle = DELib_Fighter_GetID(_ptr);

            return id;
        }

        /// <summary>
        /// Equip the specified weapon.
        /// </summary>
        public bool Equip(AssetID assetid, AttachmentCombinationID combinationid, ItemID itemid, RPGSkillID skillid)
        {
            return DELib_Fighter_Equip(_ptr, assetid, combinationid, itemid, skillid);
        }

        /// <summary>
        /// Equip the specified weapon.
        /// </summary>
        public bool Equip(ItemID itemid, AttachmentCombinationID combinationid)
        {
            return DELib_Fighter_Equip2(_ptr, itemid, combinationid);
        }

        /// <summary>
        /// Drop weapon if we have any.
        /// </summary>
        public bool DropWeapon(DropWeaponOption opt)
        {
            return DELib_Fighter_DropWeapon(_ptr, ref opt);
        }

        /// <summary>
        /// Throw the weapon in our hands if we have any.
        /// </summary>
        public void ThrowEquipAsset(bool leftHand, bool rightHand)
        {
            DELib_Fighter_ThrowEquipAsset(_ptr, leftHand, rightHand);
        }

        public unsafe void RequestDamage(BattleDamageInfo damage)
        {
            IntPtr ptr = damage.ToIntPtr();
            DELib_Fighter_RequestDamage(_ptr, ptr);

            Marshal.FreeHGlobal(ptr);
        }

        ///<summary>Get the battle AI of this fighter.</summary>
        public BattleCommandAI GetBattleAI()
        {
            IntPtr addr = DELib_Fighter_GetCommandAI(_ptr);
            BattleCommandAI ai = new BattleCommandAI();

            ai.Pointer = addr;

            return ai;
        }

        public uint GetReactionType()
        {
            return DELib_Fighter_GetReactionType(_ptr);
        }

        //
        public EntityHandle<Character> GetSyncPair()
        {
            return DELib_Fighter_GetSyncPair(_ptr);
        }

        ///<summary>Get the root matrix of this fighter.</summary>
        public Matrix4x4 CalcRootMatrix()
        {
            IntPtr matrixPtr = DELib_Fighter_CalcRootMatrix(_ptr);

            if (matrixPtr != IntPtr.Zero)
            {
                Matrix4x4 matrixObj = Marshal.PtrToStructure<Matrix4x4>(matrixPtr);
                Unsafe.CPP.FreeUnmanagedMemory(matrixPtr);

                return matrixObj;
            }
            else
                return new Matrix4x4();
        }


        public static bool operator ==(Fighter v1, Fighter v2)
        {
            if (ReferenceEquals(v1, null) && ReferenceEquals(v2, null))
                return true;

            if (ReferenceEquals(v1, null) || ReferenceEquals(v2, null))
                return false;

            if (v1.Character == null || v2.Character == null)
                return false;

            return v1.Character.UID == v2.Character.UID;
        }

        public static bool operator !=(Fighter v1, Fighter v2)
        {
            if (ReferenceEquals(v1, null) && ReferenceEquals(v2, null))
                return false;

            if (ReferenceEquals(v1, null) || ReferenceEquals(v2, null))
                return true;

            if (v1.Character == null || v2.Character == null)
                return true;

            return v1.Character.UID != v2.Character.UID;
        }

    }

    [StructLayout(LayoutKind.Explicit, Size = 0x4)]
    public unsafe struct FighterID
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTERID_GETTER_HANDLE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_FighterID_Getter_Handle(IntPtr fighterID);

        [FieldOffset(0x0)]
        public uint Handle;
    }
}
