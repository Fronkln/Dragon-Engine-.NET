using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class Character : CharacterBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETTER_STATUS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Getter_Status(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETTER_COMPONENTS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Getter_Components(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GET_BATTLESTATUS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Get_BattleStatus(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_IS_DEAD", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Character_IsDead(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETFIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_GetFighter(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GET_ANG_Y", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DELib_Character_GetAngY(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_SET_ANG_Y", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Character_SetAngY(IntPtr chara, float ang);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_REQUEST_WARP_POSE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Character_RequestWarpPose(IntPtr chara, ref PoseInfo inf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GET_CONSTRUCTOR", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_GetConstructor(IntPtr chara);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETTER_HUMANMODEMANAGER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_GetHumanModeManager(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_CREATE1", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_Character_Create(uint parent, CharacterID charaID);

        /*
        /// <summary>
        /// Common information about the NPC
        /// </summary>
        public CharacterStatus Status
        {
            get
            {
                IntPtr result = DELib_Character_Getter_Status(Pointer);

                if (result == null)
                    return new CharacterStatus();
                else
                    return Marshal.PtrToStructure<CharacterStatus>(result);
            }
        }
        */

        /// <summary>
        /// Common components of a character
        /// </summary>
        public CharacterComponents Components
        {
            get
            {
                return new CharacterComponents(DELib_Character_Getter_Components(_objectAddress));
            }
        }

        public HumanModeManager HumanModeManager
        {
            get
            {
                IntPtr res = DELib_Character_GetHumanModeManager(Pointer);
                HumanModeManager manager = new HumanModeManager();

                manager.Pointer = res;

                return manager;
            }
        }

        public float GetAngleY()
        {
            return DELib_Character_GetAngY(_objectAddress);
        }

        public void SetAngleY(float angle)
        {
            DELib_Character_SetAngY(_objectAddress, angle);
        }

        public void RequestWarpPose(PoseInfo inf)
        {
            DELib_Character_RequestWarpPose(_objectAddress, ref inf);
        }

        public bool IsDead()
        {
            return DELib_Character_IsDead(Pointer);
        }

        /// <summary>
        /// Get the fighter object of this character if it's fighting.
        /// </summary>
        public Fighter GetFighter()
        {
            return new Fighter(DELib_Character_GetFighter(_objectAddress));
        }

        /// <summary>
        /// Get the battle component of this character.
        /// </summary>
        public ECBattleStatus GetBattleStatus()
        {
            IntPtr addr = DELib_Character_Get_BattleStatus(_objectAddress);

            ECBattleStatus status = new ECBattleStatus();
            status._objectAddress = addr;

            return status;
        }

        public ECConstructorCharacter GetConstructor()
        {
            ECConstructorCharacter constructor = new ECConstructorCharacter();
            constructor._objectAddress = DELib_Character_GetConstructor(_objectAddress);

            return constructor;
        }
        
        public static EntityHandle<Character> Create(EntityHandle<EntityBase> parent, CharacterID ID)
        {
            return DELib_Character_Create(parent.UID, ID);
        }
    }
}
