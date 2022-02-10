using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    public class Sphere
    {
        public Vector3 Center;
        public float Radius;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class AABox : Sphere
    {
        public Vector4 Extent;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class OrBox : AABox
    {
        public Quaternion Orient;
    }
}
