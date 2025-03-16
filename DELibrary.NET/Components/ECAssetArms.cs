using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECAssetArms : EntityComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_EC_ASSET_ARMS_GETTER_UNIT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_AssetUnit_Getter_Unit(IntPtr armsPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_EC_ASSET_ARMS_GETTER_ARMS_TYPE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ArmsType DELib_AssetUnit_Getter_ArmsType(IntPtr armsPtr);

        //public AssetUnit Unit { get { return new EntityHandle<AssetUnit>(DELib_AssetUnit_Getter_Unit(Pointer)); } }

        public AssetUnit Unit { get { return new EntityHandle<AssetUnit>(Owner.UID); } }

        public ArmsType ArmsType { get { return DELib_AssetUnit_Getter_ArmsType(Pointer); } }


        public ECAssetArmsSingle AsSingle()
        {
            if (ArmsType == ArmsType.single)
                return new EntityComponentHandle<ECAssetArmsSingle>(UID);
            return null;
        }
    }
}
