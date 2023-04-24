using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public class ECAssetArmsSingle : ECAssetArms
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_EC_ASSET_ARMS_SINGLE_GETTER_USE_COUNT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DELib_ECAssetArmsSingle_Getter_UseCount(IntPtr armsPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_EC_ASSET_ARMS_SINGLE_SETTER_USE_COUNT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DELib_ECAssetArmsSingle_Setter_UseCount(IntPtr armsPtr, int value);

        public int UseCount { get { return DELib_ECAssetArmsSingle_Getter_UseCount(Pointer); } set { DELib_ECAssetArmsSingle_Setter_UseCount(Pointer, value); } }
    }
}
