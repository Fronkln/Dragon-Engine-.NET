﻿using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class Character : CharacterBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_GET_ANG_Y", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DELib_Character_GetAngY(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_SET_ANG_Y", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Character_SetAngY(IntPtr chara, float ang);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCHARACTER_REQUEST_WARP_POSE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Character_RequestWarpPose(IntPtr chara, ref PoseInfo inf);

        public float GetAngleY()
        {
            return DELib_Character_GetAngY(_objectAddress);
        }

        public void SetAngleY(float angle)
        {
            DELib_Character_SetAngY(_objectAddress, angle);
        }

        public void RequestWarpPose(PoseInfo inf)
        {
            DELib_Character_RequestWarpPose(_objectAddress, ref inf);
        }
    }
}
