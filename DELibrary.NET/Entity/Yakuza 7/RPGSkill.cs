using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
#if TURN_BASED_GAME
    public static class RPGSkill
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_RPG_SKILL_GET_SKILL_COMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_RPGSkill_GetSkillCommand(IntPtr inVal, uint skill, uint jobID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_RPG_SKILL_SET_EX_EFFECT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_RPGSkill_SetExEffect(ref uint in_fighter, uint effParamID, ref uint inBootFighter, int skillID, int itemID, bool dontShowMsg, bool fromPreExec);
        public static IntPtr GetSkillCommand(RPGSkillID skillId, RPGJobID jobID = RPGJobID.invalid)
        {
            IntPtr buf = Marshal.AllocHGlobal(0xD8);
            return DELib_RPGSkill_GetSkillCommand(buf, (uint)skillId, (uint)jobID);
        }

        public static void SetExEffect(uint in_fighter, uint effParamID, uint inBootFighter, int skillID, int itemID, bool dontShowMsg, bool fromPreExec)
        {
            DELib_RPGSkill_SetExEffect(ref in_fighter, effParamID, ref inBootFighter, skillID, itemID, dontShowMsg, fromPreExec);
        }
    }
#endif
}
