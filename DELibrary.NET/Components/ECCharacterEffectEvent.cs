using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECCharacterEffectEvent : EntityComponent
    {

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CHARACTER_EFFECT_EVENT_STOP_EVENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECCharacterEffectEvent_StopEvent(IntPtr eventPtr, EffectEventCharaID effectID, bool fadeOut);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CHARACTER_EFFECT_EVENT_STOP_EVENT_ALL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECCharacterEffectEvent_StopEventAll(IntPtr eventPtr);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CHARACTER_EFFECT_EVENT_PLAY_EVENT1", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECCharacterEffectEvent_PlayEvent(IntPtr eventPtr, EffectEventCharaID effectID);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CHARACTER_EFFECT_EVENT_PLAY_EVENT2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECCharacterEffectEvent_PlayEvent(IntPtr eventPtr, EffectEventCharaID start, EffectEventCharaID next);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CHARACTER_EFFECT_EVENT_PLAY_EVENT_OVERRIDE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECCharacterEffectEvent_PlayEventOverride(IntPtr eventPtr, EffectEventCharaID effectID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CHARACTER_EFFECT_EVENT_ATTACH", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_ECCharacterEffectEvent_Attach(IntPtr chara, ref bool b_new);

        /// <summary>
        /// Attach the component to character.
        /// </summary>
        public static EntityComponentHandle<ECCharacterEffectEvent> Attach(Character chara, bool b_new = true)
        {
            return DELibrary_ECCharacterEffectEvent_Attach(chara._objectAddress, ref b_new);
        }

        /// <summary>
        /// Stop all playing effects.
        /// </summary>
        public void StopEventAll()
        {
            DELibrary_ECCharacterEffectEvent_StopEventAll(_objectAddress);
        }

        /// <summary>
        /// Play the specified BEP ID. Some ID's may not work.
        /// </summary>
        public void PlayEvent(EffectEventCharaID effectID)
        {
            DELibrary_ECCharacterEffectEvent_PlayEvent(_objectAddress, effectID);
        }

        /// <summary>
        /// Play the specified BEP ID first, then the next one.
        /// </summary>
        public void PlayEvent(EffectEventCharaID start, EffectEventCharaID next)
        {
            DELibrary_ECCharacterEffectEvent_PlayEvent(_objectAddress, start, next);
        }

        /// <summary>
        /// Science cant explain what fucking difference this function has to other play function
        /// </summary>
        public void PlayEventOverride(EffectEventCharaID effectID)
        {
            DELibrary_ECCharacterEffectEvent_PlayEventOverride(_objectAddress, effectID);
        }

        ///<summary>Stop specified effect.</summary>
        public void StopEvent(EffectEventCharaID effectID, bool fadeOut)
        {
            DELibrary_ECCharacterEffectEvent_StopEvent(_objectAddress, effectID, fadeOut);
        }
    }
}
