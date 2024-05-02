using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class CharacterComponents
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_COMPONENTS_GETTER_EFFECTEVENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_CharacterComponents_Getter_EffectEvent(IntPtr component);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_COMPONENTS_GETTER_OCTCOLLECTCHARACTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_CharacterComponents_Getter_OctCollectCharacter(IntPtr component);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_COMPONENTS_GETTER_OCTCOLLECTASSET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_CharacterComponents_Getter_OctCollectAsset(IntPtr component);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_COMPONENTS_GETTER_OCTCOLLECTASSETPICKUP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_CharacterComponents_Getter_OctCollectAssetPickup(IntPtr component);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_COMPONENTS_TEST_FUNC", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint DELibrary_CharacterComponents_Getter_Test(IntPtr component);

        internal IntPtr _objectAddress;

        internal CharacterComponents(IntPtr addr)
        {
            _objectAddress = addr;
        }

        /// <summary>
        /// Play BEPs on character.
        /// </summary>
        public EntityComponentHandle<ECCharacterEffectEvent> EffectEvent
        {
            get
            {
                return new EntityComponentHandle<ECCharacterEffectEvent>(DELibrary_CharacterComponents_Getter_EffectEvent(_objectAddress));
            }
        }

        public EntityComponentHandle<ECOctTreeCollect> OctCollectCharacter
        {
            get
            {
                return new EntityComponentHandle<ECOctTreeCollect>(DELibrary_CharacterComponents_Getter_OctCollectCharacter(_objectAddress));
            }
        }

        public EntityComponentHandle<ECOctTreeCollect> OctCollectAsset
        {
            get
            {
                return new EntityComponentHandle<ECOctTreeCollect>(DELibrary_CharacterComponents_Getter_OctCollectAsset(_objectAddress));
            }
        }

        public EntityComponentHandle<ECOctTreeCollect> OctCollectAssetPickup
        {
            get
            {
                return new EntityComponentHandle<ECOctTreeCollect>(DELibrary_CharacterComponents_Getter_OctCollectAssetPickup(_objectAddress));
            }
        }
    }
}
