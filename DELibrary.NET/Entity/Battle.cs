using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class Battle
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLE_PLAYEFFECT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_Battle_PlayEffect(ParticleID id, IntPtr matrix);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BATTLE_LOAD_INIT_FILE", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LoadInitFile();

        ///<summary>Play battle effect. (Crash prone?)</summary>
        public static void PlayEffect(ParticleID id, Matrix4x4 matrix)
        {
            IntPtr matrixPtr = matrix.ToIntPtr();
            DELib_Battle_PlayEffect(id, matrixPtr);

            Marshal.AllocHGlobal(matrixPtr);
        }
    }
}
