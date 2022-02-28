using System;
using System.Runtime.InteropServices;
using DragonEngineLibrary;
using MinHook.NET;

namespace Y7MP
{
    public static class MPHooks
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate void ECMotionRequestGmt(IntPtr motionPtr, IntPtr serialIDPtr, MotionID gm);

        public static void Init()
        {
            MinHookHelper.initialize();

            _requestGmtDelegate = new ECMotionRequestGmt(ECMotion_RequestGMT_Sync);

            MinHookHelper.createHook((IntPtr)0x1415EA7A0, _requestGmtDelegate, out _requestGmtTrampoline);
            MinHookHelper.enableAllHook();
        }

        private static ECMotionRequestGmt _requestGmtDelegate;
        private static ECMotionRequestGmt _requestGmtTrampoline;
        private static void ECMotion_RequestGMT_Sync(IntPtr motionPtr, IntPtr serialIDPtr, MotionID gmt)
        {
            ECMotion motion = new ECMotion();
            motion.Pointer = motionPtr;

            if (motion.Owner.Attributes.is_player)
                if (MPManager.Connected)
                {

                    NetPacket anim = new NetPacket(false);

                    anim.Writer.Write((byte)PacketMessage.CharacterPlayGMT);
                    anim.Writer.Write((uint)gmt);

                    MPManager.SendToEveryone(anim, Steamworks.EP2PSend.k_EP2PSendReliable);
                }


            _requestGmtTrampoline(motionPtr, serialIDPtr, gmt);
        }
    }
}
