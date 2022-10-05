using System;
using System.Linq;
using DragonEngineLibrary;

namespace Brawler
{
 
    //Zheng's fuckbuddy at chapter 2
    internal class EnemyAIHu : EnemyAI
    {
        private TalkParamID m_downedHact = (TalkParamID)12901;
        private Fighter m_Zheng;

        public override void Start()
        {
            base.Start();

            m_Zheng = BrawlerBattleManager.Enemies.FirstOrDefault(x => x.Character.Attributes.enemy_id == BattleRPGEnemyID.yazawa_boss_tei_c03);
           
            if (m_Zheng == null)
                m_Zheng = new Fighter((IntPtr)0);
        }

        public override void CombatUpdate()
        {
            base.CombatUpdate();

            //Zheng: Alive
            //Hu: <= 35% HP & Downed
            if(m_Zheng.IsValid())
            {
                if (!m_performedHacts.Contains(m_downedHact))
                    if (Character.IsHPBelowRatio(0.35f))
                        if (Character.IsDown())
                            if(Vector3.Distance((Vector3)Chara.Get().Transform.Position, (Vector3)BrawlerBattleManager.KasugaChara.Transform.Position) <= 6.5f)
                                DoHAct(m_downedHact, m_Zheng);
            }
        }
    }
}
