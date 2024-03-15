using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class Player
    {
#if IW_AND_UP
        public enum ID : uint
        {
            invalid,         // constant 0x0
            kiryu = 1,
            yagami = 2,
            majima = 3,
            kasuga = 4,
            sakaida = 5,
            adachi = 6,
            hoshino = 7,
            test_player = 8,
            saeko = 9,
            nanba = 10,
            ayaka = 11,
            chou = 12,
            jyungi = 13,
            woman_a = 14,
            coyote_kasuga = 15,
            saori = 16,
            kaito = 17,
            saori_dress = 18,
            chitose = 19,
            tomizawa = 20,
            sonhi = 21,
        }
#else
        public enum ID
        {
            invalid,         // constant 0x0
            kiryu,       // constant 0x1
            yagami,      // constant 0x2
            majima,      // constant 0x3
            kasuga,      // constant 0x4
            sakaida,         // constant 0x5
            adachi,      // constant 0x6
            hoshino,         // constant 0x7
            test_player,         // constant 0x8
            saeko,       // constant 0x9
            nanba,       // constant 0xA
            ayaka,       // constant 0xB
            chou,        // constant 0xC
            jyungi,      // constant 0xD
            woman_a,		 // constant 0xE
        }
#endif


        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_GET_HP_NOW", CallingConvention = CallingConvention.Cdecl)]
        internal extern static long DELib_Player_GetHPNow(ID player);

        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_GET_HP_MAX", CallingConvention = CallingConvention.Cdecl)]
        internal extern static long DELib_Player_GetHPMax(ID player);


        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_GET_HEAT_NOW", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int DELib_Player_GetHeatNow(ID player);

        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_GET_HEAT_MAX", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int DELib_Player_GetHeatMax(ID player);


        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_SET_HEAT_NOW", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void DELib_Player_SetHeatNow(ID player, int val);

#if TURN_BASED_GAME

        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_SET_LEVEL", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void DELib_Player_SetLevel(uint level, ID playerID, IntPtr saveData = default(IntPtr));

        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_GET_LEVEL", CallingConvention = CallingConvention.Cdecl)]
        internal extern static uint DELib_Player_GetLevel(ID playerID);

        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_SET_JOB_LEVEL", CallingConvention = CallingConvention.Cdecl)]
        internal extern static uint DELib_Player_SetJobLevel(RPGJobID job, uint level, ID player, bool levelUpFullRecover, bool adjustXP);

        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_GET_JOB_LEVEL", CallingConvention = CallingConvention.Cdecl)]
        internal extern static uint DELib_Player_GetJobLevel(RPGJobID job, ID player);

        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_GET_CURRENT_JOB", CallingConvention = CallingConvention.Cdecl)]
        internal extern static RPGJobID DELib_Player_GetCurrentJob(ID player);

        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_SET_CURRENT_JOB", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void DELib_Player_SetCurrentJob(ID player, RPGJobID job, bool recoverHPMPDifference);
#endif
#if TURN_BASED_GAME
        ///<summary>Get current HP of player.</summary>
        public static long GetHPNow(ID playerID)
        {
            return DELib_Player_GetHPNow(playerID);
        }

        ///<summary>Get maximum HP of player.</summary>
        public static long GetHPMax(ID playerID)
        {
            return DELib_Player_GetHPMax(playerID);
        }

        ///<summary>Get heat of player.</summary>
        public static int GetHeatNow(ID playerID)
        {
            return DELib_Player_GetHeatNow(playerID);
        }

        ///<summary>Get maximum heat of player.</summary>
        public static int GetHeatMax(ID playerID)
        {
            return DELib_Player_GetHeatMax(playerID);
        }

        ///<summary>Set heat of the player.</summary>
        public static void SetHeatNow(ID playerID, int val)
        {
            DELib_Player_SetHeatNow(playerID, val);
#endif
    }

#if TURN_BASED_GAME
        ///<summary>Set current level of player.</summary>
        public static void SetLevel(uint level, ID playerID)
        {
            DELib_Player_SetLevel(level, playerID, IntPtr.Zero);
        }
        ///<summary>Set current level of player.</summary>
        public static void SetLevel(uint level, ID playerID, IntPtr saveData)
        {
            DELib_Player_SetLevel(level, playerID, saveData);
        }

        ///<summary>Get level of player.</summary>
        public static uint GetLevel(ID player)
        {
            return DELib_Player_GetLevel(player);
        }

        ///<summary>Set current job level of player.</summary>
        public static void SetJobLevel(uint level, ID player, bool levelUpFullRecover = true, bool adjustXP = true)
        {
            DELib_Player_SetJobLevel(GetCurrentJob(player), level, player, levelUpFullRecover, adjustXP);
        }

        ///<summary>Set job level of player.</summary>
        public static void SetJobLevel(RPGJobID job, uint level, ID player, bool levelUpFullRecover = true, bool adjustXP = true)
        {
            DELib_Player_SetJobLevel(job, level, player, levelUpFullRecover, adjustXP);
        }

        ///<summary>Get current job level of player.</summary>
        public static uint GetJobLevel(ID player)
        {
            return DELib_Player_GetJobLevel(GetCurrentJob(player), player);
        }

        ///<summary>Get job level of player</summary>
        public static uint GetJobLevel(RPGJobID job, ID player)
        {
            return DELib_Player_GetJobLevel(job, player);
        }

        ///<summary>Get current job of player.</summary>
        public static RPGJobID GetCurrentJob(ID player)
        {
            return DELib_Player_GetCurrentJob(player);
        }

        ///<summary>Set current job of player.</summary>
        public static void SetCurrentJob(ID player, RPGJobID job, bool recoverHPAndMPDifference = true)
        {
            DELib_Player_SetCurrentJob(player, job, recoverHPAndMPDifference);
        }

}
#endif
}
