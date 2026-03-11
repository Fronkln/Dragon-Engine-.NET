using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary.ImGuizmoNET
{
    public enum OPERATION
    {
        TRANSLATE,
        ROTATE,
        SCALE,
        BOUNDS,
    }

    public enum MODE
    {
        LOCAL,
        WORLD
    }

    public static class ImGuizmo
    {
        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_SetDrawlist", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetDrawlist(IntPtr drawlist);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_BeginFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BeginFrame();

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_SetImGuiContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetImGuiContext(IntPtr ctx);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_IsOverNil", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool IsOver();

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_IsOverOPERATION", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool IsOver(OPERATION op);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_IsUsing", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool IsUsing();

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_Enable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Enable([MarshalAs(UnmanagedType.U1)] bool enable);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_DecomposeMatrixToComponents", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DecomposeMatrixToComponents(ref float matrix, ref float translation, ref float rotation, ref float scale);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_RecomposeMatrixFromComponents", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RecomposeMatrixFromComponents(ref float translation, ref float rotation, ref float scale, ref float matrix);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_SetRect", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetRect(float x, float y, float width, float height);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_SetOrthographic", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetOrthographic([MarshalAs(UnmanagedType.U1)] bool isOrthographic);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_DrawCubes", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DrawCubes(ref float view, ref float projection, ref float matrices, int matrixCount);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_DrawGrid", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DrawGrid(ref float view, ref float projection, ref float matrix, float gridSize);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_Manipulate", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool Manipulate(
            ref float view,
            ref float projection,
            OPERATION operation,
            MODE mode,
            ref float matrix,
            IntPtr deltaMatrix,
            IntPtr snap,
            IntPtr localBounds,
            IntPtr boundsSnap);

        public static bool Manipulate(ref float view, ref float projection, OPERATION operation, MODE mode, ref float matrix)
        {
            return Manipulate(ref view, ref projection, operation, mode, ref matrix, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_ViewManipulate", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ViewManipulate(ref float view, float length, Vector2 position, Vector2 size, uint backgroundColor);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_SetID", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetID(int id);

        [DllImport("cimgui.dll", EntryPoint = "ImGuizmo_SetGizmoSizeClipSpace", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetGizmoSizeClipSpace(float value);
    }
}
