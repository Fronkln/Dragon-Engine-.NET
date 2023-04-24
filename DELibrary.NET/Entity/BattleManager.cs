﻿using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class BattleManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "CBATTLE_MANAGER_GETTER_PAD", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_BattleManager_Getter_Pad();

        public static PadInputInfo PadInfo
        {
            get
            {
                return new PadInputInfo() { Pointer = DELib_BattleManager_Getter_Pad() };
            }
        }
    }
}


