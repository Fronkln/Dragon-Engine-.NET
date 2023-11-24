using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECRenderCharacter : ECRenderMesh
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_RENDER_CHARACTER_RELOAD", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_ECRenderCharacter_Reload(IntPtr renderCharacter, CharacterID chara_id, byte bank_mask, bool is_preload, bool is_change_chara_id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_RENDER_CHARACTER_BATTLE_TRANSFORM_ON", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_ECRenderCharacter_BattleTransformOn(IntPtr renderCharacter);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_RENDER_CHARACTER_BATTLE_TRANSFORM_OFF", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_ECRenderCharacter_BattleTransformOff(IntPtr renderCharacter);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_RENDER_CHARACTER_GETTER_CHARACTER_ID", CallingConvention = CallingConvention.Cdecl)]
        private static extern CharacterID DELib_ECRenderCharacter_Getter_CharacterID(IntPtr renderCharacter);

        public CharacterID CharacterID
        {
            get { return DELib_ECRenderCharacter_Getter_CharacterID(Pointer); }
        }

        /// <summary>
        /// Reload the mesh with specified character id.
        /// </summary>
        public void Reload(CharacterID chara_id, byte bank_mask, bool is_change_chara_id = true)
        {
            DELib_ECRenderCharacter_Reload(_objectAddress, chara_id, bank_mask, false, is_change_chara_id);
        }

        /// <summary>
        /// Reload the mesh with specified character id.
        /// </summary>
        public void Reload(CharacterID chara_id, bool is_change_chara_id = true)
        {
#if YLAD_AND_GREATER
            bool characterFighting = new EntityHandle<Character>(Owner.UID).Get().GetFighter().IsValid();
#else
            bool characterFighting = false;
#endif

            DELib_ECRenderCharacter_Reload(_objectAddress, chara_id, (characterFighting ? (byte)0x80 : (byte)0x1), false, is_change_chara_id);
        }

#if TURN_BASED_GAME
        ///<summary>Battle transform the character.</summary>
        public bool BattleTransformationOn()
        {
            return DELib_ECRenderCharacter_BattleTransformOn(_objectAddress);
        }

        ///<summary>Un-battle transform the character.</summary>
        public bool BattleTransformationOff()
        {
            return DELib_ECRenderCharacter_BattleTransformOff(_objectAddress);
        }
#endif
    }
}
