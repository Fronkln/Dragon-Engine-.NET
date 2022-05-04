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
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CHARACTER_BASE_GETTER_ATTRIBUTES", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Base_Getter_Attributes(IntPtr character_base);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CHARACTER_BASE_GET_MOTION", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Base_GetMotion(IntPtr character_base);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CHARACTER_BASE_GET_RENDER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Base_GetRender(IntPtr character_base);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CHARACTER_BASE_GET_POSTUREE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Character_Base_GetPosture(IntPtr character_base);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CHARACTER_BASE_IS_RAGDOLL", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_Character_Base_IsRagdoll(IntPtr character_base);

        ///<summary>Information that was used to create the character.</summary>
        public CharacterAttributes Attributes
        {
            get
            {
                IntPtr ptr = DELib_Character_Base_Getter_Attributes(_objectAddress);

                if (ptr == IntPtr.Zero)
                    return new CharacterAttributes(); //return a default one to avoid crash

                CharacterAttributes attributes = Marshal.PtrToStructure<CharacterAttributes>(ptr);
                return attributes;
            }
        }

        ///<summary>The motion component of this character.</summary>
        public ECMotion GetMotion()
        {
            IntPtr addr = DELib_Character_Base_GetMotion(_objectAddress);

            ECMotion motion = new ECMotion();
            motion._objectAddress = addr;

            return motion;
        }
        ///<summary>The mesh compoment of this character.</summary>
        public ECRenderCharacter GetRender()
        {
            IntPtr addr = DELib_Character_Base_GetRender(_objectAddress);

            ECRenderCharacter rend = new ECRenderCharacter();
            rend._objectAddress = addr;

            return rend;
        }

        ///<summary>The posture component of this character.</summary>
        public ECPosture GetPosture()
        {
            IntPtr addr = DELib_Character_Base_GetPosture(_objectAddress);
            return new ECPosture() { Pointer = addr };
        }

        ///<summary>Is the character ragdolled?</summary>
        public bool IsRagdoll()
        {
            return DELib_Character_Base_IsRagdoll(Pointer);
        }
    }
}

