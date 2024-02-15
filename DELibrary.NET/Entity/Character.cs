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

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_REQUEST_START_FIGHTER", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Character_RequestStartFighter(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_IS_DEAD", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Character_IsDead(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_TO_DEAD", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Character_ToDead(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETFIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_GetFighter(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GET_ANG_Y", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DELib_Character_GetAngY(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_SET_ANG_Y", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Character_SetAngY(IntPtr chara, float ang);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_WARP_POS_AND_ANGLE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Character_WarpPosAndAng(IntPtr chara, Vector4 pos, float ang);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_REQUEST_WARP_POSE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Character_RequestWarpPose(IntPtr chara, IntPtr inf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_REQUEST_MOVE_POSE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Character_RequestMovePose(IntPtr chara, IntPtr inf);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_WARP_POS_AND_ORIENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_Character_WarpPosAndOrient(IntPtr chara, ref Vector4 position, ref Quaternion rotation);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GET_CONSTRUCTOR", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_GetConstructor(IntPtr chara);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETTER_HUMANMODEMANAGER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_GetHumanModeManager(IntPtr chara);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETTER_CHARACTERSTATUS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_GetterCharacterStatus(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_CREATE1", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_Character_Create(uint parent, CharacterID charaID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETTER_ECBATTLETARGETDECIDE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Getter_ECBattleTargetDecide(IntPtr chara);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETTER_PAD", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Getter_Pad(IntPtr chara);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETTER_SPEECH", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Getter_Speech(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "TESTY_TEST_FUNC", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr DELib_TESTY_TEST_FUNC(IntPtr chara);

#if TURN_BASED_GAME
        ///<summary>Target select module of the character.</summary>
        public ECBattleTargetDecide TargetDecide
        {
            get
            {
                ECBattleTargetDecide decide = new ECBattleTargetDecide();
                decide.Pointer = DELib_Character_Getter_ECBattleTargetDecide(Pointer);

                return decide;
            }
        }
#endif

        ///<summary>Common information about the character.</summary>
        public CharacterStatus Status
        {
            get
            {
                IntPtr status = DELib_Character_GetterCharacterStatus(Pointer);

                CharacterStatus statuschara = new CharacterStatus();
                statuschara.pointer = status;

                return statuschara;
            }
        }

        ///<summary>Common components of a character.</summary>
        public CharacterComponents Components
        {
            get
            {
                return new CharacterComponents(DELib_Character_Getter_Components(_objectAddress));
            }
        }

        ///<summary>The HumanModeManager component of this character.</summary>
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

        public PadInputInfo Pad
        {
            get
            {
                return new PadInputInfo() { Pointer = DELib_Character_Getter_Pad(Pointer) };
            }
        }

        public IntPtr Speech
        {
            get
            {
                return DELib_Character_Getter_Speech(Pointer);
            }
        }


        ///<summary>Get the Y angle of this character.</summary>
        public float GetAngleY()
        {
            return DELib_Character_GetAngY(_objectAddress);
        }

#if YK2
        ///<summary>Request to turn this character into a fighter.</summary>
        public bool RequestStartFighter()
        {
            return DELib_Character_RequestStartFighter(Pointer);
        }
#endif

        ///<summary>Set Y angle of this character.</summary>
        public void SetAngleY(float angle)
        {
            DELib_Character_SetAngY(_objectAddress, angle);
        }

#if YK2
        ///<summary>Warp to the coordinates specified.</summary>
        public void WarpPosAndAngle(Vector4 position, float angle)
        {
            DELib_Character_WarpPosAndAng(Pointer, position, angle);
        }
#endif

        ///<summary>Request to be warped to the coordinates specified.</summary>
        public void RequestWarpPose(PoseInfo inf)
        {
            IntPtr infPtr = inf.ToIntPtr();
            DELib_Character_RequestWarpPose(_objectAddress, infPtr);

            Marshal.FreeHGlobal(infPtr);
        }

        ///<summary>Request to move to the coordinates obeying physics and collision.</summary>
        public void RequestMovePose(PoseInfo inf)
        {
            IntPtr infPtr = inf.ToIntPtr();
            DELib_Character_RequestMovePose(_objectAddress, infPtr);

            Marshal.FreeHGlobal(infPtr);
        }

        public void WarpPosAndOrient(Vector4 position, Quaternion rotation)
        {
            DELibrary_Character_WarpPosAndOrient(Pointer, ref position, ref rotation);
        }

        ///<summary>Kill this character.</summary>
        public void ToDead()
        {
            DELib_Character_ToDead(Pointer);
        }

        ///<summary>Is the character dead?</summary>
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

        ///<summary>Get the constructor of this character.</summary>
        public ECConstructorCharacter GetConstructor()
        {
            ECConstructorCharacter constructor = new ECConstructorCharacter();
            constructor._objectAddress = DELib_Character_GetConstructor(_objectAddress);

            return constructor;
        }


        ///<summary>Set the commandset of this character.</summary>
        public void SetCommandSet(BattleCommandSetID commandSet)
        {
            CommandSetModel commandSetMdl = HumanModeManager.CommandsetModel;
            GetBattleStatus().ClearCommand();

            commandSetMdl.SetCommandSet(0, commandSet);
        }

        ///<summary>Create a character.</summary>
        public static EntityHandle<Character> Create(EntityHandle<EntityBase> parent, CharacterID ID)
        {
            return DELib_Character_Create(parent.UID, ID);
        }
    }
}
