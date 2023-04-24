using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class UIEntityPlayerGauge : EntityBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CUI_ENTITY_PLAYER_GAUGE_INIT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIEntityPlayerGauge_Init(IntPtr gauge, ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CUI_ENTITY_PLAYER_GAUGE_GETTER_GAUGE_FRAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong DELib_UIEntityPlayerGauge_Getter_Gauge_Frame(IntPtr gauge);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CUI_ENTITY_PLAYER_GAUGE_GETTER_GAUGE_NOW", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong DELib_UIEntityPlayerGauge_Getter_Gauge_Now(IntPtr gauge);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CUI_ENTITY_PLAYER_GAUGE_JOB_UPDATE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIEntityPlayerGauge_JobUpdate(IntPtr gauge);


        public UIHandleBase LifeGauge
        {
            get
            {
                return new UIHandleBase();
            }
            set
            {
                DELib_UIEntityPlayerGauge_Init(Pointer, value.Handle);
            }
        }
        public UIHandleBase GaugeFrame
        {
            get
            {
                return new UIHandleBase() { Handle = DELib_UIEntityPlayerGauge_Getter_Gauge_Frame(Pointer) };
            }
        }

        public UIHandleBase GaugeNow
        {
            get
            {
                return new UIHandleBase() { Handle = DELib_UIEntityPlayerGauge_Getter_Gauge_Now(Pointer) };
            }
        }


        public void JobUpdate()
        {
            //DELib_UIEntityPlayerGauge_JobUpdate(Pointer);
        }
    }
}
