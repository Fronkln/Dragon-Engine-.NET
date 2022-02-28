using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class HumanModeManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOREADY", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_ToReady(IntPtr manager, bool no_blend);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOSTAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToStand(IntPtr manager, RequireType reqType);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOKAMAE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToKamae(IntPtr manager, bool no_blend, bool force_kamae, bool no_start_req);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOMOVE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToMove(IntPtr manager, bool no_blend);

        public enum RequireType
        {
            Normal = 0x0,
            Immediate = 0x1,
            NoBlend = 0x2,
            Count = 0x3,
        };

        public IntPtr Pointer { get; internal set; }

        public bool ToReady(bool no_blend) { return DELib_HumanModeManager_ToReady(Pointer, no_blend); }
        public void ToStand(RequireType reqType) => DELib_HumanModeManager_ToStand(Pointer, reqType);
        public void ToKamae(bool no_blend, bool force_kamae, bool no_start_req) => DELib_HumanModeManager_ToKamae(Pointer, no_blend, force_kamae, no_start_req);
        public void ToMove(bool no_blend) => DELib_HumanModeManager_ToMove(Pointer, no_blend);
    }
}
