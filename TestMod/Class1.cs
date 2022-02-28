using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;

namespace TestMod
{
    public class Mod : DragonEngineMod
    {
        public static Character chara;

        public void InputThread()
        {
            while (true)
            {
                if (DragonEngine.IsKeyDown(VirtualKey.Numpad1))
                {
                    //NakamaManager.RemoveAllPartyMembers();

                    FighterManager.GenerateEnemyFighter(new PoseInfo(DragonEngine.GetHumanPlayer().Transform.Position, 0), 0, CharacterID.m_masato);
                }

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad2))
                {
                    //NakamaManager.RemoveAllPartyMembers();

                    FighterManager.GenerateEnemyFighter(new PoseInfo(DragonEngine.GetHumanPlayer().Transform.Position, 0), 0, CharacterID.s_masumi_44);
                }

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad4))
                {
                    NPCRequestMaterial material = new NPCRequestMaterial();
                    material.Material = new NPCMaterial();
                    material.Material.pos_ = DragonEngine.GetHumanPlayer().Transform.Position;


                    material.Material.ai_command_.pack_id_ = (AIPackID)8;
                    material.Material.ai_command_.commander_ = AIUtilCommand.Commander.debug;
                    material.Material.ai_command_.priority_ = 100;

                    material.Material.character_id_ = CharacterID.m_kiryu;
                    material.Material.is_eternal_life_ = true;
                    material.Material.is_encounter_ = true;
                    material.Material.is_encount_btl_type_ = true;
                    material.Material.soldier_data_id_ = CharacterNPCSoldierPersonalDataID.yazawa_btl15_0030_000_7;
                    material.Material.enemy_id_ = BattleRPGEnemyID.yazawa_boss_aoki;
                    material.Material.height_scale_id_ = CharacterHeightID.invalid;
                    material.Material.is_minimum_mode_ = false;
                    material.Material.is_force_create_ = true;
                    material.Material.is_force_visible_ = true;
                    material.Material.behavior_set_id_ = BehaviorSetID.encounter;
                    material.Material.voicer_id_ = CharacterVoicerID.invalid;
                    material.Material.parent_ = SceneService.CurrentScene.Get().GetSceneEntity<EntityBase>(SceneEntity.character_manager).UID;
                    material.Material.npc_setup_id_ = CharacterNPCSetup.fix_soldier;
 

                    Character chara = NPCFactory.RequestCreate(material);
                    FighterManager.RequestRegistrationFighter(chara.UID, BattleGroupID.Enemy);
                }
                if (DragonEngine.IsKeyDown(VirtualKey.Numpad5))
                {
                    HActRequestOptions opts = new HActRequestOptions();

                    opts.Init();
                    opts.id = TalkParamID.h5040_shimano_throw;
                    opts.is_force_play = false;
                    opts.can_skip = false;

                    opts.Register(HActReplaceID.hu_player1, FighterManager.GetAllEnemies()[0].Character.UID);
                    opts.Register(HActReplaceID.hu_enemy_00, FighterManager.GetAllEnemies()[1].Character.UID);

                    BattleTurnManager.RequestHActEvent(opts);
                }
            }

        }

        public static void Update()
        {
            if (DragonEngine.IsKeyHeld(VirtualKey.Numpad3))
            {
                NPCRequestMaterial material = new NPCRequestMaterial();
                material.Material = new NPCMaterial();
                material.Material.pos_ = DragonEngine.GetHumanPlayer().GetPosCenter();
                material.Material.character_id_ = CharacterID.m_dummy;
                material.Material.collision_type_ = 0;
                material.Material.is_eternal_life_ = true;
                material.Material.height_scale_id_ = CharacterHeightID.height_170;
                material.Material.is_force_create_ = true;
                material.Material.is_force_visible_ = true;
                material.Material.behavior_set_id_ = BehaviorSetID.invalid;
                material.Material.voicer_id_ = CharacterVoicerID.sonhi;
                material.Material.parent_ = DragonEngineLibrary.Service.SceneService.CurrentScene.Get().GetSceneEntity<EntityBase>(SceneEntity.character_manager).UID;
                material.Material.npc_setup_id_ = CharacterNPCSetup.no_collision_ever_fix;

                chara = NPCFactory.RequestCreate(material);

                EntityComponentHandle<ECCharacterEffectEvent> newComp = ECCharacterEffectEvent.Attach(chara);
                newComp.Get().PlayEvent(EffectEventCharaID.boss_mabuchi_lp);

            }
        }

        public override void OnModInit()
        {
            base.OnModInit();

            DragonEngine.Initialize();

            Thread thread = new Thread(InputThread);
            thread.Start();

            DragonEngine.RegisterJob(Update, DEJob.Update);
        }
    }
}
