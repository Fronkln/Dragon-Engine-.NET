using System;
using System.Runtime.InteropServices;

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
    public class Fighter
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_GET_INFO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Fighter_GetInfo(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_SCOMMANDAI", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Fighter_GetCommandAI(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_GETTER_CHARACTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Fighter_Getter_Character(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_EQUIP", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_Equip(IntPtr fighterPtr, AssetID assetid, AttachmentCombinationID combinationid, ItemID itemid, RPGSkillID skillid);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_DROPWEAPON", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_DropWeapon(IntPtr fighterPtr, ref DropWeaponOption opt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_THROWEQUIPASSET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Fighter_ThrowEquipAsset(IntPtr fighterPtr, bool leftHand, bool rightHand);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_GETID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_Fighter_GetID(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_ISENEMY", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_IsEnemy(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_IS_DOWN", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Fighter_IsDown(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_CALC_ROOT_MATRIX", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Fighter_CalcRootMatrix(IntPtr fighterPtr);

        public Character Character { get; internal set; }
        internal IntPtr _ptr;

        public bool IsValid()
        {
            return Character.IsValid() && Character.GetFighter()._ptr == _ptr;
        }

        public Fighter(IntPtr pointer)
        {
            _ptr = pointer;

            //do PInvoke once. i doubt the character pointer of a fighter will ever change.
            Character = new Character();
            Character._objectAddress = DELib_Fighter_Getter_Character(_ptr);
        }

        /// <summary>
        /// Get the battle component of this fighter.
        /// </summary>
        public ECBattleStatus GetStatus()
        {
            return Character.GetBattleStatus();
        }


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
            BattleFighterInfo inf = GetInfo();
            return inf.is_ragdoll_;
        }

        public bool IsDead()
        {
            return Character.IsDead();
        }

        /// <summary>
        /// Is the fighter a player?
        /// </summary>
        public bool IsPlayer()
        {
            return Character.Attributes.is_player;
        }



        /// <summary>
        /// Is the fighter an enemy?
        /// </summary>
        public bool IsEnemy()
        {
            return DELib_Fighter_IsEnemy(_ptr);
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

        public BattleCommandAI GetBattleAI()
        {
            IntPtr addr = DELib_Fighter_GetCommandAI(_ptr);
            BattleCommandAI ai = new BattleCommandAI();

            ai.Pointer = addr;

            return ai;
        }

        public Matrix4x4 CalcRootMatrix()
        {
            IntPtr matrixPtr = DELib_Fighter_CalcRootMatrix(_ptr);

            if (matrixPtr != IntPtr.Zero)
            {
                Matrix4x4 matrixObj = Marshal.PtrToStructure<Matrix4x4>(matrixPtr);
                DragonEngine.FreeUnmanagedMemory(matrixPtr);

                return matrixObj;
            }
            else
                return new Matrix4x4();
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x4)]
    public struct FighterID
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTERID_GETTER_HANDLE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_FighterID_Getter_Handle(IntPtr fighterID);

        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.U4)]
        public EntityHandle<Character> Handle;
    }
}
