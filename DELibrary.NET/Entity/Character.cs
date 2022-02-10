using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class Character : CharacterBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETTER_COMPONENTS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Getter_Components(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GET_BATTLESTATUS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Get_BattleStatus(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GETFIGHTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_GetFighter(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GET_ANG_Y", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DELib_Character_GetAngY(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_SET_ANG_Y", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Character_SetAngY(IntPtr chara, float ang);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_REQUEST_WARP_POSE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Character_RequestWarpPose(IntPtr chara, ref PoseInfo inf);

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
    }
}
