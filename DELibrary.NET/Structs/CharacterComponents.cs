using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class CharacterComponents
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_COMPONENTS_GETTER_EFFECTEVENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_CharacterComponents_Getter_EffectEvent(IntPtr component);

        internal IntPtr _objectAddress;

        internal CharacterComponents(IntPtr addr)
        {
            _objectAddress = addr;
        }

        /// <summary>
        /// Play BEPs on character.
        /// </summary>
        public ECCharacterEffectEvent EffectEvent
        {
            get
            {
                return new EntityComponentHandle<ECCharacterEffectEvent>(DELibrary_CharacterComponents_Getter_EffectEvent(_objectAddress));
            }
        }
    }
}
