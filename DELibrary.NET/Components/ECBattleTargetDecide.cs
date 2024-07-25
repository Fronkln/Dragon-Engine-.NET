using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECBattleTargetDecide : ECCharaComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLETARGETDECIDE_SETTER_AUTH_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ECBattleTargetDecide_Setter_Target(IntPtr ptr, uint chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ECBATTLETARGETDECIDE_GETTER_AUTH_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_ECBattleTargetDecide_Getter_Target(IntPtr ptr);

        ///<summary>Set the target of the target decider.</summary>
        public void SetTarget(FighterID id)
        {
            DELib_ECBattleTargetDecide_Setter_Target(Pointer, id.Handle);
        }

        public EntityHandle<Character> GetTarget()
        {
            return DELib_ECBattleTargetDecide_Getter_Target(Pointer);
        }
    }
}
