using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class UIEntityComponentEnemyLifeGauge : UIEntityComponentLifeGauge
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CUI_ENTITY_COMPONENT_ENEMY_LIFE_GAUGE_SET_CATEGORY_NAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_UIEntityComponentEnemyLifeGauge_SetCategoryName(IntPtr gauge, string name);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CUI_ENTITY_COMPONENT_ENEMY_LIFE_GAUGE_ATTACH", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_UIEntityComponentEnemyLifeGauge_Attach(IntPtr character);


        public void SetCategoryName(string name)
        {
            DELib_UIEntityComponentEnemyLifeGauge_SetCategoryName(Pointer, name);
        }

        public static EntityComponentHandle<UIEntityComponentEnemyLifeGauge> Attach(Character chara)
        {
            return DELib_UIEntityComponentEnemyLifeGauge_Attach(chara.Pointer);
        }
    }
}
