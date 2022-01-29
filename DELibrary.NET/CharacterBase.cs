using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class CharacterBase : GameObject
    {

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CHARACTER_BASE_GET_MOTION", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_Character_Base_Get_Motion(IntPtr character_base);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CHARACTER_BASE_GET_RENDER", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_Character_Base_Get_Render(IntPtr character_base);


        /// <summary>
        /// Returns reference to the character's motion component.
        /// </summary>
        public ECMotion GetMotion()
        {
            IntPtr addr = DELib_Character_Base_Get_Motion(_objectAddress);

            ECMotion motion = new ECMotion();
            motion._objectAddress = addr;

            return motion;
        }

        /// <summary>
        /// Returns reference to the character's render mesh component.
        /// </summary>
        public ECRenderCharacter GetRender()
        {
            IntPtr addr = DELib_Character_Base_Get_Render(_objectAddress);

            ECRenderCharacter rend = new ECRenderCharacter();
            rend._objectAddress = addr;

            return rend;
        }
    }
}
