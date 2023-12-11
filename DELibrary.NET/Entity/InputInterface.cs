using System;
using System.Runtime.InteropServices;


namespace DragonEngineLibrary
{
    public static class InputInterface
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint KeyboardActiveDelegate();

        private static KeyboardActiveDelegate m_keyboardActive;

        static InputInterface()
        {
            //TODO: Move to CPP
            m_keyboardActive = (KeyboardActiveDelegate)Marshal.GetDelegateForFunctionPointer(Unsafe.CPP.PatternSearch("48 83 EC ? E8 ? ? ? ? FF C8 83 F8 ?"), typeof(KeyboardActiveDelegate));
        }

        public static bool IsKeyboardActive()
        {
            uint result = m_keyboardActive.Invoke();
            return result == 1;
        }
    }
}
