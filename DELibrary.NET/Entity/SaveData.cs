using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class SaveData
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CSAVE_DATA_GET_ITEM", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetItem(uint chunkID);
        [DllImport("Y7Internal.dll", EntryPoint = "PLAYER_GET_CCHARA_DATA", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetCharaData(uint charaIdx);
    }
}
