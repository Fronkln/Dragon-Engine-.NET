using DragonEngineLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brawler
{
    //Recent AI changes broke majimas shitton of sidestepping code
    //Reimplement!
    internal class EnemyAIMajima : EnemyAIBoss
    {
        public override MotionID TauntMotion => (MotionID)17084;

        public override void InitResources()
        {
            //gv_fighter_majima_extra
            SoundManager.LoadCuesheet(5572);
        }

        /*
        public override bool ShouldBlockAttack(BattleDamageInfoSafe dmgInf)
        {
            return false;
        }
        */
        public override bool DoSpecial(BattleDamageInfoSafe inf)
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
