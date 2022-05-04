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
        public static Character chara = new Character();

        public void InputThread()
        {
            while (true)
            {
                if (DragonEngine.IsKeyHeld(VirtualKey.Numpad6))
                {
                   EntityComponent comp = FighterManager.GetPlayer().Character.EntityComponentMap.GetComponent(ECSlotID.navigate);

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
                    DragonEngine.Log("P1: " + ParticleManager.IsLoadedRaw(ParticleID.AAa0077));
                    DragonEngine.Log("P2: " + ParticleManager.IsLoadedRaw(ParticleID.AAa0078));
                    DragonEngine.Log("P3: " + ParticleManager.IsLoadedRaw(ParticleID.AMs0001));


                    DragonEngine.Log("play");

                    ParticleManager.Play(ParticleID.AAa0077, DragonEngine.GetHumanPlayer().GetPosture().GetRootMatrix(), ParticleType.Stage);
                    ParticleManager.Play(ParticleID.AAa0078, DragonEngine.GetHumanPlayer().GetPosture().GetRootMatrix(), ParticleType.Stage);


                    DragonEngine.Log("played");
                }
            }

        }




        public static MotionPlayInfo Copy(MotionPlayInfo to, MotionPlayInfo from)
        {
            to.vfptr = from.vfptr;
            to.jaunt_goal_ = from.jaunt_goal_;
            to.blend_space_param_ = from.blend_space_param_;
            to.behavior_id_ = from.behavior_id_;
            to.behavior_id_next_ = from.behavior_id_next_;
            to.bep_handle_ = from.bep_handle_;
            to.gmt_id_ = from.gmt_id_;
            to.tick_gmt_now_ = from.tick_gmt_now_;
            to.tick_now_ = from.tick_now_;
            to.tick_old_ = from.tick_old_;
            to.tick_blend_ = from.tick_blend_;
            to.tick_key_ = from.tick_key_;
            to.no_blend_ = from.no_blend_;
            to.ignore_trans_ = from.ignore_trans_;

            return to;
        }
        
        public static void Update()
        {
            MotionPlayInfo inf1 = Copy(new MotionPlayInfo(), DragonEngine.GetHumanPlayer().GetMotion().PlayInfo);
            MotionPlayInfo inf2 = Copy(new MotionPlayInfo(), DragonEngine.GetHumanPlayer().GetMotion().BhvPartsInfo);


            if(chara.IsValid())
            {
                chara.GetMotion().PlayInfo = inf1;
                chara.GetMotion().BhvPartsInfo = inf2;
                chara.Transform.Position = DragonEngine.GetHumanPlayer().Transform.Position;
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
