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

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private delegate bool BattleTurnManagerForceCounterCommand(IntPtr turnManager, IntPtr counterFighter, IntPtr attacker, RPGSkillID skill);

        public static void Init()
        {
            MinHookHelper.initialize();

            _requestGmtDelegate = new ECMotionRequestGmt(ECMotion_RequestGMT_Sync);
            _forceCounterCommandDelegate = new BattleTurnManagerForceCounterCommand(BattleTurnManager_ForceCounterCommand_Sync);

            MinHookHelper.createHook((IntPtr)0x1415EA7A0, _requestGmtDelegate, out _requestGmtTrampoline);
            MinHookHelper.createHook((IntPtr)0x1404C5D20, _forceCounterCommandDelegate, out _forceCounterCommandTrampoline);
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

        private static BattleTurnManagerForceCounterCommand _forceCounterCommandDelegate;
        private static BattleTurnManagerForceCounterCommand _forceCounterCommandTrampoline;
        private static bool BattleTurnManager_ForceCounterCommand_Sync(IntPtr turnManager, IntPtr counterFighter, IntPtr attacker, RPGSkillID skill)
        {
            if (!MPManager.Connected || counterFighter == IntPtr.Zero || attacker == IntPtr.Zero)
                return _forceCounterCommandTrampoline(turnManager, counterFighter, attacker, skill);

            Fighter counterFighterObj = new Fighter(counterFighter);
            Fighter attackerObj = new Fighter(attacker);

            MPPlayer victim = MPManager.GetPlayerForCharacter(attackerObj.Character);

            //Only sync counter commands against players
            if (victim == null)
                return _forceCounterCommandTrampoline(turnManager, counterFighter, attacker, skill);

            if (counterFighterObj.Character.UID == DragonEngine.GetHumanPlayer().UID)
            {
                NetPacket counterPacket = new NetPacket(false);
                counterPacket.Writer.Write((byte)PacketMessage.TURNBASED_ForceCounterCommand);

                counterPacket.Writer.Write(MPPlayer.LocalPlayer.Owner.m_SteamID);
                counterPacket.Writer.Write(victim.Owner.m_SteamID);
                counterPacket.Writer.Write((uint)skill);

                MPManager.SendToEveryone(counterPacket, Steamworks.EP2PSend.k_EP2PSendReliable);
            }

            return _forceCounterCommandTrampoline(turnManager, counterFighter, attacker, skill);
        }
    }
}
