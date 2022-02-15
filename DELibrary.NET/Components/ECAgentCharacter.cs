using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECAgentCharacter : ECAgent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_AGENT_CHARACTER_GETTER_AI", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_ECAgentCharacter_Getter_AI(IntPtr agent);

        /// <summary>
        /// AI component.
        /// </summary>
        public EntityComponentHandle<ECAI> AI
        {
            get { return DELib_ECAgentCharacter_Getter_AI(_objectAddress); }
        }       
    }
}
