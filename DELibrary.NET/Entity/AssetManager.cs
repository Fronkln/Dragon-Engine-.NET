using System;
using System.Runtime.InteropServices;


namespace DragonEngineLibrary
{
    public static class AssetManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CASSET_MANAGER_CREATE_ASSET_IMMEDIATE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_AssetManager_CreateAssetImmediate(AssetID asset, ref Vector4 position, ref Quaternion rotation);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CASSET_MANAGER_CREATE_ASSET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_AssetManager_CreateAsset(AssetID asset, ref Vector4 position, ref Quaternion rotation);

        public static EntityHandle<AssetUnit> CreateAssetImmediate(AssetID asset, Vector4 position, Quaternion rotation)
        {
            return DELibrary_AssetManager_CreateAssetImmediate(asset, ref position, ref rotation);
        }

        public static EntityHandle<AssetUnit> CreateAsset(AssetID asset, Vector4 position, Quaternion rotation)
        {
            return DELibrary_AssetManager_CreateAsset(asset, ref position, ref rotation);
        }
    }
}
