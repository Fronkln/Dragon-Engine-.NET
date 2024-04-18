using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECAI : EntityComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_AI_GETTER_AI_MIND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_ECAI_Getter_AIMind(IntPtr ai);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_AI_GETTER_REQUEST_COMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_ECAI_Getter_RequestCommand(IntPtr ai);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_AI_GETTER_LAST_ACCEPT_REQUEST_COMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_ECAI_Getter_LastAcceptRequestCommand(IntPtr ai);

        public AIPackBase Mind
        {
            get
            {
                return new AIPackBase() { Pointer = DELib_ECAI_Getter_AIMind(Pointer) };
            }
        }

        public AIUtilCommand RequestCommand
        {
            get
            {
                IntPtr ptr = DELib_ECAI_Getter_RequestCommand(_objectAddress);

                if (ptr == IntPtr.Zero)
                    return new AIUtilCommand();
                else
                    return Marshal.PtrToStructure<AIUtilCommand>(ptr);
            }
        }
        public AIUtilCommand LastAcceptedRequestCommand
        {
            get
            {
                IntPtr ptr = DELib_ECAI_Getter_LastAcceptRequestCommand(_objectAddress);

                if (ptr == IntPtr.Zero)
                    return new AIUtilCommand();
                else
                    return Marshal.PtrToStructure<AIUtilCommand>(ptr);
            }
        }
    }
}
