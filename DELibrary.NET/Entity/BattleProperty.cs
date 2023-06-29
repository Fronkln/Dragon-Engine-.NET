using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class BattleProperty
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLE_PROPERTY_GETTER_BATTLE_CONFIG_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_BattleProperty_Getter_BattleConfigID();
        public static uint BattleConfigID { get { return DELib_BattleProperty_Getter_BattleConfigID(); } }
    }
}
