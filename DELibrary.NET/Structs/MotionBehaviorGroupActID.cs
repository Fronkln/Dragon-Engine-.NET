using System;
using System.Runtime.InteropServices;


namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MotionBehaviorGroupActID
    {
        public BehaviorGroupID group;
        public BehaviorActionID action;
    }
}
