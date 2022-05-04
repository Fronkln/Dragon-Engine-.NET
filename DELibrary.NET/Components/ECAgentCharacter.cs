using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECAgentCharacter : ECAgent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_AGENT_CHARACTER_GETTER_AI", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_ECAgentCharacter_Getter_AI(IntPtr agent);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_AGENT_CHARACTER_GETTER_CEC_SOLDIER_INFO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_ECAgentCharacter_Getter_SoldierInfo(IntPtr agent);

        /// <summary>
        /// AI component of the Agent.
        /// </summary>
        public EntityComponentHandle<ECAI> AI
        {
            get { return DELib_ECAgentCharacter_Getter_AI(_objectAddress); }
        }

        ///<summary>Soldier info of this agent.</summary>
        public EntityComponentHandle<ECSoldierInfo> SoldierInfo
        {
            get { return DELib_ECAgentCharacter_Getter_SoldierInfo(_objectAddress); }
        }
    }
}
