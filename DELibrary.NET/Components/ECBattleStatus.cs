using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECBattleStatus : ECCharaComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_GETBATTLEAI", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELibrary_ECBattleStatus_GetBattleAI(IntPtr battlestatus);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_SETALIVEHPCURRENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_SetAliveHPCurrent(IntPtr battlestatus, long hp);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_SETHPMAX", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_SetHPMax(IntPtr battlestatus, long hp);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_SETHPCURRENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_SetHPCurrent(IntPtr battlestatus, long hp);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_GETTER_MAXHP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern long DELibrary_ECBattleStatus_Getter_MaxHp(IntPtr battlestatus);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_GETTER_CURRENTHP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern long DELibrary_ECBattleStatus_Getter_CurrentHp(IntPtr battlestatus);

        public long HPLimit
        {
            get
            {
                return DELibrary_ECBattleStatus_Getter_MaxHp(_objectAddress);
            }
            set
            {
                SetHPMax(value);
            }
        }

        public long CurrentHP
        {
            get
            {
                return DELibrary_ECBattleStatus_Getter_CurrentHp(_objectAddress);
            }
            set
            {
                SetHPCurrent(value);
                SetAliveHPCurrent(value);
            }
        }

        public long MaxHP
        {
            get
            {
                return DELibrary_ECBattleStatus_Getter_MaxHp(_objectAddress);
            }
            set
            {
                SetHPMax(value);
            }
        }

        public void SetAliveHPCurrent(long hp)
        {
            DELibrary_ECBattleStatus_SetAliveHPCurrent(_objectAddress, hp);
        }

        public void SetHPMax(long hp)
        {
           DELibrary_ECBattleStatus_SetHPMax(_objectAddress, hp);
        }

        public void SetHPCurrent(long hp)
        {
            DELibrary_ECBattleStatus_SetHPCurrent(_objectAddress, hp);
        }

        public BattleCommandAI GetBattleAI()
        {
            IntPtr addr = DELibrary_ECBattleStatus_GetBattleAI(_objectAddress);
            BattleCommandAI ai = new BattleCommandAI();
            ai.Pointer = addr;

            return ai;
        }
        
    }
}
