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

        public Character Character { get; internal set; }
        internal IntPtr _ptr;

        internal Fighter(IntPtr pointer)
        {
            _ptr = pointer;

            //do PInvoke once. i doubt the character pointer of a fighter will ever change.
            Character = new Character();
            Character._objectAddress = DELib_Fighter_Getter_Character(_ptr);
        }

        public bool IsPlayer()
        {
            return Character.Attributes.is_player;
        }


        
        //TODO, do the calculations from C# instead of calling a native func
        //When the time comes that is
        public bool IsEnemy()
        {
            return DELib_Fighter_IsEnemy(_ptr);
        }

        //might be incorrect?
        public bool IsAlly()
        {
            return !IsPlayer() && !IsEnemy();
        }
        
        public FighterID GetID()
        {

            FighterID id = new FighterID();

            DragonEngine.Log("CSharp: " + DELib_Fighter_GetID(_ptr));
            
            id.Handle = DELib_Fighter_GetID(_ptr);

            return id;
        }

        public bool Equip(AssetID assetid, AttachmentCombinationID combinationid, ItemID itemid, RPGSkillID skillid)
        {
            return DELib_Fighter_Equip(_ptr, assetid, combinationid, itemid, skillid);
        }

        public bool DropWeapon(DropWeaponOption opt)
        {
            return DELib_Fighter_DropWeapon(_ptr, ref opt);
        }

        public void ThrowEquipAsset(bool leftHand, bool rightHand)
        {
            DELib_Fighter_ThrowEquipAsset(_ptr, leftHand, rightHand);
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
