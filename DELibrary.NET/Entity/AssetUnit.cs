using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class AssetUnit : GameObject
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CASSET_UNIT_GETTER_ASSET_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern AssetID DELib_AssetUnit_Getter_AssetID(IntPtr fighterPtr);

        /// <summary>
        /// Asset ID of unit.
        /// </summary>
        public AssetID AssetID { get { return DELib_AssetUnit_Getter_AssetID(Pointer); } }
    }
}
