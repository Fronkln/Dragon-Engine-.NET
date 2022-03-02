using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECSoldierInfo : EntityComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_SOLDIER_INFO_GETTER_HEALTH_GAUGE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_ECSoldierInfo_Getter_HealthGauge(IntPtr agent);


        public EntityComponentHandle<UIEntityComponentEnemyLifeGauge> HealthGauge
        {
            get { return DELib_ECSoldierInfo_Getter_HealthGauge(Pointer); }
        }
    }
}
