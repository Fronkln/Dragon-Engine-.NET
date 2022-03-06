using System;
using DragonEngineLibrary;

namespace Y7MP
{
    /// <summary>
    /// GeneratedEnemy: Enemy created from FighterManager.GenerateEnemyFighter
    /// </summary>
    public class GeneratedEnemy : SimpleNetworkedEntity
    {
        public new Character Entity;

        //    public void Create(Vector3 pos, float angle, uint soldierID, uint chara)
        public void Create(Vector3 pos, float angle)
        {
            Entity = FighterManager.GenerateEnemyFighter(new PoseInfo(pos, angle), MPPlayer.DefaultPvpEnemyID, MPPlayer.DefaultPlayerModel);
            DragonEngine.Log("Networked GenerateEnemyFighter by host");
        }

        /// <summary>
        /// This is processed by the host.
        /// </summary>
        /// <returns></returns>
        public override bool ShouldBeRemoved()
        {
            return Entity != null && Entity.IsDead();
        }

        public override void Destroy()
        {
            base.Destroy();

            Entity.DestroyEntity();
        }
    }
}
