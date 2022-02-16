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
            while (true)
            {
                if (DragonEngine.IsKeyDown(VirtualKey.Numpad2))
                {
                    NakamaManager.RemoveAllPartyMembers();
                }

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad1))
                {
                    FighterManager.GetPlayer().Character.GetConstructor().StopAgent();
                }
            }

        }

        public static void Update()
        {
            if (DragonEngine.IsKeyHeld(VirtualKey.Numpad3))
            {
                NPCRequestMaterial material = new NPCRequestMaterial();
                material.Material = new NPCMaterial();
                material.Material.pos_ = DragonEngine.GetHumanPlayer().Get().GetPosCenter();
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
