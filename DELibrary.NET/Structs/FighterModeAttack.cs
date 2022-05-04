using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public class FighterModeAttack : HumanMode
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTERMODE_ATTACK_CREATE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_FighterModeAttack_Create(IntPtr humanModeManager, FighterCommandID id);

        public static FighterModeAttack Create(HumanModeManager manager, FighterCommandID id)
        {
            FighterModeAttack obj = new FighterModeAttack();
            obj.m_pointer =  DELib_FighterModeAttack_Create(manager.Pointer, id);

            return obj;
        }
    }
}
