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
                if (DragonEngine.IsKeyHeld(VirtualKey.Numpad6))
                {
                   EntityComponent comp = FighterManager.GetPlayer().Character.EntityComponentMap.GetComponent(EntityComponent.ECSlotID.navigate);

                    if (comp.IsValid())
                        DragonEngine.Log("comp valid");

                    /*
                    TalkTextManager.TextInfo inf = new TalkTextManager.TextInfo();

                    EntityHandle<Character> chara = Character.Create(DragonEngine.GetHumanPlayer(), CharacterID.m_kiryu);
                    chara.Get().Transform.Position = DragonEngine.GetHumanPlayer().Transform.Position + DragonEngine.GetHumanPlayer().Transform.forwardDirection * 3f;

                    inf.msg_text = "turn off brawler";
                    inf.msg_talker = "suzuki";
                    inf.h_chara = chara.UID;
                    inf.skip_type = TalkTextSkipTypeID.invalid;
                    inf.wnd_type = TalkTextWindowTypeID.normal;
                    inf.play_speed = 1;
                    inf.bgm_trigger = TalkSoundBGMTriggerID.invalid;
                    inf.bgm_id = 0;
                    inf.bgm_fadein_sec = 0;
                    inf.bgm_fadeout_sec = 0;
                    inf.se_id = 0;
                    inf.oneshot_voice_id_talk = SoundCategoryLabelGVTalkID.disappoed1;
                    inf.time_next_tick = 29900;
                    inf.voicer_id = CharacterVoicerID.kasuga;
                    inf.flag_text = 0;
                    inf.face_tex_id = 0;
                    inf.color_text = 4294967295;
                    inf.lip_scale = 0;
                    inf.speech_time = 14950;
                    inf.disable_tick = 0;

                    TalkTextManager.SetText(inf);
                    */
                }

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad2))
                {
                    //NakamaManager.RemoveAllPartyMembers();

                    FighterManager.GenerateEnemyFighter(new PoseInfo(DragonEngine.GetHumanPlayer().Transform.Position, 0), 0, CharacterID.s_masumi_44);
                }

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad5))
                {
                    /*
                    HActRequestOptions opts = new HActRequestOptions();

                    opts.Init();
                    opts.id = TalkParamID.h5040_shimano_throw;
                    opts.is_force_play = false;
                    opts.can_skip = false;

                    opts.Register(HActReplaceID.hu_player1, FighterManager.GetAllEnemies()[0].Character.UID);
                    opts.Register(HActReplaceID.hu_enemy_00, FighterManager.GetAllEnemies()[1].Character.UID);

                    BattleTurnManager.RequestHActEvent(opts);
                    */
                }
            }

        }


        public override void OnModInit()
        {
            base.OnModInit();

            DragonEngine.Initialize();

            Thread thread = new Thread(InputThread);
            thread.Start();

        }
    }
}
