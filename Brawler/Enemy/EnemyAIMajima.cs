using DragonEngineLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brawler
{
    internal class EnemyAIMajima : EnemyAIBoss
    {
        public override MotionID TauntMotion => (MotionID)17084;
        public override int EvadeAmount => 99999;

        public override void InitResources()
        {
            //gv_fighter_majima_extra
            SoundManager.LoadCuesheet(5572);
        }

        public override bool ShouldBlockAttack(BattleDamageInfo dmgInf)
        {
            return false;
        }

        public override bool DoSpecial(BattleDamageInfo inf)
        {
            if(IsBeingSpammed())
            {
                Sway();
                return true;
            }

            return false;
        }
    }
}
