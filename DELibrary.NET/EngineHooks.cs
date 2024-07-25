using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class EngineHooks
    {
        private static bool m_init = false;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate void TalkTextManagerConstructor(IntPtr textMan, EntityUID uid, uint parent);

        public static void Initialize()
        {
            if (m_init)
                return;

            return;
        }
    }
}
