using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public class WeaponManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_WEAPONMANAGER_EQUIPDEFAULT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_WeaponManager_EquipDefault(IntPtr wepManager, IntPtr fighterPtr);


        public IntPtr Pointer;

        public void EquipDefault(Fighter fighter)
        {
            DELib_WeaponManager_EquipDefault(Pointer, fighter._ptr);
        }
    }
}
