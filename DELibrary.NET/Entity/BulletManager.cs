using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class BulletManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_BULLETMANAGER_PLAYEFF", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_BulletManager_PlayEffect(Vector4 pos, Vector4 dir, ParticleID effect_id, float ptc_scale_z);

        public static void PlayEffect(Vector4 pos, Vector4 dir, ParticleID effect_id, float ptc_scale_z)
        {
            DELib_BulletManager_PlayEffect(pos, dir, effect_id, ptc_scale_z);
        }
    }
}
