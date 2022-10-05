using System;
using System.Linq;
using DragonEngineLibrary;

namespace Brawler
{
    internal class EnemyAISaejima : EnemyAIBoss
    {

        private Fighter Kyodai;

        //Majima exists
        //Saejima grabbed Ichiban
        private EnemyMoveSync m_tagTeamSaejimaGrab = new EnemyMoveSync();

        public bool IsKyodaiValid()
        {
            return Kyodai.IsValid() && !Kyodai.IsDead();
        }

        public override void Start()
        {
            base.Start();

            m_tagTeamSaejimaGrab = new EnemyMoveSync()
            {
                Sync = (RPGSkillID)193,
                GrabbedAnim = (MotionID)2168,
                OnHitSuccess = delegate { DoHAct((TalkParamID)12898, Kyodai); }
            };

            Kyodai = BrawlerBattleManager.Enemies.FirstOrDefault(x => EnemyManager.EnemyAIs[x.Character.UID] is EnemyAIMajima);

            if (Kyodai == null)
                Kyodai = new Fighter((IntPtr)0);

            if (Kyodai != null)
                Console.WriteLine("found my bro!");
        }

        public override void OnStartAttack()
        {
            base.OnStartAttack();

            SyncHActModule.StartProcedure(Character, m_tagTeamSaejimaGrab);
        }
    }
}
