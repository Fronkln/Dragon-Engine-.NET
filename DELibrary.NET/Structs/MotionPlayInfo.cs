using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 0x60)]
    public struct MotionPlayInfo
    {
        //motion::PlayInfoVtbl *vfptr;

        [FieldOffset(0x0)] public IntPtr vfptr;
        [FieldOffset(0x10)] public Vector4 jaunt_goal_;
        [FieldOffset(0x20)] public Vector4 blend_space_param_;
        [FieldOffset(0x30)] public MotionBehaviorGroupActID behavior_id_;
        [FieldOffset(0x38)] public MotionBehaviorGroupActID behavior_id_next_;
        //cbehavior_property_play
        [FieldOffset(0x40)] public uint bep_handle_;
        [FieldOffset(0x44)] public MotionID gmt_id_;
        [FieldOffset(0x48)] public uint tick_gmt_now_;
        [FieldOffset(0x4C)] public uint tick_now_;
        [FieldOffset(0x50)] public uint tick_old_;
        [FieldOffset(0x54)] public uint tick_blend_;
        [FieldOffset(0x58)] public uint tick_key_;
        [FieldOffset(0x5C)] public bool no_blend_;
        [FieldOffset(0x5D)] public bool ignore_trans_;
    

        /// <summary>
        /// Dont use
        /// </summary>
        public void Set(MotionPlayInfo value)
        {
          //  vfptr = value.vfptr;
            jaunt_goal_ = value.jaunt_goal_;
            blend_space_param_ = value.blend_space_param_;
            behavior_id_ = value.behavior_id_;
            behavior_id_next_ = value.behavior_id_next_;
            gmt_id_ = value.gmt_id_;
            tick_gmt_now_ = value.tick_gmt_now_;
            tick_now_ = value.tick_now_;
            tick_old_ = value.tick_old_;
            tick_blend_ = value.tick_blend_;
            tick_key_ = value.tick_key_;
            no_blend_ = value.no_blend_;
            ignore_trans_ = value.ignore_trans_;
        }
    }
}
