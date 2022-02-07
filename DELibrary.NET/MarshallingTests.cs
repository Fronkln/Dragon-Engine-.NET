using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class MarshallingTests
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_NPCMATERIAL_MARSHALTEST", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DELib_NPCMaterial_MarshalTest(IntPtr mat);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_AIUTIL_COMMAND_MARSHALTEST", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DELib_AIUtilCommandElement_MarshalTest(ref AIUtilCommandElement mat);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_NPCREQUESTMATERIAL_MARSHALTEST", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DELib_NPCRequestMaterial_MarshalTest(ref NPCRequestMaterial mat);
    }
}
