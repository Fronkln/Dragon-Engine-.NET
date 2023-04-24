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
            m_keyboardActive = (KeyboardActiveDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)0x141830330, typeof(KeyboardActiveDelegate));
        }

        public static bool IsKeyboardActive()
        {
            uint result = m_keyboardActive.Invoke();
            return result == 1;
        }
    }
}
