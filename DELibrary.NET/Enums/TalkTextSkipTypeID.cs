using System;

namespace DragonEngineLibrary
{
    public enum TalkTextSkipTypeID : uint
    {
        invalid,         // constant 0x0
        not_skip,        // constant 0x1
        not_skip_and_auto_next,      // constant 0x2
        not_next,    // constant 0x3
        not_skip_and_not_next,       // constant 0x4
        not_skip_and_time_next,      // constant 0x5
        speech_text_skip,        // constant 0x6
        speech_text_skip_wait,       // constant 0x7
        speech_text_skip_disable,        // constant 0x8
        time_next,   // constant 0x9
        normal,	 // constant 0xA
    }
}
