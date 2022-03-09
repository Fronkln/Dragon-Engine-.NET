using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class TalkTextManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CTALK_TEXT_MAN_SHOW_WINDOW", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_TalkTextManager_DisplayWindow(bool is_show, bool is_talker_show);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CTALK_TEXT_MAN_SET_WINDOW_TYPE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_TalkTextManager_SetWindowType(TalkTextWindowTypeID id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CTALK_TEXT_MAN_SET_TEXT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_TalkTextManager_SetText(IntPtr textInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CTALK_TEXT_MAN_TEST", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_TalkTextManager_Test();


        [StructLayout(LayoutKind.Sequential)]
        public struct TextInfo
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string msg_text;
            [MarshalAs(UnmanagedType.LPStr)]
            public string msg_talker;
            //EntityHandle character
            public uint h_chara;
            public TalkTextSkipTypeID skip_type;
            public TalkTextWindowTypeID wnd_type;
            public float play_speed;
            public TalkSoundBGMTriggerID bgm_trigger;
            public uint bgm_id;
            public float bgm_fadein_sec;
            public float bgm_fadeout_sec;
            public uint se_id;
            public SoundCategoryLabelGVTalkID oneshot_voice_id_talk;
            public SoundCategoryLabelGVCmnID oneshot_voice_id_cmn;
            public uint time_next_tick;
            public CharacterVoicerID voicer_id;
            public uint flag_text;
            public UITextureID face_tex_id;
            public uint color_text;
            public float lip_scale;
            public uint speech_time;
            public uint disable_tick;
            public uint h_auth_play_base;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 4)]
            public byte[] pv_elm_node_guid;
        };

        public static void Test()
        {
            TextInfo inf = new TextInfo();

            EntityHandle<Character> chara = Character.Create(DragonEngine.GetHumanPlayer(), CharacterID.m_kiryu); 

            inf.msg_text = "Yakuza 7 works better as a turn based game.";
            inf.msg_talker = "Ichiban 'Facts' Kasuga";
            inf.h_chara = chara.UID;
            inf.skip_type = TalkTextSkipTypeID.invalid;
            inf.wnd_type = TalkTextWindowTypeID.normal;
            inf.play_speed = 1;
            inf.bgm_trigger = TalkSoundBGMTriggerID.invalid;
            inf.bgm_id = 0;
            inf.bgm_fadein_sec = 0;
            inf.bgm_fadeout_sec = 0;
            inf.se_id = 0;
            inf.oneshot_voice_id_talk = SoundCategoryLabelGVTalkID.laugh1;
            inf.time_next_tick = 29900;
            inf.voicer_id = CharacterVoicerID.kasuga;
            inf.flag_text = 0;
            inf.face_tex_id = (UITextureID)0;
            inf.color_text = 4294967295;
            inf.lip_scale = 0;
            inf.speech_time = 14950;
            inf.disable_tick = 0;
        }

        public static void DisplayWindow(bool show, bool talkerShow)
        {
            DELibrary_TalkTextManager_DisplayWindow(show, talkerShow);
        }

        public static void SetWindowType(TalkTextWindowTypeID id)
        {
            DELibrary_TalkTextManager_SetWindowType(id);
        }

        public static void SetText(TextInfo inf)
        {
            IntPtr infPtr = inf.ToIntPtr();
            DELibrary_TalkTextManager_SetText(infPtr);
        }
    }
}
