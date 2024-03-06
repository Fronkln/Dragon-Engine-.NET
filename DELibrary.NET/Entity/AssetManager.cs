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

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CASSET_MANAGER_FIND_NEAREST_ASSET_FROM_ALL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_AssetManager_FindNearestAssetFromAll(Vector4 position, uint search_type);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CASSET_MANAGER_FIND_NEAREST_ASSET_FROM_COLLECTION", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_AssetManager_FindNearestAssetFromCollection(Vector4 position, IntPtr vector, uint search_type);

        /// <summary>
        /// Immediately create an asset. (wow)
        /// </summary>
        public static EntityHandle<AssetUnit> CreateAssetImmediate(AssetID asset, Vector4 position, Quaternion rotation)
        {
            return DELibrary_AssetManager_CreateAssetImmediate(asset, ref position, ref rotation);
        }

        /// <summary>
        /// Creates an asset.
        /// </summary>
        public static EntityHandle<AssetUnit> CreateAsset(AssetID asset, Vector4 position, Quaternion rotation)
        {
            return DELibrary_AssetManager_CreateAsset(asset, ref position, ref rotation);
        }

        /// <summary>
        /// Creates an asset.
        /// </summary>
        public static EntityHandle<AssetUnit> FindNearestAssetFromAll(Vector3 position, uint searchType)
        {
            return DELibrary_AssetManager_FindNearestAssetFromAll(position, searchType);
        }

        /// <summary>
        /// Creates an asset.
        /// </summary>
        public static EntityHandle<AssetUnit> FindNearestAssetFromCollection(Vector3 position, IntPtr vector, uint searchType)
        {
            return DELibrary_AssetManager_FindNearestAssetFromCollection(position, vector, searchType);
        }
    }
}
