using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class DB
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DB_REFRESH", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_DB_Refresh();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DB_UNLOAD", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_DB_Unload(DbId id);

        ///<summary>Don't use.</summary>
        public static void Refresh()
        {
            DELib_DB_Refresh();
        }

        ///<summary>Don't use.</summary>
        public static void Unload(DbId id)
        {
            DELib_DB_Unload(id);
        }
    }
}
