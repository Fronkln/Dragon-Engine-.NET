using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
#if YLAD
    public static class RPGSkill
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_RPG_SKILL_GET_SKILL_COMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_RPGSkill_GetSkillCommand(IntPtr inVal, uint skill, uint jobID);
        public static IntPtr GetSkillCommand(RPGSkillID skillId, RPGJobID jobID = RPGJobID.invalid)
        {
            IntPtr buf = Marshal.AllocHGlobal(0xD8);
            return DELib_RPGSkill_GetSkillCommand(buf, (uint)skillId, (uint)jobID);
        }
    }
#endif
}
