using System;
using System.Runtime.InteropServices;
using System.Threading;
using DragonEngineLibrary;

namespace TestMod
{
    public class Mod : DragonEngineMod
    {
        public static Character chara;

        public void InputThread()
        {
            //A while(true) loop is not dangerous here because it's a seperate thread.
            //It does not block any other functions
            while (true)
            {
                if (DragonEngine.IsKeyDown(VirtualKey.Numpad2))
                {
                    DragonEngine.GetHumanPlayer().Get().Components.EffectEvent.Get().PlayEvent(EffectEventCharaID.boss_mabuchi_lp);
                }

                if(DragonEngine.IsKeyDown(VirtualKey.Numpad3))
                {
                    NPCRequestMaterial material = new NPCRequestMaterial();
                    material.Material = new NPCMaterial();
                    material.Material.pos_ = DragonEngine.GetHumanPlayer().Get().GetPosCenter();
                    material.Material.character_id_ = CharacterID.m_dummy;
                    material.Material.collision_type_ = 0;
                    material.Material.is_eternal_life_ = true;
                    material.Material.height_scale_id_ = CharacterHeightID.height_170;
                    material.Material.is_encounter_ = true;
                    material.Material.is_encount_btl_type_ = true;
                    material.Material.enemy_id_ = BattleRPGEnemyID.yazawa_boss_nanba;
                    material.Material.is_force_create_ = true;
                    material.Material.is_force_visible_ = true;
                    material.Material.behavior_set_id_ = BehaviorSetID.encounter;
                    material.Material.voicer_id_ = CharacterVoicerID.sonhi;
                    material.Material.parent_ = DragonEngineLibrary.Service.SceneService.CurrentScene.Get().GetSceneEntity<EntityBase>(SceneEntity.character_manager).UID;
                    material.Material.npc_setup_id_ = CharacterNPCSetup.no_collision_ever_fix;
                    material.Material.map_icon_id_ = MapIconID.enemy;

                   chara = NPCFactory.RequestCreate(material);

                   EntityComponentHandle<ECCharacterEffectEvent> newComp = ECCharacterEffectEvent.Attach(chara);
                   newComp.Get().PlayEvent(EffectEventCharaID.boss_mabuchi_lp);

                }
            }

        }

        public static void Update()
        {
            if(chara != null && chara.IsValid())
            {
                chara.SetAngleY(chara.GetAngleY() + 0.05f);
            }

            if (DragonEngine.IsKeyHeld(VirtualKey.Numpad5))
            {
                DragonEngine.Log("work");
                NPCRequestMaterial material = new NPCRequestMaterial();
                material.Material = new NPCMaterial();
                material.Material.pos_ = DragonEngine.GetHumanPlayer().Get().GetPosCenter();
                material.Material.character_id_ = CharacterID.m_kiryu;
                material.Material.collision_type_ = 0;
                material.Material.is_eternal_life_ = true;
                material.Material.height_scale_id_ = CharacterHeightID.height_170;
                material.Material.is_encounter_ = true;
                material.Material.is_encount_btl_type_ = true;
                material.Material.enemy_id_ = BattleRPGEnemyID.yazawa_boss_nanba;
                material.Material.is_force_create_ = true;
                material.Material.is_force_visible_ = true;
                material.Material.behavior_set_id_ = BehaviorSetID.encounter;
                material.Material.voicer_id_ = CharacterVoicerID.sonhi;
                material.Material.parent_ = DragonEngineLibrary.Service.SceneService.CurrentScene.Get().GetSceneEntity<EntityBase>(SceneEntity.character_manager).UID;
                material.Material.npc_setup_id_ = CharacterNPCSetup.no_collision_ever_fix;
                material.Material.map_icon_id_ = MapIconID.enemy;


                Character npc = NPCFactory.RequestCreate(material);

                if(npc.IsValid())
                {
                    DragonEngine.Log("Is valid");
                    FighterManager.RegistrationFighter(npc, BattleGroupID.Ally);

                    DragonEngine.Log("Is it valid? The effect event " + npc.Components.EffectEvent.UID);
                }
                else
                {
                    DragonEngine.Log("Creation went through but its not valid.");
                }


            }
        }

        public override void OnModInit()
        {
            base.OnModInit();

            DragonEngine.Initialize();

            Thread thread = new Thread(InputThread);
            thread.Start();

            DragonEngine.RegisterJob(Update, DEJob.Update, false);
        }
    }
}
