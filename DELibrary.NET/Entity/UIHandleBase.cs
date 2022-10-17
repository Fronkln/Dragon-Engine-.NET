using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public struct UIHandleBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_PAUSE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_Pause(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_VALUE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetValue(ulong handle, float value);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_WIDTH", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetWidth(ulong handle, float width);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_TEXT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetText(ulong handle, string txt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_FRAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetFrame(ulong handle, float frame);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_TEXTURE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetTexture(ulong handle, uint texture);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_TEXTURE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_UIHandleBase_GetTexture(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_POSITION_V", CallingConvention = CallingConvention.Cdecl)]
        internal static extern Vector4 DELib_UIHandleBase_GetPositionV(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_POSITION_V", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetPositionV(ulong handle, Vector4 vec);


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

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_MATERIAL_COLOR", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetMaterialColor(ulong handle, uint colorID);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_MATERIAL_COLOR2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetMaterialColor2(ulong handle, uint color);

        public ulong Handle;


        public UIHandleBase(ulong handle)
        {
            Handle = handle;
        }

        public Vector4 GetPosition()
        {
            return DELib_UIHandleBase_GetPositionV(Handle);
        }

        public void SetPosition(Vector4 pos)
        {
            DELib_UIHandleBase_SetPositionV(Handle, pos);
        }

        public void SetValue(float value)
        {
            DELib_UIHandleBase_SetValue(Handle, value);
        }

        public void SetWidth(float width)
        {
            DELib_UIHandleBase_SetWidth(Handle, width);
        }

        public void SetText(string text)
        {
            DELib_UIHandleBase_SetText(Handle,text);
        }

        public void SetFrame(float frame)
        {
            DELib_UIHandleBase_SetFrame(Handle, frame);   
        }

        public void SetVisible(bool visible)
        {
            DELib_UIHandleBase_SetVisible(Handle, visible);
        }

        public void SetMaterialColorID(uint col)
        {
            DELib_UIHandleBase_SetMaterialColor(Handle, col);
        }

        public void SetMaterialColor(uint col)
        {
            DELib_UIHandleBase_SetMaterialColor2(Handle, col);
        }

        public void SetTexture(uint texture)
        {
            DELib_UIHandleBase_SetTexture(Handle, texture);
        }

        public uint GetTexture()
        {
            return DELib_UIHandleBase_GetTexture(Handle);
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

        ///<summary>Pause the UI.</summary>
        public void Pause()
        {
            DELib_UIHandleBase_Pause(Handle);
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
