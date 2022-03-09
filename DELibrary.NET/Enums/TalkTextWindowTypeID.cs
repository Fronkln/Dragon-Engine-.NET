using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public enum TalkTextWindowTypeID : uint
    {
        invalid,         // constant 0x0
        normal,      // constant 0x1
        phone_down,      // constant 0x2
        phone_up,        // constant 0x3
        none,        // constant 0x4
        speech_skip,         // constant 0x5
        party_talk_l,        // constant 0x6
        party_talk_r,        // constant 0x7
        party_talk_l_large,      // constant 0x8
        party_talk_r_large,      // constant 0x9
        party_talk_l_cutin,      // constant 0xA
        party_talk_r_cutin,      // constant 0xB
        party_talk_both,         // constant 0xC
        party_talk_none,         // constant 0xD
    }
}

