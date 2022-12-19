using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECBattleStatus : ECCharaComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_ATTACH", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_ECBattleStatus_Attach(IntPtr chara);

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

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_GETTER_FIGHTER_LEVEL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_ECBattleStatus_Getter_FighterLevel(IntPtr battlestatus);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_SETTER_FIGHTER_LEVEL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_Setter_FighterLevel(IntPtr battlestatus, uint level);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_GETTER_ATTACK_POWER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_ECBattleStatus_Getter_AttackPower(IntPtr battlestatus);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_SETTER_ATTACK_POWER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_Setter_AttackPower(IntPtr battlestatus, uint power);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_GETTER_SP_ATTACK_POWER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_ECBattleStatus_Getter_SPAttackPower(IntPtr battlestatus);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_SETTER_SP_ATTACK_POWER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_Setter_SPAttackPower(IntPtr battlestatus, uint power);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_GETTER_DEFENSE_POWER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_ECBattleStatus_Getter_DefensePower(IntPtr battlestatus);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_SETTER_DEFENSE_POWER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_Setter_DefensePower(IntPtr battlestatus, uint power);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_GETTER_HEAT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DELibrary_ECBattleStatus_Getter_Heat(IntPtr battlestatus);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_SETTER_HEAT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_Setter_Heat(IntPtr battlestatus, int heat);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_GETTER_ACTION_COMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern BattleCommandSetID DELibrary_ECBattleStatus_Getter_ActionCommand(IntPtr battlestatus);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_SETTER_ACTION_COMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_Setter_ActionCommand(IntPtr battlestatus, BattleCommandSetID set);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_GET_ARTS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_ECBattleStatus_Get_Arts(IntPtr battlestatus);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_GETTER_RPG_COMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern BattleCommandSetID DELibrary_ECBattleStatus_Getter_RPGCommand(IntPtr battlestatus);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_SETTER_RPG_COMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_Setter_RPGCommand(IntPtr battlestatus, BattleCommandSetID set);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_CLEAR_COMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_ClearCommand(IntPtr battlestatus);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_SET_SUPER_ARMOR", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_SetSuperArmor(IntPtr battlestatus, bool armor, bool isSuperHard = false);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_IS_SUPER_ARMOR", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELibrary_ECBattleStatus_IsSuperArmor(IntPtr battlestatus);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLESTATUS_ADD_DAMAGE_INFO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECBattleStatus_AddDamageInfo(IntPtr battlestatus, ref BattleDamageInfo info);

        public static EntityComponentHandle<ECBattleStatus> Attach(Character character)
        {
            return DELibrary_ECBattleStatus_Attach(character.Pointer);
        }

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

        public int Heat
        {
            get
            {
                return DELibrary_ECBattleStatus_Getter_Heat(_objectAddress);
            }
            set
            {
                DELibrary_ECBattleStatus_Setter_Heat(_objectAddress, value);
            }
        }

        ///<summary>RPG Command set of th echaracter.</summary>
        public BattleCommandSetID RPGCommand
        {
            get
            {
                return DELibrary_ECBattleStatus_Getter_RPGCommand(_objectAddress);
            }
            set
            {
                DELibrary_ECBattleStatus_Setter_RPGCommand(_objectAddress, value);
            }
        }

        ///<summary>Action command set of the character.</summary>
        public BattleCommandSetID ActionCommand
        {
            get
            {
                return DELibrary_ECBattleStatus_Getter_ActionCommand(_objectAddress);
            }
            set
            {
                DELibrary_ECBattleStatus_Setter_ActionCommand(_objectAddress, value);
            }
        }

        public uint Level
        {
            get { return DELibrary_ECBattleStatus_Getter_FighterLevel(Pointer); }
            set { DELibrary_ECBattleStatus_Setter_FighterLevel(Pointer, value); }
        }

        public uint AttackPower
        {
            get { return DELibrary_ECBattleStatus_Getter_AttackPower(Pointer); }
            set { DELibrary_ECBattleStatus_Setter_AttackPower(Pointer, value); }
        }

        public uint SPAttackPower
        {
            get { return DELibrary_ECBattleStatus_Getter_SPAttackPower(Pointer); }
            set { DELibrary_ECBattleStatus_Setter_SPAttackPower(Pointer, value); }
        }

        public uint DefensePower
        {
            get { return DELibrary_ECBattleStatus_Getter_DefensePower(Pointer); }
            set { DELibrary_ECBattleStatus_Setter_DefensePower(Pointer, value); }
        }

        ///<summary>Set current alive HP?? What??</summary>
        public void SetAliveHPCurrent(long hp)
        {
            DELibrary_ECBattleStatus_SetAliveHPCurrent(_objectAddress, hp);
        }

        ///<summary>Set max HP.</summary>
        public void SetHPMax(long hp)
        {
           DELibrary_ECBattleStatus_SetHPMax(_objectAddress, hp);
        }

        ///<summary>Set current HP.</summary>
        public void SetHPCurrent(long hp)
        {
            DELibrary_ECBattleStatus_SetHPCurrent(_objectAddress, hp);
        }

        ///<summary>Clear fighter command.</summary>
        public void ClearCommand()
        {
            DELibrary_ECBattleStatus_ClearCommand(Pointer);
        }

        ///<summary>Get the battle AI of the character.</summary>
        public BattleCommandAI GetBattleAI()
        {
            IntPtr addr = DELibrary_ECBattleStatus_GetBattleAI(_objectAddress);
            BattleCommandAI ai = new BattleCommandAI();
            ai.Pointer = addr;

            return ai;
        }


        public void SetSuperArmor(bool armor, bool isSuperHard = false)
        {
            DELibrary_ECBattleStatus_SetSuperArmor(Pointer, armor, isSuperHard);
        }

        public bool IsSuperArmor()
        {
           return DELibrary_ECBattleStatus_IsSuperArmor(Pointer);
        }

        public uint GetArts()
        {
            return DELibrary_ECBattleStatus_Get_Arts(Pointer);
        }

        public void AddDamageInfo(BattleDamageInfo inf)
        {
          //  IntPtr dmgPtr = inf.ToIntPtr();
            DELibrary_ECBattleStatus_AddDamageInfo(Pointer, ref inf);
        }

        
    }
}
