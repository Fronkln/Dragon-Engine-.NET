using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class AssetUnit : GameObject
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CASSET_UNIT_GETTER_ASSET_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern AssetID DELib_AssetUnit_Getter_AssetID(IntPtr fighterPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CASSET_UNIT_GET_EC_ARMS", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_AssetUnit_Get_EC_Arms(IntPtr asset);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CASSET_UNIT_BREAK_ASSET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_AssetUnit_Break_Asset(IntPtr asset);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CASSET_UNIT_IS_CAN_BREAK", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_AssetUnit_IsCanBreak(IntPtr asset);

        /// <summary>
        /// Asset ID of unit.
        /// </summary>
        public AssetID AssetID { get { return DELib_AssetUnit_Getter_AssetID(Pointer); } }

        /// <summary>
        /// Weapon component of the asset.
        /// </summary>
        public ECAssetArms Arms { get { return new EntityComponentHandle<ECAssetArms>(DELib_AssetUnit_Get_EC_Arms(Pointer)); } }


        public bool IsCanBreak()
        {
            return DELib_AssetUnit_IsCanBreak(Pointer);
        }
        public void Break()
        {
            DELib_AssetUnit_Break_Asset(Pointer);
        }
    }
}
