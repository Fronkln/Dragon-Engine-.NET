using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECBattleTargetDecide : ECCharaComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLETARGETDECIDE_SETTER_AUTH_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ECBattleTargetDecide_Setter_Target(IntPtr ptr, uint chara);

        ///<summary>Set the target of the target decider.</summary>
        public void SetTarget(FighterID id)
        {
            DELib_ECBattleTargetDecide_Setter_Target(Pointer, id.Handle);
        }
    }
}
