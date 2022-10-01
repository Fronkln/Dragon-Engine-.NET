using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class Asset
    {
        [DllImport("Y7Internal.dll", EntryPoint = "CASSET_GET_ARMS_CATEGORY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern AssetArmsCategoryID DELib_Asset_GetArmsCategory(AssetID id);

        [DllImport("Y7Internal.dll", EntryPoint = "CASSET_GET_ARMS_CATEGORY_SUB", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_Asset_GetArmsCategorySub(AssetID id);

        /// <summary> Get the weapon category of this asset </summary>
        public static AssetArmsCategoryID GetArmsCategory(AssetID id)
        {
            return DELib_Asset_GetArmsCategory(id);
        }

        public static uint GetArmsCategorySub(AssetID id)
        {
            return DELib_Asset_GetArmsCategorySub(id);
        }
    }
}
