using System;


namespace DragonEngineLibrary
{

#if IW_AND_UP
    public enum BattleControlType : uint
    {
        invalid,         // constant 0x0
        player = 1,
        zako = 2,
        chinpira = 3,
        gorotuki = 4,
        zako_kenka = 5,
        zako_guard = 6,
        zako_tackle = 7,
        infighter = 8,
        kakutou = 9,
        karate = 10,
        boxer = 11,
        kick_boxer = 12,
        jaka = 13,
        chuka_lea = 14,
        chuka = 15,
        zako_yopparai = 16,
        power = 17,
        fat = 18,
        sbc = 19,
        dorobou = 20,
        zako_throw = 21,
        zako_machinegun = 22,
        zako_wpt = 23,
        zako_wpi = 24,
        zako_wpj = 25,
        ninja_wpe = 26,
        naguttemiro = 27,
        bourei_wpe = 28,
        mosa_rikiya = 29,
        mosa_daigo = 30,
        mosa_karate = 31,
        mosa_kick = 32,
        mosa_wrestler = 33,
        mosa_mask = 34,
        mosa_yahata = 35,
        mosa_kuze = 36,
        mosa_sougou = 37,
        mosa_lee = 38,
        mosa_kanda = 39,
        mosa_sbs = 40,
        mosa_cook = 41,
        mosa_katana = 42,
        mosa_wpd = 43,
        mosa_gun = 44,
        mosa_snd = 45,
        mosa_wpg = 46,
        npc_kaito = 47,
        npc_sugiura = 48,
        ynk = 49,
        zako2 = 50,
        zako3 = 51,
        dekunobou = 52,
        boss_rugby_a = 53,
        boss_rugby_b = 54,
        npc_higashi = 55,
        mg_boxing_zako = 56,
        issen_kenpo = 57,
        hagoku_kenpo = 58,
        enbu_kenpo = 59,
        ninja = 60,
        boss_street = 61,
        boss_street_spd = 62,
        mg_boxing_todoroki = 63,
        boss_tesso_claw = 64,
        mg_boxing_sakuma = 65,
        mg_boxing_in_fighter = 66,
        mg_boxing_out_boxer = 67,
        boss_bad_boy = 68,
        boss_kuwana = 69,
        boss_souma = 70,
        boss_akutsu = 71,
        boss_watanabe = 72,
        boss_rk_a = 73,
        npc_kuwana = 74,
        boss_rk_d = 75,
        boss_rk_b = 76,
        boss_rk_c = 77,
        boss_rk_e = 78,
        mosa = 79,
        mg_boxing_ban_nou = 80,
        mg_boxing_tyson = 81,
        mosa_grow_wpb = 82,
        mg_boxing_rns = 83,
        wizard = 84,
        tiger = 85,
        boss_oshikiri = 86,
        boss_kasai = 87,
        boss_honda = 88,
        boss_sakakiba = 89,
        boss_koga = 90,
        boss_amon = 91,
        npc_vr_yagami = 92,
        mosa_wpc = 93,
        npc_oshikiri = 94,
        kaito = 95,
        sugiura = 96,
        higashi = 97,
        npc_man = 98,
        npc_woman = 99,
        kasuga = 100,
        nanba = 101,
        adachi = 102,
        saeko = 103,
        chou = 104,
        jyungi = 105,
        woman_a = 106,
        kiryu = 107,
        salaryman = 108,
        drunk = 109,
        homeless = 110,
        host = 111,
        grappler = 112,
        tribal = 113,
        gang = 114,
        gang_leader = 115,
        tepoudama = 116,
        yakuza_touzyo = 117,
        yakuza_oumi = 118,
        yakuza = 119,
        yakuza_knife = 120,
        yakuza_dosu = 121,
        yakuza_bat = 122,
        yakuza_katana = 123,
        yakuza_pistol = 124,
        yakuza_handgun = 125,
        shokunin = 126,
        thief = 127,
        cmafia = 128,
        kempo = 129,
        chef = 130,
        performer = 131,
        tourima = 132,
        doctor = 133,
        shinkyu = 134,
        big_eater = 135,
        hitman = 136,
        soldier = 137,
        bousou = 138,
        bouncer = 139,
        hacker = 140,
        dealer = 141,
        gambler = 142,
        rapper = 143,
        scout_man = 144,
        pyro = 145,
        criminal = 146,
        fanatic = 147,
        fanatic_b = 148,
        reinou = 149,
        guardman = 150,
        baseball_fan = 151,
        ballplayer = 152,
        cleaner = 153,
        press = 154,
        skater = 155,
        hippie = 156,
        sandwich_man = 157,
        bcj = 158,
        hecaton = 159,
        regent = 160,
        lotion_man = 161,
        zombie = 162,
        rosyutsu = 163,
        arashi = 164,
        bannin = 165,
        slugger = 166,
        inventor = 167,
        paripi = 168,
        otaku = 169,
        otaku_b = 170,
        scissors = 171,
        wp_master = 172,
        umibouzu = 173,
        gomi_fukuro = 174,
        musician = 175,
        straw_doll = 176,
        boss_sawashiro = 177,
        boss_sawashiro_brt = 178,
        boss_sawashiro_b = 179,
        boss_sawashiro_e = 180,
        boss_sawashiro_x = 181,
        boss_clean_robot = 182,
        boss_tiger = 183,
        boss_kuma = 184,
        boss_crane_truck = 185,
        boss_power_shovel = 186,
        boss_matoba_e = 187,
        boss_healer = 188,
        boss_tank = 189,
        boss_attacker = 190,
        boss_mabuchi_i = 191,
        boss_ishioda = 192,
        boss_kiryu = 193,
        boss_kiryu_crash = 194,
        boss_kiryu_rush = 195,
        boss_kiryu_legend = 196,
        boss_saejima = 197,
        boss_majima_b = 198,
        boss_mirror_face = 199,
        boss_tendo = 200,
        boss_arakawa = 201,
        boss_arakawa_y = 202,
        sniper = 203,
        boss_jyungi = 204,
        boss_nanba = 205,
        mazooji = 206,
        furamacho = 207,
        land_surfer = 208,
        akutoku_collector = 209,
        ayashi_sharman = 210,
        back_meat_shop = 211,
        big_head = 212,
        calorie_knight = 213,
        dundee_coat = 214,
        spray_gang = 215,
        sun_oil_man = 216,
        ukulele_man = 217,
        beach_ball_man = 218,
        hawaii_paripi = 219,
        kantigai_narikin = 220,
        trouble_boxer = 221,
        fake_cop = 222,
        cosplayer = 223,
        ayashi_seibishi = 224,
        yasagure_niwashi = 225,
        city_worm = 226,
        ungra_fighter = 227,
        homeless_gari = 228,
        ayashi_hanaya = 229,
        kakyo_mafia = 230,
        nagare_kung_fu = 231,
        ayashi_ballerino = 232,
        hori_gurui = 233,
        kaibou_gurui = 234,
        kyoujin = 235,
        aloha_gang = 236,
        hawaiian_noir = 237,
        haikai_aruchu = 238,
        katuage_performer = 239,
        yasi_man = 240,
        dig_man = 241,
        big_sumou = 242,
        leg_long_pierrot = 243,
        salesman = 244,
        stray_dog = 245,
        board_man = 246,
        vacance_muscle = 247,
        smoking_dead = 248,
        resura_kuzure = 249,
        mituryo = 250,
        boss_kraken = 251,
        boss_kraken_left = 252,
        boss_kraken_right = 253,
        yusuriya = 254,
        bad_boys = 255,
        huhou_touki = 256,
        gaityu_kuzyo = 257,
        ayashi_kikori = 258,
        punk = 259,
        boss_dwight_x = 260,
        e_special_shark = 261,
        e_boss_wong = 262,
        boss_wong = 263,
        boss_yamai = 264,
        boss_yamai_h = 265,
        kimen = 266,
        boss_kraken_sumi = 267,
        boss_ufo = 268,
        palekana_warrior = 269,
        npc_eiji = 270,
        boss_big_shark = 271,
        boss_daigo = 272,
        boss_narasaki = 273,
        boss_ebina = 274,
        boss_ebina_e = 275,
        boss_yamai_honki = 276,
        player_kiryu = 277,
        boss_bluthe = 278,
        boss_bluthe_e = 279,
        special_bluthe = 280,
        boss_roman = 281,
        palekana_warrior_r = 282,
        boss_asakura = 283,
        delihelp_nathan = 284,
        delihelp_gonda = 285,
        delihelp_kashiwa = 286,
        special_hostess = 287,
        chitose = 288,
        sonhi = 289,
        tomizawa = 290,
        delihelp_charlie = 291,
        delihelp_c_buster = 292,
        delihelp_pokecer = 293,
        delihelp_mame = 294,
        boss_lau = 295,
        gas_generator = 296,
        ufo_charger = 297,
        boss_komaki = 298,
        barracuda_grn = 299,
        boss_amon_01 = 300,
        boss_amon_02 = 301,
        boss_amon_03 = 302,
    };
#else
    public enum BattleControlType : uint
    {
        invalid,         // constant 0x0
        player,      // constant 0x1
        zako,        // constant 0x2
        chinpira,        // constant 0x3
        gomi_fukuro,         // constant 0x4
        clean_robot,         // constant 0x5
        salaryman,       // constant 0x6
        ally_npca,       // constant 0x7
        ally_npcb,       // constant 0x8
        ally_npcc,       // constant 0x9
        ally_ksg,        // constant 0xA
        tepoudama,       // constant 0xB
        slugger,         // constant 0xC
        grappler,        // constant 0xD
        reinou,      // constant 0xE
        ally_kasuga,         // constant 0xF
        ally_ayaka,      // constant 0x10
        ally_nanba,      // constant 0x11
        ally_adachi,         // constant 0x12
        ally_saeko,      // constant 0x13
        npc_man,         // constant 0x14
        npc_woman,       // constant 0x15
        tourima,         // constant 0x16
        rapper,      // constant 0x17
        cleaner,         // constant 0x18
        thief,       // constant 0x19
        sawashiro,       // constant 0x1A
        sawashiro_b,         // constant 0x1B
        sawashiro_x,         // constant 0x1C
        performer,       // constant 0x1D
        homeless,        // constant 0x1E
        shokunin,        // constant 0x1F
        drunk,       // constant 0x20
        shinkyu,         // constant 0x21
        cmafia,      // constant 0x22
        chef,        // constant 0x23
        musician,        // constant 0x24
        kasuga,      // constant 0x25
        nanba,       // constant 0x26
        adachi,      // constant 0x27
        ayaka,       // constant 0x28
        saeko,       // constant 0x29
        chou,        // constant 0x2A
        jyungi,      // constant 0x2B
        woman_a,         // constant 0x2C
        big_eater,       // constant 0x2D
        hitman,      // constant 0x2E
        soldier,         // constant 0x2F
        bouncer,         // constant 0x30
        guardman,        // constant 0x31
        rosyutsu,        // constant 0x32
        doctor,      // constant 0x33
        hacker,      // constant 0x34
        pyro,        // constant 0x35
        boxer,       // constant 0x36
        press,       // constant 0x37
        skater,      // constant 0x38
        kempo,       // constant 0x39
        hippie,      // constant 0x3A
        gambler,         // constant 0x3B
        lotion_man,      // constant 0x3C
        sandwich_man,        // constant 0x3D
        hecaton,         // constant 0x3E
        bannin,      // constant 0x3F
        otaku,       // constant 0x40
        regent,      // constant 0x41
        baseball_fan,        // constant 0x42
        scout_man,       // constant 0x43
        tribal,      // constant 0x44
        host,        // constant 0x45
        gang,        // constant 0x46
        gang_leader,         // constant 0x47
        yakuza_yakuza,       // constant 0x48
        yakuza_katana,       // constant 0x49
        yakuza_pistol,       // constant 0x4A
        yakuza_handgun,      // constant 0x4B
        yakuza_dosu,         // constant 0x4C
        yakuza_knife,        // constant 0x4D
        yakuza_bat,      // constant 0x4E
        yakuza_yakuza_oumi,      // constant 0x4F
        yakuza_yakuza_touzyo,        // constant 0x50
        arashi,      // constant 0x51
        paripi,      // constant 0x52
        umibouzu,        // constant 0x53
        wp_master,       // constant 0x54
        inventor,        // constant 0x55
        zombie,      // constant 0x56
        scissors,        // constant 0x57
        straw_doll,      // constant 0x58
        fanatic,         // constant 0x59
        ballplayer,      // constant 0x5A
        criminal,        // constant 0x5B
        dealer,      // constant 0x5C
        yakuza_oumi,         // constant 0x5D
        yakuza_touzyo,       // constant 0x5E
        yakuza,      // constant 0x5F
        tiger,       // constant 0x60
        crane_truck,         // constant 0x61
        power_shovel,        // constant 0x62
        matoba_e,        // constant 0x63
        boss_sawashiro,      // constant 0x64
        boss_sawashiro_b,        // constant 0x65
        boss_sawashiro_x,        // constant 0x66
        boss_clean_robot,        // constant 0x67
        boss_tiger,      // constant 0x68
        boss_crane_truck,        // constant 0x69
        boss_power_shovel,       // constant 0x6A
        boss_matoba_e,       // constant 0x6B
        boss_healer,         // constant 0x6C
        boss_sawashiro_e,        // constant 0x6D
        boss_mabuchi_i,      // constant 0x6E
        bcj,         // constant 0x6F
        bousou,      // constant 0x70
        boss_sawashiro_brt,      // constant 0x71
        boss_attacker,       // constant 0x72
        boss_ishioda,        // constant 0x73
        boss_kiryu_rush,         // constant 0x74
        boss_saejima,        // constant 0x75
        boss_kiryu_legend,       // constant 0x76
        boss_majima_b,       // constant 0x77
        boss_tank,       // constant 0x78
        boss_kiryu,      // constant 0x79
        boss_kiryu_crash,        // constant 0x7A
        boss_mirror_face,        // constant 0x7B
        boss_tendo,      // constant 0x7C
        boss_arakawa,        // constant 0x7D
        boss_arakawa_y,      // constant 0x7E
        sniper,      // constant 0x7F
        boss_kuma,       // constant 0x80
        otaku_b,         // constant 0x81
        boss_jyungi,         // constant 0x82
        boss_nanba,      // constant 0x83
        mazooji,         // constant 0x84
        fanatic_b,       // constant 0x85
        boss_amon,       // constant 0x86
        num,         // constant 0x87
    };
#endif
}