using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECCharacterEffectEvent : EntityComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CHARACTER_EFFECT_EVENT_PLAYEVENT1", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECCharacterEffectEvent_PlayEvent(IntPtr eventPtr, EffectEventCharaID effectID);

        /// <summary>
        /// Play the specified BEP ID. Some ID's may not work.
        /// </summary>
        public void PlayEvent(EffectEventCharaID effectID)
        {
            DELibrary_ECCharacterEffectEvent_PlayEvent(_objectAddress, effectID);
        }
    }
}
