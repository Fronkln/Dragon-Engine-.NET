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
#if YLAD
            bool characterFighting = new EntityHandle<Character>(Owner.UID).Get().GetFighter().IsValid();
#else
            bool characterFighting = false;
#endif

            DELib_ECRenderCharacter_Reload(_objectAddress, chara_id, (characterFighting ? (byte)0x80 : (byte)0x1), false, is_change_chara_id);
        }
    }
}
