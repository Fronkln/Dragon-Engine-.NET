using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{

    public class BattleCommandAI
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLECOMMANDAI_GETTER_OWNER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_BattleCommandAI_Getter_Owner(IntPtr commandAI);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLECOMMANDAI_GETTER_IS_INITIALIZED", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_BattleCommandAI_Getter_IsInitialized(IntPtr commandAI);

        /// <summary>
        /// Owner of this AI
        /// </summary>
        public Character Owner
        {
            get
            {
                IntPtr ptr = DELib_BattleCommandAI_Getter_Owner(Pointer);
                Character chara = new Character();
                chara.Pointer = ptr;

                return chara;
            }
        }

        public IntPtr Pointer { get; internal set; }

        /// <summary>
        /// Is the AI initialized
        /// </summary>
        /// <returns></returns>
        public bool IsInitialized()
        {
            return DELib_BattleCommandAI_Getter_IsInitialized(Pointer);
        }
    }
}
