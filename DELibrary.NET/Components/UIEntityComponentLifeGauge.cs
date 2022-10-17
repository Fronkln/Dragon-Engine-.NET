using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class UIEntityComponentLifeGauge : EntityComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CUI_ENTITY_COMPONENT_LIFE_GAUGE_GETTER_UI_ROOT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong DELib_UIEntityComponentLifeGauge_Getter_UIRoot(IntPtr gauge);

        public UIHandleBase UIRoot
        {
            get { return new UIHandleBase(DELib_UIEntityComponentLifeGauge_Getter_UIRoot(Pointer)); }
        }
    }
}
