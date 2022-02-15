using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECConstructorCharacter : ECConstructor
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CONSTRUCTOR_CHARACTER_GET_AGENT_COMPONENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_ECConstructorCharacter_GetAgentComponent(IntPtr constructor);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CONSTRUCTOR_CHARACTER_GETTER_OWNER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_ECConstructorCharacter_Getter_Owner(IntPtr constructor);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CONSTRUCTOR_CHARACTER_START_AGENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ECConstructorCharacter_StartAgent(IntPtr constructor);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_CONSTRUCTOR_CHARACTER_STOP_AGENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ECConstructorCharacter_StopAgent(IntPtr constructor);

        public Character Owner
        {
            get
            {
                Character owner = new Character();
                owner._objectAddress = DELib_ECConstructorCharacter_Getter_Owner(_objectAddress);

                return owner;
            }
        }

        public ECAgentCharacter GetAgentComponent()
        {       
            ECAgentCharacter agent = new ECAgentCharacter();
            agent._objectAddress = DELib_ECConstructorCharacter_GetAgentComponent(_objectAddress);

            return agent;
        }

        public void StartAgent()
        {
            DELib_ECConstructorCharacter_StartAgent(_objectAddress);
        }

        public void StopAgent()
        {
            DELib_ECConstructorCharacter_StopAgent(_objectAddress);
        }
    }
}
