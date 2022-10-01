using System;
using DragonEngineLibrary;

namespace Brawler
{
    internal class EnemyAIUshio : EnemyAIBoss
    {
        private const TalkParamID m_ushioHact = (TalkParamID)12897;

        public override void Start()
        {
            base.Start();

            m_counterAttacks.Add(RPGSkillID.boxer_atk_b);
        }

        public override void CombatUpdate()
        {
            base.CombatUpdate();
        }

        public override void OnStartAttack()
        {
            if (!m_performedHacts.Contains(m_ushioHact))
                if (Character.IsHPBelowRatio(0.4f))
                    DoHAct(m_ushioHact);
        }
    }
}
