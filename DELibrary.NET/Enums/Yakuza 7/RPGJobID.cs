using System;

namespace DragonEngineLibrary
{
    public enum RPGJobID : uint
    {
        invalid,         // constant 0x0
        kasuga_freeter,      // constant 0x1
        kasuga_braver,       // constant 0x2
        nanba_01,        // constant 0x3
        adachi_01,       // constant 0x4
        saeko_01,        // constant 0x5
        jyungi_01,       // constant 0x6
        chou_01,         // constant 0x7
        woman_a_01,      // constant 0x8
        man_01,      // constant 0x9
        man_02,      // constant 0xA
        man_kaitaiya,        // constant 0xB
        man_musician,        // constant 0xC
        man_05,      // constant 0xD
        man_06,      // constant 0xE
        man_07,      // constant 0xF
        man_08,      // constant 0x10
        woman_idol,      // constant 0x11
        woman_02,        // constant 0x12
        woman_dealer,        // constant 0x13
        woman_nightqueen,        // constant 0x14
        dlc_01,      // constant 0x15
        woman_martial,       // constant 0x16
#if IW_AND_UP
        kiryu_01 = 23,
        woman_kunoichi,
        man_samurai,
        man_actionstar,
        man_marine,
        man_footballer,
        man_western,
        man_firedancer,
        woman_housekeeper,
        woman_tropicaldancer,
        woman_tennis,
        chitose_01,
        tomizawa_01,
        sonhi_01,
        kasuga_sujimon
#endif
    };
}
