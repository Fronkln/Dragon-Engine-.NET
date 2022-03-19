using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class ParticleManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_MANAGER_PLAY1", CallingConvention = CallingConvention.Cdecl)]
        internal extern static uint DELib_ParticleManager_Play(ParticleID particleID, IntPtr matrix, ParticleType type);

        public static EntityHandle<ParticleInterface> Play(ParticleID particleID, Matrix4x4 matrix, ParticleType type)
        {
            IntPtr matrixMem = matrix.ToIntPtr();
            uint result = DELib_ParticleManager_Play(particleID, matrixMem, type);

            return result;
        }
    }
}
