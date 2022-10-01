using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public struct UIHandleBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_VALUE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetValue(ulong handle, float value);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_WIDTH", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetWidth(ulong handle, float width);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_PLAY_ANIMATION_SET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_PlayAnimationSet(ulong handle, uint id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_VISIBLE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetVisible(ulong handle, bool visible);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_CONTROL_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_UIHandleBase_GetControlID(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_CHILD_NUM", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_UIHandleBase_GetChildCount(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_CHILD", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong DELib_UIHandleBase_GetChild(ulong handle, int index);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_TRANSITION_ANIMATION_SET_TO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong DELib_UIHandleBase_TransitionAnimationSetTo(ulong handle, uint scene_id, uint animation_set_data_id, uint focus_operation_id);

        public ulong Handle;

        public void SetValue(float value)
        {
            DELib_UIHandleBase_SetValue(Handle, value);
        }

        public void SetWidth(float width)
        {
            DELib_UIHandleBase_SetWidth(Handle, width);
        }

        public void SetVisible(bool visible)
        {
            DELib_UIHandleBase_SetVisible(Handle, visible);
        }

        public uint GetControlID()
        {
            return DELib_UIHandleBase_GetControlID(Handle);
        }

        public uint GetChildCount()
        {
            return DELib_UIHandleBase_GetChildCount(Handle);
        }
        public UIHandleBase GetChild(int index)
        {
            return new UIHandleBase() { Handle = DELib_UIHandleBase_GetChild(Handle, index) };
        }

        ///<summary>Play animation set on UI.</summary>
        public void PlayAnimationSet(uint id)
        {
            DELib_UIHandleBase_PlayAnimationSet(Handle, id);
        }


        public void TransitionAnimationSetTo(uint scene_id, uint animation_set_data_id, uint focus_operation_id)
        {
            DELib_UIHandleBase_TransitionAnimationSetTo(Handle, scene_id, animation_set_data_id, focus_operation_id);
        }
    }
}
