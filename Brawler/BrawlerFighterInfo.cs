using System.Collections.Generic;
using DragonEngineLibrary;

namespace Brawler
{
    public struct BrawlerFighterInfo
    {
        public Fighter Fighter;

        public static Dictionary<uint, BrawlerFighterInfo> Infos = new Dictionary<uint, BrawlerFighterInfo>();

        public bool IsDead;
        public bool IsSync;
        public bool IsDown;
        public bool IsGettingUp;
        public bool IsRagdoll;
        public bool IsMove;

        //Purpose: Cache fighter variables
        //Reduces PInvoke(probably) and eliminates several crashes
        //Related to accesing those vars in input loop
        public void Update(Fighter fighter)
        {
            Fighter = fighter;

            if (fighter == null || !fighter.Character.IsValid())
            {
                Infos.Remove(fighter.Character.UID);
                return;
            }

            BattleFighterInfo inf = fighter.GetInfo();

            IsDead = fighter.IsDead();
            IsSync = fighter.IsSync();
            IsDown = fighter.IsDown();
            IsGettingUp = fighter.Character.HumanModeManager.IsStandup();
            IsRagdoll = inf.is_ragdoll_;//fighter.Character.IsRagdoll();
            IsMove = fighter.Character.HumanModeManager.IsMove();

            Infos[fighter.Character.UID] = this;
        }
    }
}
