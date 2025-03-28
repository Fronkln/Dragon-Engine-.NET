﻿using System;

namespace DragonEngineLibrary
{
    public enum EUIDKind : ushort
    {
#if IW
        entity_base = 1,
        scene_base = 2,
        scene_action_first_tenkaichi = 4,
        scene_action_first_nakamichi = 5,
        scene_action_first_building = 6,
        scene_action_pjm_movie = 7,
        scene_action_first_tenkaichi_many = 8,
        scene_startup_commandline = 9,
        scene_selector = 43,
        scene_commandline_impl = 44,
        camera_base = 45,
        camera_free = 46,
        camera_auth = 47,
        camera_title = 48,
        camera_test_atsu01 = 54,
        stage = 55,
        wind = 56,
        human_jaunt = 60,
        collision = 61,
        navigation_mesh = 62,
        effect_manager = 63,
        effect_base = 64,
        effect_glass = 65,
        effect_vollight = 67,
        ui_fade_manager = 69,
        ui_nowloading = 70,
        light_manager = 71,
        light_reference = 72,
        light_ev = 73,
        light_point = 74,
        light_spot = 75,
        light_tube = 76,
        light_ambient = 77,
        light_directional = 78,
        asset_manager = 79,
        asset_unit = 80,
        auth_manager = 85,
        auth_play = 86,
        auth_node_base = 87,
        auth_node_path = 88,
        auth_node_camera = 89,
        invisible_talk = 96,
        invisible_base = 99,
        ccc_manager = 121,
        scene_action_second_test_scenario = 122,
        scene = 126,
        light_color = 127,
        auth_node_camera_motion = 137,
        entity_effect_splash_liquid = 140,
        street_map = 149,
        street_range_base = 150,
        street_range_rectangle = 151,
        street_obstacle_base = 153,
        street_obstacle_circle = 154,
        street_obstacle_rectangle = 155,
        street_range_trapezoid = 159,
        street_range_quadrangle = 161,
        entity_effect_flow_dust = 162,
        physics_manager = 167,
        effect_glass_manager = 169,
        effect_vollight_manager = 170,
        auth_node_stage = 173,
        particle_manager = 174,
        particle_resource = 175,
        particle_instance = 176,
        scene_auth_preview = 178,
        scene_auth_test = 179,
        scene_auth_stage_change = 180,
        entity_effect_glass_water = 186,
        post_effect_dof = 192,
        post_effect_glare = 193,
        post_effect_tonemap = 194,
        post_effect_filmgrain = 195,
        post_effect_color_correction = 196,
        auth_node_element = 201,
        particle_instance_set = 202,
        particle_interface = 203,
        entity_render_target_effect_work = 204,
        character_base = 211,
        character_manager = 212,
        character = 213,
        entity_effect_sea = 215,
        movie_tex_manager = 221,
        team_manager = 225,
        scene_action_battle_validation = 227,
        battle_manager = 228,
        light_pcc = 230,
        camera_pause_menu = 232,
        post_effect_manager = 243,
        scene_action_battle_validation_only_btl = 245,
        sound_manager = 252,
        behavior_property_play = 262,
        behavior_property_manager = 263,
        dispose_folder = 267,
        dispose_workgroup = 268,
        motion_manager = 269,
        light_tube2 = 271,
        instant_chat_manager = 275,
        ui_manager = 276,
        post_effect_motion_blur = 278,
        auth_node_character = 280,
        auth_node_character_motion = 281,
        auth_node_character_behavior = 282,
        auth_node_character_node = 283,
        curtain_range_manager = 290,
        curtain_range = 291,
        game_var_manager = 295,
        post_effect_feedback = 297,
        post_effect_chromatic_aberration = 298,
        post_effect_vignette = 299,
        character_model_check = 301,
        particle_dispose = 303,
        fade_manager = 304,
        auth_play_base = 305,
        light_sh_orbox = 311,
        range_manager = 316,
        range_base = 318,
        timer_entity = 333,
        counter_entity = 334,
        auth_node_model = 335,
        auth_node_asset = 336,
        auth_node_path_motion = 337,
        auth_node_asset_motion = 338,
        entity_effect_hit_wound = 339,
        asset_scrap_group = 340,
        asset_scrap = 341,
        asset_base_common = 342,
        range = 343,
        simple_texture_entity = 344,
        sea_dispose = 347,
        scene_main = 348,
        reaction_entity_base = 350,
        auth_node_motion_base = 351,
        light_dark_orbox = 358,
        light_dark_point = 359,
        light_dark_spot = 360,
        light_dark_tube = 361,
        simple_ui_entity = 362,
        light_hemi_orbox = 365,
        entity_effect_sky_cloud = 367,
        stage_area_range = 369,
        stage_area_range_bg = 371,
        effect_track = 373,
        asset_geometry = 374,
        wall = 376,
        ui_entity_base = 377,
        effect_track_human = 380,
        effect_track_asset = 381,
        entity_effect_confetti = 382,
        character_dispose_manager = 383,
        auth_node_folder_condition = 388,
        scene_title = 389,
        scene_shop = 390,
        wall_plate = 392,
        wall_capsule = 393,
        route_path_manager = 395,
        route_path = 396,
        sound_sphere = 397,
        ui_entity_player_gauge = 398,
        rumble_manager = 400,
        sound_amb_change_range = 401,
        scene_continue = 403,
        post_effect_ldr_color_correction = 404,
        save_data_manager = 407,
        door_range_manager = 408,
        street_obstacle_manager = 411,
        lua_ui_handle_entity = 412,
        lua_ui_flow_entity = 413,
        encount_manager = 414,
        color_correction_range_manager = 422,
        inhibition_updater = 423,
        scene_info_handover = 427,
        dispose_manager = 428,
        pause_manager = 431,
        lod_manager = 432,
        auth_node_asset_node = 434,
        objective_entity = 437,
        objective_manager_entity = 438,
        player_action_entity = 439,
        effect_event_manager = 441,
        movie_tex_dispose = 444,
        scene_fade_ctrl = 445,
        chara_generate_manager = 446,
        chara_generate = 447,
        volume_preset_range = 449,
        ai_manager = 450,
        msg_window_manager = 453,
        wall_effect = 455,
        scene_clean_up_bridge = 456,
        chase_manager = 457,
        dead_effect = 458,
        ui_entity_enemy_gauge_manager = 461,
        scene_ex_status_callback_ctrl = 463,
        effect_event_chara = 464,
        map_icon_entity = 465,
        entity_sweeper = 466,
        tutorial_show = 467,
        navi_mesh_loader = 468,
        tips_manager_entity = 469,
        draw_suspend = 477,
        mission_area_manager = 478,
        mission_area = 479,
        entity_effect_sky_star = 481,
        timeline_manager = 482,
        mission_manager = 483,
        mission_base = 484,
        sky_star_dispose = 485,
        callback_db_holder = 486,
        callback_bool_void_entity = 487,
        callback_void_void_entity = 488,
        auth_node_animatable_base = 489,
        asset_preload = 493,
        uniq_dispose_manager = 495,
        wall_triangles = 497,
        sky_cloud_dispose = 498,
        wall_base = 499,
        wall_quadrangles = 502,
        tww_manager_entity = 503,
        traffic_signal_manager = 504,
        scene_minigame = 507,
        mirror_rect = 509,
        effect_mirror_manager = 510,
        camera_mirror = 511,
        navimesh_collision_manager = 512,
        water_manager = 513,
        scene_minigame_batting_center = 514,
        encount_battle = 517,
        clock_pkg_holder = 518,
        ladder = 523,
        npc_tag_manager = 524,
        scene_action_alpha_test_area = 528,
        navimesh_manager = 529,
        camera_photo = 530,
        general_car_manager = 532,
        general_car_road = 533,
        ladder_manager = 536,
        scene_minigame_darts = 541,
        mg_darts_manager = 542,
        effect_draw_mask_manager = 543,
        lua_ui_caption_entity = 545,
        player_point_entity = 550,
        scene_minigame_mahjong = 552,
        minigame_mahjong = 553,
        mahjong_camera = 554,
        effect_lens_flare = 555,
        wall_manager = 556,
        popup_manager_entity = 557,
        npc_control_manager = 560,
        npc_control = 561,
        wind_range = 564,
        foreground_texture_entity = 566,
        scene_minigame_karaoke = 567,
        mg_karaoke_manager = 568,
        foreground_ui_texture_que = 570,
        sound_dispose_entity_base = 573,
        sound_amb_volume_range = 574,
        sound_line_source = 575,
        general_car_intersection_manager = 578,
        general_car_intersection = 579,
        player_pause_entity = 580,
        sea_cap_dispose = 581,
        map_minimap_entity = 582,
        character_picture = 585,
        scene_empty = 587,
        street_map_area_manager = 594,
        street_map_area = 595,
        effect_only_one_manager = 597,
        item_dispose = 599,
        player_activity_entity = 600,
        camera_pip = 603,
        effect_draw_pip = 604,
        vending_machine = 606,
        manual_entity = 608,
        player_ability_entity = 611,
        camera_mg_darts = 615,
        simple_movie_entity = 623,
        effect_discoball = 627,
        camera_fps = 629,
        pause_mail_manager_entity = 630,
        pause_sns_manager_entity = 631,
        scene_vf5fs = 632,
        map_render_entity = 633,
        tww_notify_entity = 635,
        item_get_manager_entity = 637,
        camera_minigame_karaoke = 646,
        player_qte_entity = 649,
        shop_destory_manager = 650,
        effect_mission_area = 661,
        minigame_hitting_controller = 667,
        load_position_entity = 669,
        pause_id_manager = 670,
        player_control_manager = 671,
        player_control = 672,
        scene_cache_restore = 673,
        result_result_entity = 674,
        minigame_segaages = 675,
        minigame_batting_center_manager = 676,
        player_carry_item_entity = 677,
        invisible_allstage = 678,
        time_updater = 679,
        camera_cat_cafe = 680,
        play_go_message = 681,
        scene_minigame_switching_exe = 684,
        stage_area_monitoring = 688,
        title_entity = 690,
        driftage = 696,
        random_point_manager = 697,
        random_point = 698,
        battle_tutorial = 699,
        callback_bool_uint_ref_entity = 701,
        reward_manager = 702,
        camera_selfie = 703,
        battle_preload_entity = 704,
        scene_timer_setting = 705,
        taiken_timer = 706,
        scene_taiken_last_info = 707,
        clone_talk = 708,
        scene_reset = 710,
        scene_minigame_karaoke_denmoku = 713,
        auth_skip_manager = 715,
        phase_arg_entity = 717,
        actor = 718,
        camera_generic = 719,
        mind_control_manager = 721,
        mind_control = 722,
        audience_mind_maker = 723,
        btl_audience_mind_maker = 724,
        lua_scene_vm_entity = 725,
        phase_list_entity = 728,
        scene_phase_base = 729,
        auth_node_root_path = 730,
        entity_effect_room_dust = 733,
        exclusion_control_checker = 735,
        minigame_cabaret_island_manager = 739,
        file_port_manager = 742,
        scene_minigame_blackjack = 744,
        minigame_blackjack = 748,
        camera_blackjack = 749,
        entity_effect_charge_dust = 750,
        entity_effect_rt_main_down_sampler = 755,
        entity_effect_window_light = 756,
        talk_text_man = 759,
        camera_cabaret_island = 766,
        window_light_dispose = 771,
        entity_effect_fall_rain = 775,
        fall_rain_dispose = 776,
        entity_effect_projection_pool_ = 783,
        scene_minigame_shogi = 785,
        mg_shogi_entity = 786,
        wrapping_collision_manager = 790,
        wrapping_collision = 791,
        scene_minigame_poker = 793,
        minigame_poker = 794,
        camera_poker = 795,
        projection_pool_dispose = 797,
        game_object = 800,
        scene_minigame_hanafuda = 801,
        minigame_hanafuda = 802,
        scene_minigame_lock_picking = 807,
        effect_cover_area = 808,
        hanafuda_camera = 809,
        physics_world = 810,
        scene_chase = 811,
        live_chase_manager = 812,
        lock_picking_manager = 813,
        asset_pickup_disable_range = 814,
        scene_minigame_ufo_catcher = 815,
        uniq_auth_manager = 816,
        run_trick_point_manager = 817,
        run_trick_point = 818,
        player_photo_mode_entity = 819,
        ufo_catcher_manager = 820,
        ufo_catcher_world = 822,
        video_call = 827,
        camera = 828,
        mg_shogi_asset_entity = 829,
        room_dust_dispose = 831,
        player_battle_buff_entity = 832,
        scene_minigame_golf_driving_range = 833,
        mg_golf_driving_range = 834,
        render_texture_entity = 836,
        friend_gauge_man = 837,
        unlocking_adv_man = 841,
        effect_analog_noise = 843,
        scene_minigame_m2ftg = 845,
        navi_node_entity = 850,
        navi_manager_entity = 851,
        camera_ufo_catcher = 852,
        camera_coin_locker = 854,
        scene_minigame_oichokabu = 856,
        minigame_oichokabu = 857,
        oichokabu_camera = 858,
        player_equip_entity = 866,
        navi_edge_entity = 880,
        navi_node_manager_entity = 881,
        navi_edge_manager_entity = 882,
        entity_effect_body_rain_splash = 884,
        minigame_batting_center_movie_player = 885,
        effect_character_halo = 888,
        player_equip_item_drop_entity = 889,
        pause_use_item_entity = 892,
        pickup_diapose_base = 901,
        skill_dispose = 902,
        minigame_hitting_effect_ball_track = 909,
        camera_gatling_gun = 910,
        coin_locker_key_manager = 917,
        coin_locker_key = 918,
        fish_generate = 925,
        fish_generate_manager = 926,
        escalator_base = 938,
        escalator_stairs = 939,
        escalator_footpath = 940,
        shop_manager_entity = 945,
        stage_resident_auth = 946,
        effect_tex_random_noise = 947,
        effect_tex_perlin_noise = 948,
        pickup_dispose_manager = 951,
        effect_old_film_noise = 952,
        escalator_manager = 953,
        continue_ui_flow_holder = 955,
        entity_effect_corn_light = 956,
        effect_corn_light_dispose = 957,
        effect_track_draw_manager = 960,
        entity_effect_vr_space = 961,
        minigame_photo_shooting_mission_manager = 966,
        minigame_photo_shooting_mission_entity = 967,
        minigame_photo_shooting_mission_character = 968,
        photo_mission_dispose_manager = 971,
        minigame_photo_shooting_mission_ui = 975,
        loading_text_entity = 981,
        minigame_decorating_office_ui = 982,
        entity_effect_caption = 983,
        dialog_information_manager = 984,
        asset_physics_disable_range = 987,
        effect_rain_fog_dispose = 990,
        effect_shootable_area = 991,
        entity_effect_window_rain = 996,
        window_rain_dispose = 997,
        effect_static_roof_mask_dispose = 999,
        window_rain_global_param_dispose = 1000,
        hp_recovery_range = 1005,
        item_dispose_secret_medicine = 1006,
        minigame_photo_shooting_mission_smartphone_accessor = 1009,
        player_photo_mode_scene_entity = 1010,
        tss_manager = 1011,
        tutorial_entity = 1018,
        camera_ui = 1020,
        staffroll_entity = 1021,
        ui_loading_image = 1022,
        particle_ui = 1029,
        vehicle_train_entity = 1030,
        ui_popup_number_manager = 1038,
        thumbnail_entity = 1043,
        sea_local_wave_dispose = 1048,
        wall_effect_plate = 1051,
        effect_grain_noise = 1053,
        party_equip_entity = 1056,
        callback_bool_uint_entity = 1065,
        effect_distortion = 1066,
        camera_filter_range_manager = 1068,
        talk_party_chat_manager = 1071,
        OBSOLETE_flow_entity = 1072,
        effect_event_dispose = 1087,
        talk_party_talk_manager = 1088,
        map_manager_entity = 1091,
        db_manager_entity = 1096,
        light_ev_char = 1107,
        sea_debris_dispose = 1108,
        link_any = 1110,
        sea_local_muddiness_dispose = 1114,
        part_time_job_quest_entity = 1116,
        player_personality = 1119,
        entity_effect_blast = 1121,
        score_ranking_manager = 1123,
        map_stage_entity = 1125,
        invisible_party_chat = 1128,
        puid_manager_entity = 1129,
        checkpoint_entity = 1130,
        grass_dispose = 1131,
        grass_area = 1134,
        talk_notice_gauge_manager = 1135,
        collection_spot = 1136,
        pause_auth_renderer_entity = 1139,
        grass_draw_manager = 1140,
        mission_objective_manager_entity = 1141,
        objective_manager_entity_base = 1142,
        talk_trust_param_manager = 1143,
        entity_effect_transition = 1144,
        wall_effect_no_fade = 1145,
        wall_effect_plate_no_fade = 1146,
        speech_subtitle_manager = 1147,
        picked_item = 1155,
        ui_create_item = 1162,
        crowd_tile_manager = 1163,
        crowd_tile_group = 1164,
        taxi_travel = 1167,
        smelting_manager = 1183,
        talk_adv_hp_gauge_manager = 1186,
        entity_effect_bush_shaker = 1187,
        street_obstacle_rectangle_planting = 1190,
        planting_collision_manager = 1191,
        dlc_manager = 1192,
        minigame_character = 1193,
        adv_battle_gauge_manager = 1194,
        height_field_manager = 1195,
        height_field_collision = 1196,
        asset_manager_draw = 1198,
        water_dispose_control_area = 1199,
        texture_auto_lod_manager = 1200,
        actor_manager = 1202,
        actor_team_manager = 1203,
        popup_talk = 1208,
        chase_qte_crowd = 1215,
        chase_qte_base = 1216,
        talk_log_manager = 1219,
        chase_qte_hurdle = 1229,
        chase_qte_branch = 1244,
        entity_effect_projection_graffiti = 1254,
        graffiti_dispose = 1255,
        font2_manager = 1256,
        chase_qte_wallrun = 1257,
        photo_scoop_mission_manager = 1261,
        post_effect_color_scheme = 1264,
        crowd_tile_characters = 1279,
        chase_qte_poleturn = 1280,
        pickup_diapose_generic = 1284,
        item_dispose_reward = 1286,
        mg_snack_mob_character = 1290,
        scene_reboot_self = 1293,
        entity_effect_volume_fog_ = 1294,
        volume_fog_dispose = 1295,
        title_character_controller = 1298,
        m3e_manager = 1304,
        m3e_console = 1306,
        chase_qte_kick_asset = 1307,
        button_guide_manager = 1308,
        camera_m3e = 1309,
        chase_dispose_item = 1310,
        effect_draw_base = 1313,
        popup_talk_group = 1331,
        scene_boot_to_load = 1332,
        scene_ps5_wait_deeplink = 1339,
        mg_homeless_manager = 1341,
        mg_homeless_can = 1342,
        mg_homeless_character = 1343,
        mg_homeless_player = 1344,
        mg_homeless_rival = 1345,
        mg_homeless_map = 1346,
        mg_homeless_map_creater = 1347,
        mg_homeless_navi_map = 1348,
        mg_homeless_truck = 1349,
        mg_homeless_camera = 1350,
        mg_homeless_anpan = 1351,
        mg_homeless_lucky_npc = 1352,
        mg_homeless_character_talk = 1353,
        mg_homeless_can_spot = 1354,
        mg_homeless_can_line = 1355,
        camera_rpg_battle = 1357,
        camera_free2 = 1359,
        scene_minigame_sujimon_battle = 1362,
        mg_sujimon_battle_manager = 1365,
        mg_sjmnbtl_battle_manager = 1366,
        mg_sjmnbtl_character = 1367,
        mg_sjmnbtl_party = 1368,
        mg_sjmnbtl_camera_manager = 1369,
        mg_sjmnbtl_trainer = 1370,
        mg_sjmnbtl_battle_menu = 1371,
        mg_sjmnbtl_roulette = 1372,
        mg_sjmnbtl_auto_battle_manager = 1373,
        mg_sjmnbtl_battle_alternation = 1374,
        mg_crazy_delivery_manager = 1375,
        mg_crazy_delivery_camera = 1376,
        mg_crazy_delivery_character = 1377,
        scene_minigame_crazy_delivery = 1379,
        scene_minigame_resort_island = 1380,
        scene_minigame_examination = 1381,
        emote_manager = 1383,
        mg_resort_island_manager = 1384,
        scene_implementation_tutorial_graphic = 1385,
        mg_resort_island_mode_housing_grid_range_base = 1386,
        mg_resort_island_mode_housing_grid_range_box = 1387,
        mg_sjmnbtl_behavior_property_play = 1394,
        mg_resort_island_reaction_object_rock = 1396,
        cabaret_yoasobi_man = 1397,
        mg_resort_island_reaction_object_trush = 1399,
        mg_crazy_delivery_dispose_item = 1401,
        mg_crazy_delivery_dispose_item_line = 1402,
        mg_sjmnbtl_auto_battle_alternation = 1403,
        mg_resort_island_mode_housing_grid_wall_decrement = 1404,
        mg_crazy_delivery_jump_point = 1405,
        minigame_shikaku2_main_manager = 1406,
        minigame_shikaku2_menu_ui_flow = 1408,
        insect_manager = 1409,
        insect_butterfly = 1410,
        insect_base = 1411,
        insect_dragonfly = 1412,
        trolley_bus_manager = 1414,
        trolley_bus_camera = 1415,
        mg_resort_island_camera_type_room_dispose = 1417,
        scene_minigame_examination_main = 1418,
        mg_resort_island_residents_manager = 1419,
        weather_dispose = 1420,
        mg_sjmnbtl_auto_battle_command = 1421,
        scene_minigame_homeless = 1423,
        mg_fishing_manager = 1424,
        general_timer_manager = 1426,
        mg_crazy_delivery_anchor_jump_point = 1428,
        drama_sanpo_manager = 1429,
        access_point_drama_sanpo = 1430,
        quest_manager = 1432,
        mg_resort_island_camera_type_room_default = 1434,
        camera_shogi = 1438,
        chishow_test_entity = 1439,
        mg_crazy_delivery_jump_board = 1440,
        sujimon_quest_spot = 1441,
        entity_effect_flow_bubble = 1442,
        entity_effect_swimming_pool = 1443,
        swimming_pool_dispose = 1444,
        item_present = 1446,
        wheelchair_character = 1447,
        actor_holder = 1448,
        mg_sjmnbtl_get_manager = 1449,
        mg_crazy_delivery_general_point = 1450,
        entity_effect_fall_snow = 1451,
        fall_snow_dispose = 1452,
        scene_minigame_matching_app = 1453,
        mg_matching_app_manager = 1454,
        entity_effect_projection_light = 1455,
        projection_light_dispose = 1456,
        insect_firefly = 1457,
        random_point_type_firefly_area = 1458,
        random_point_group_butterfly_perch = 1459,
        random_point_group_firefly_area = 1460,
        ajito_manager = 1461,
        mg_sjmnbtl_select_battle_manager = 1462,
        entity_effect_footprint = 1463,
        scene_display_texture = 1464,
        auth_node_character_motion_blend_shape = 1465,
        minigame_stuntman = 1466,
        scene_minigame_stuntman = 1467,
        popup_talk_random = 1468,
        stage_instance_entity = 1469,
        mg_crazy_delivery_dispose_character = 1470,
        mg_crazy_delivery_customer = 1471,
        dispose_entity_group = 1472,
        dungeon_manager = 1473,
        sujimon_item_spot = 1475,
        insect_locust = 1476,
        random_point_group_insect = 1477,
        entity_effect_flow_volume = 1478,
        sound_amb_change_line_range = 1484,
        insect_drosophila = 1485,
        photo_rally_spot = 1487,
        mg_sujimon_get_chance_position = 1488,
        scene_minigame_sujimon_get_chance = 1489,
        mg_sjmngch_manager = 1490,
        scene_boot = 1492,
        minigame_stuntman_manager = 1493,
        scene_display_auth = 1495,
        scene_game_boot = 1496,
        mg_resort_island_ui_top_bar = 1497,
        car_encount_manager = 1498,
        najimi_manager = 1499,
        insect_unknown = 1502,
        mg_sjmnbtl_vermin = 1503,
        entity_effect_sky_rainbow = 1504,
        sky_rainbow_dispose = 1505,
        mg_fishing_adv_manager = 1506,
        mg_resort_island_camera_type_base = 1507,
        scene_minigame_snap_mode = 1508,
        photo_rally_manager = 1509,
        mg_resort_island_housing_parts_access_entity = 1510,
        insect_beetle = 1511,
        mg_resort_island_camera_type_editmode_outside = 1512,
        najimi_quest_spot = 1513,
        mg_snap_mode_manager = 1514,
        mg_snap_mode_camera = 1515,
        murai_game_manager = 1516,
        mg_resort_island_camera_type_free_camera_outside = 1517,
        photo_point = 1518,
        flow_volume_dispose = 1519,
        wide_area_activity_manager = 1520,
        mg_resort_island_camera_type_sujimon_park_fureai = 1521,
        day_night_cycle_dispose = 1522,
        minigame_result_manager = 1523,
        character_selector = 1524,
        mg_resort_island_residents_road = 1525,
        ending_note_manager = 1526,
        dish_generator = 1527,
        item_drop_manager = 1528,
        scene_language_selector = 1529,
        scene_reset_for_restore_save = 1530,
        mg_resort_island_camera_type_editmode_room = 1531,
        mg_resort_island_camera_type_editmode_base = 1532,
        sound_portable_music_player = 1533,
        lite_processing_manager = 1534,
        effect_tire_mark_entity = 1535,
        notice_manager = 1536,
        flow_volume_spreader_dispose = 1539,
        ability_notice = 1540,
        mg_resort_island_camera_type_battle_mission_start = 1542,
        scene_fiction = 1544,
        clock_range_pkg_holder = 1545,
        mg_resort_island_camera_type_editmode_wall = 1546,
        invisible_area = 1547,
        overlay_movie_manager = 1549,
    }
#else
        empty,        // constant 0x0
        entity_base,         // constant 0x1
        scene_base,      // constant 0x2
        scene,      // constant 0x7E
        scene_empty,         // constant 0x24B
        scene_action_first_tenkaichi,        // constant 0x4
        scene_action_first_nakamichi,        // constant 0x5
        scene_action_first_building,         // constant 0x6
        scene_action_second_test_scenario,       // constant 0x7A
        scene_action_alpha_test_area,        // constant 0x210
        scene_action_pjm_movie,      // constant 0x7
        scene_action_first_tenkaichi_many,       // constant 0x8
        scene_startup_commandline,       // constant 0x9
        scene_auth_preview,      // constant 0xB2
        scene_auth_test,         // constant 0xB3
        scene_auth_stage_change,         // constant 0xB4
        scene_action_battle_validation,      // constant 0xE3
        scene_action_battle_validation_only_btl,         // constant 0xF5
        scene_main,      // constant 0x15C
        scene_shop,      // constant 0x186
        scene_title,         // constant 0x185
        scene_continue,      // constant 0x193
        scene_clean_up_bridge,       // constant 0x1C8
        scene_minigame,      // constant 0x1FB
        scene_minigame_switching_exe,        // constant 0x2AC
        scene_minigame_batting_center,       // constant 0x202
        scene_minigame_sandlot_baseball,         // constant 0x209
        scene_minigame_snack,        // constant 0x213
        scene_minigame_darts,        // constant 0x21D
        scene_minigame_mahjong,      // constant 0x228
        scene_minigame_karaoke,      // constant 0x237
        scene_minigame_karaoke_denmoku,      // constant 0x2C9
        scene_minigame_sandlot_baseball_menu,        // constant 0x265
        scene_minigame_offertory_box,        // constant 0x272
        scene_minigame_puyo,         // constant 0x28E
        scene_minigame_segaages_fzn,         // constant 0x296
        scene_minigame_segaages_hng,         // constant 0x297
        scene_minigame_segaages_otr,         // constant 0x298
        scene_minigame_segaages_sph,         // constant 0x299
        scene_reboot_elf,        // constant 0x4AB
        scene_minigame_toylets,      // constant 0x33A
        scene_minigame_golf_driving_range,       // constant 0x341
        scene_vf5fs,         // constant 0x278
        scene_cache_restore,         // constant 0x2A1
        scene_timer_setting,         // constant 0x2C1
        scene_taiken_last_info,      // constant 0x2C3
        scene_reset,         // constant 0x2C6
        scene_minigame_cabaret_island,       // constant 0x2E4
        scene_minigame_chinchiro,        // constant 0x2E5
        scene_minigame_blackjack,        // constant 0x2E8
        scene_minigame_poker,        // constant 0x319
        scene_minigame_roulette,         // constant 0x309
        scene_stalking,      // constant 0x2F1
        scene_chase,         // constant 0x32B
        scene_minigame_shogi,        // constant 0x311
        scene_minigame_drone_race,       // constant 0x314
        scene_minigame_drone_race_reverse,       // constant 0x38A
        scene_minigame_drone_vs,         // constant 0x394
        scene_minigame_drone_vs2,        // constant 0x39C
        scene_minigame_hanafuda,         // constant 0x321
        scene_minigame_lock_picking,         // constant 0x327
        scene_minigame_ufo_catcher,      // constant 0x32F
        scene_minigame_m2ftg,        // constant 0x34D
        scene_minigame_oichokabu,        // constant 0x358
        scene_minigame_sugoroku,         // constant 0x388
        scene_sugoroku_battle,       // constant 0x3E9
        scene_minigame_decorate_office,      // constant 0x3BE
        scene_minigame_photo_shooting,       // constant 0x3BF
        scene_minigame_drone_select_tgs,         // constant 0x3FF
        scene_minigame_dragon_cart,      // constant 0x400
        scene_minigame_dragon_cart_menu,         // constant 0x42D
        scene_minigame_dragon_cart_rival_story,      // constant 0x48D
        scene_minigame_meigaza,      // constant 0x40F
        scene_minigame_homeless,         // constant 0x425
        scene_upstart_management,        // constant 0x457
        scene_upstart_battle,        // constant 0x458
        scene_minigame_pachislot,        // constant 0x48E
        scene_minigame_shikaku,      // constant 0x499
        scene_minigame_shikaku_main,         // constant 0x49B
        scene_minigame_pachislot_dll,        // constant 0x4AD
        scene_playing_to_load,       // constant 0x4AE
        scene_system_selector,       // constant 0x3
        scene_scenario_selector,         // constant 0xFF
        scene_scenario_selector_impl,        // constant 0x10A
        debug_chara_selector_entity,         // constant 0x3A2
        debug_selector_favo,         // constant 0x3DA
        scene_pub_fly_through,       // constant 0x1B8
        scene_test_toki01,       // constant 0xB
        scene_test_toki02,       // constant 0xC
        scene_test_toki03,       // constant 0xD
        scene_test_toki04,       // constant 0xE
        scene_test_toki05,       // constant 0xF
        scene_test_toki06,       // constant 0x10
        scene_test_toki07,       // constant 0xAB
        scene_test_toki08,       // constant 0x161
        scene_test_toki09,       // constant 0x1B1
        scene_test_toki10,       // constant 0x1CB
        scene_test_toki11,       // constant 0x266
        scene_test_toki12,       // constant 0x2E1
        scene_test_toki13,       // constant 0x2FB
        scene_test_toki14,       // constant 0x3CA
        scene_test_auto_exit,        // constant 0x100
        scene_test_oda01,        // constant 0x11
        scene_test_oyama01,      // constant 0x12
        scene_test_chara,        // constant 0x13
        scene_test_study,        // constant 0x3F7
        scene_test_cube_render,      // constant 0x15
        scene_test_eito01,       // constant 0x16
        scene_test_eito_examples,        // constant 0x17
        scene_test_eito_swathe,      // constant 0x18
        scene_test_eito_sequel,      // constant 0x140
        scene_test_atsu01,       // constant 0x19
        scene_test_physics01,        // constant 0x435
        scene_test_physics02,        // constant 0x436
        scene_test_physics03,        // constant 0x437
        scene_test_waraman01,        // constant 0x1A
        scene_test_waraman02,        // constant 0x1B
        scene_test_waraman03,        // constant 0x1C
        scene_test_waraman_exp,      // constant 0x1EE
        scene_test_waraman_exp2,         // constant 0x2F0
        scene_test_waraman_exp3,         // constant 0x31F
        scene_test_waraman_exp4,         // constant 0x42B
        scene_test_effect_glass,         // constant 0x1D
        scene_test_effect_vollight,      // constant 0x1E
        scene_test_effect_chara,         // constant 0x33E
        scene_test_takakie01,        // constant 0x1F
        scene_test_asset,        // constant 0x20
        scene_test_asset_auto_check,         // constant 0x22
        scene_test_inaguma_debug01,      // constant 0x23
        scene_test_inaguma_debug02,      // constant 0x24
        scene_test_inaguma_debug03,      // constant 0x25
        scene_test_shoji_k,      // constant 0x26
        scene_test_shoji_k_save_data,        // constant 0xBF
        scene_test_shoji_k_companion,        // constant 0xD0
        scene_test_shoji_k_webapi,       // constant 0x315
        scene_test_battle,       // constant 0x27
        scene_test_battle_hact,      // constant 0x1A5
        scene_test_talk,         // constant 0x28
        scene_test_chishow,      // constant 0x29
        scene_test_motion_conv,      // constant 0x124
        scene_test_ai_mob,       // constant 0x7C
        scene_test_ai_mob_avoid,         // constant 0x284
        scene_test_ai_combat,        // constant 0xD6
        scene_test_instant_chat,         // constant 0x11E
        scene_test_spatial_perception,       // constant 0x126
        scene_test_encount,      // constant 0x14C
        scene_test_car_encount,      // constant 0x144
        scene_test_nakais,       // constant 0x7D
        scene_test_nakais_db,        // constant 0x8B
        scene_test_nakais_puid,      // constant 0x9E
        scene_test_nakais_ui,        // constant 0x10E
        scene_test_nakais_font,      // constant 0x1A8
        scene_test_nakais_network,       // constant 0x248
        scene_test_nakais_ui_3d,         // constant 0x34A
        scene_test_nakais_map_navi,      // constant 0x34E
        scene_test_ui_spy,       // constant 0x2DC
        scene_test_ui_test,      // constant 0x117
        scene_test_ui_test_3d,       // constant 0x35C
        scene_test_db_unittest,      // constant 0x3D9
        scene_test_libpuid_unittest,         // constant 0x3E6
        scene_test_map_unittest,         // constant 0x3E2
        scene_test_ui_unittest,      // constant 0x3E3
        scene_test_json_unittest,        // constant 0x441
        scene_test_rpc_unittest,         // constant 0x442
        scene_test_upstart_battle,       // constant 0x449
        scene_test_upstart_management,       // constant 0x44B
        scene_test_shop_kamuro,      // constant 0x1A2
        scene_test_particle,         // constant 0x94
        scene_test_simple_auth_play,         // constant 0x224
        scene_test_crowd,        // constant 0x47D
        scene_test_jack,         // constant 0xEB
        scene_test_google,       // constant 0x2A
        scene_test_yhattori_http_server,         // constant 0x9D
        scene_test_yhattori_movie,       // constant 0xDA
        scene_test_yhattori_sound,       // constant 0xFB
        scene_test_kato_00,      // constant 0x9C
        scene_test_kato_01,      // constant 0xC6
        scene_test_kato_02,      // constant 0xC7
        scene_test_look_at,      // constant 0xA6
        scene_test_ohtah_00,         // constant 0x125
        scene_test_talk_00,      // constant 0x12C
        scene_test_talk_01,      // constant 0x159
        scene_test_taxi,         // constant 0x27C
        scene_test_unlocking,        // constant 0x34F
        scene_test_dynamic_collision,        // constant 0x15A
        scene_test_dynamic_collision_battle,         // constant 0x348
        scene_test_wanderer,         // constant 0x195
        scene_test_sound_exec_test,      // constant 0x1AE
        scene_test_yhattori_sound_test,      // constant 0x1FA
        scene_test_inaguma_minigame,         // constant 0x1FC
        scene_test_simple_boot,      // constant 0x24C
        scene_test_namiki_00,        // constant 0x24E
        scene_test_namiki_01,        // constant 0x361
        scene_test_sakaue,       // constant 0x24D
        scene_test_pitch_ball,       // constant 0x256
        scene_test_yoshimitsu,       // constant 0x268
        scene_test_minigame_offerings,       // constant 0x2AA
        scene_test_satohk,       // constant 0x306
        scene_test_character_preview,        // constant 0x31E
        scene_test_yanagisako,       // constant 0x326
        scene_test_takahashit3,      // constant 0x346
        scene_test_sugoroku_default,         // constant 0x35D
        scene_test_sugoroku_kamuro,      // constant 0x35F
        scene_test_kurimoto01,       // constant 0x3F8
        scene_test_kurimoto02,       // constant 0x3FB
        scene_test_kuwabara01,       // constant 0x3F9
        scene_test_dragon_cart,      // constant 0x401
        scene_test_meigaza,      // constant 0x40A
        scene_test_itotsyk_01,       // constant 0x416
        scene_test_ui_example,       // constant 0x431
        scene_test_ui_sample,        // constant 0x432
        scene_test_party_talk,       // constant 0x43C
        scene_test_excavator_continuous_track,       // constant 0x471
        scene_test_animal_ik,        // constant 0x472
        scene_test_xu,       // constant 0x491
        scene_test_horie,        // constant 0x492
        scene_test_oto,      // constant 0x493
        scene_test_ishii,        // constant 0x494
        scene_test_yonezawa,         // constant 0x495
        scene_test_teacher,      // constant 0x49E
        debug_entity_arrow,      // constant 0x409
        scene_selector,      // constant 0x2B
        scene_commandline_impl,      // constant 0x2C
        camera_base,         // constant 0x2D
        camera,      // constant 0x33C
        camera_free,         // constant 0x2E
        camera_drone_adv,        // constant 0x2FD
        camera_fps,      // constant 0x275
        camera_fps_drone_adv,        // constant 0x301
        camera_fps_talk,         // constant 0x3F6
        camera_pause_menu,       // constant 0xE8
        camera_photo,        // constant 0x212
        camera_selfie,       // constant 0x2BF
        camera_gatling_gun,      // constant 0x38E
        camera_auth,         // constant 0x2F
        camera_title,        // constant 0x30
        camera_fight,        // constant 0x31
        camera_mirror,       // constant 0x1FF
        camera_pip,      // constant 0x25B
        camera_turn_btl,         // constant 0x40B
        camera_null,         // constant 0x32
        camera_test,         // constant 0x33
        camera_test_holder,      // constant 0x1C6
        camera_static,       // constant 0xD1
        camera_particle_viewer,      // constant 0xB8
        camera_particle_capture,         // constant 0x3E0
        camera_maya,         // constant 0x34
        camera_cube_render,      // constant 0x35
        camera_test_atsu01,      // constant 0x36
        camera_mg_darts,         // constant 0x267
        camera_minigame_karaoke,         // constant 0x286
        camera_cabaret,      // constant 0x293
        camera_cat_cafe,         // constant 0x2A8
        camera_generic,      // constant 0x2CF
        camera_drone,        // constant 0x2E9
        camera_minigame_chinchiro,       // constant 0x31C
        camera_ufo_catcher,      // constant 0x354
        camera_coin_locker,      // constant 0x356
        camera_sugoroku,         // constant 0x357
        camera_ui,       // constant 0x3FC
        camera_verification_fps,         // constant 0x3AD
        camera_verification_look_at,         // constant 0x3AE
        camera_rpg_battle,       // constant 0x40C
        camera_minigame_pachislot,       // constant 0x4A4
        stage,      // constant 0x37
        stage_area_range,        // constant 0x171
        stage_area_range_bg,         // constant 0x173
        stage_resident_auth,         // constant 0x3B2
        wind,        // constant 0x38
        wind_range,      // constant 0x234
        game_object,         // constant 0x320
        character_base,      // constant 0xD3
        character_model_check,       // constant 0x12D
        character_picture,       // constant 0x249
        minigame_character,      // constant 0x4A9
        character,      // constant 0xD5
        human_jaunt,         // constant 0x3C
        character_manager,       // constant 0xD4
        character_dispose_manager,       // constant 0x17F
        human_view_com,      // constant 0xA3
        collision,       // constant 0x3D
        navigation_mesh,         // constant 0x3E
        effect_manager,      // constant 0x3F
        effect_base,         // constant 0x40
        effect_glass,        // constant 0x41
        effect_glass_manager,        // constant 0xA9
        effect_vollight,         // constant 0x43
        effect_vollight_manager,         // constant 0xAA
        effect_track,        // constant 0x175
        effect_track_human,      // constant 0x17C
        effect_track_asset,      // constant 0x17D
        effect_track_draw_manager,       // constant 0x3C0
        effect_draw_entity,      // constant 0x181
        effect_draw_manager,         // constant 0x182
        effect_event_manager,        // constant 0x1B9
        effect_event_chara,      // constant 0x1D0
        effect_event_dispose,        // constant 0x43F
        effect_mirror_manager,       // constant 0x1FE
        effect_draw_mask_manager,        // constant 0x21F
        effect_lens_flare,       // constant 0x22B
        effect_draw_pip,         // constant 0x25C
        effect_discoball,        // constant 0x273
        effect_mission_area,         // constant 0x295
        effect_cover_area,       // constant 0x328
        effect_shootable_area,       // constant 0x3DF
        effect_analog_noise,         // constant 0x34B
        effect_old_film_noise,       // constant 0x3B8
        effect_tex_random_noise,         // constant 0x3B3
        effect_tex_perlin_noise,         // constant 0x3B4
        effect_character_halo,       // constant 0x378
        ui_fade_manager,         // constant 0x45
        ui_nowloading,       // constant 0x46
        ui_manager,      // constant 0x114
        ui_entity_base,      // constant 0x179
        ui_entity_enemy_gauge_manager,       // constant 0x1CD
        player_ability_entity,       // constant 0x263
        player_point_entity,         // constant 0x226
        player_equip_entity,         // constant 0x362
        player_carry_item_entity,        // constant 0x2A5
        player_carry_item_launcher_entity,       // constant 0x3EA
        player_equip_item_drop_entity,       // constant 0x379
        pause_use_item_entity,       // constant 0x37C
        OBSOLETE_flow_entity,        // constant 0x430
        map_minimap_entity,      // constant 0x246
        player_photo_mode_entity,        // constant 0x333
        player_photo_mode_scene_entity,      // constant 0x3F2
        map_stage_entity,        // constant 0x465
        map_render_entity,       // constant 0x279
        objective_entity,        // constant 0x1B5
        objective_manager_entity,        // constant 0x1B6
        mission_objective_manager_entity,        // constant 0x475
        objective_manager_entity_base,       // constant 0x476
        player_action_entity,        // constant 0x1B7
        ui_entity_player_gauge,      // constant 0x18E
        player_battle_buff_entity,       // constant 0x340
        encount_unsafe_area_entity,      // constant 0x3EF
        map_manager_entity,      // constant 0x443
        game_var_manager,        // constant 0x127
        fade_manager,        // constant 0x130
        puid_manager_entity,         // constant 0x469
        db_manager_entity,       // constant 0x448
        map_icon_entity,         // constant 0x1D1
        navi_node_entity,        // constant 0x352
        navi_node_manager_entity,        // constant 0x371
        navi_edge_entity,        // constant 0x370
        navi_edge_manager_entity,        // constant 0x372
        navi_manager_entity,         // constant 0x353
        tips_manager_entity,         // constant 0x1D5
        tww_manager_entity,      // constant 0x1F7
        tww_notify_entity,       // constant 0x27B
        thumbnail_entity,        // constant 0x413
        title_entity,        // constant 0x2B2
        pause_mail_manager_entity,       // constant 0x276
        pause_message_manager_entity,        // constant 0x399
        player_pause_entity,         // constant 0x244
        player_activity_entity,      // constant 0x258
        player_qte_entity,       // constant 0x289
        popup_manager_entity,        // constant 0x22D
        manual_entity,       // constant 0x260
        pause_sns_manager_entity,        // constant 0x277
        item_get_manager_entity,         // constant 0x27D
        shop_manager_entity,         // constant 0x3B1
        roulette_manager_entity,         // constant 0x30A
        pause_crowdfunding_entity,       // constant 0x3DC
        ui_loading_image,        // constant 0x3FE
        light_manager,       // constant 0x47
        light_reference,         // constant 0x48
        light_ev,        // constant 0x49
        light_point,         // constant 0x4A
        light_spot,      // constant 0x4B
        light_tube,      // constant 0x4C
        light_tube2,         // constant 0x10F
        light_ambient,       // constant 0x4D
        light_directional,       // constant 0x4E
        light_color,         // constant 0x7F
        light_pcc,       // constant 0xE6
        light_sh_orbox,      // constant 0x137
        light_hemi_orbox,        // constant 0x16D
        light_dark_point,        // constant 0x167
        light_dark_spot,         // constant 0x168
        light_dark_tube,         // constant 0x169
        light_dark_orbox,        // constant 0x166
        asset_manager,       // constant 0x4F
        asset_unit,      // constant 0x50
        asset_geometry,      // constant 0x176
        asset_scrap_group,       // constant 0x154
        asset_scrap,         // constant 0x155
        asset_base_common,       // constant 0x156
        asset_preload,       // constant 0x1ED
        asset_tool_com,      // constant 0x30C
        asset_pickup_disable_range,      // constant 0x32E
        asset_physics_disable_range,         // constant 0x3DB
        asset_treasure_box,      // constant 0x389
        shop_destory_manager,        // constant 0x28A
        battle_manager,      // constant 0xE4
        motion_manager,      // constant 0x10D
        auth_manager,        // constant 0x55
        auth_play_base,      // constant 0x131
        auth_play,       // constant 0x56
        auth_node_base,      // constant 0x57
        auth_node_animatable_base,       // constant 0x1E9
        auth_node_path,      // constant 0x58
        auth_node_path_motion,       // constant 0x151
        auth_node_camera,        // constant 0x59
        auth_node_camera_motion,         // constant 0x89
        auth_node_character,         // constant 0x118
        auth_node_character_motion,      // constant 0x119
        auth_node_character_behavior,        // constant 0x11A
        auth_node_character_behavior_simple_talk,        // constant 0x44E
        auth_node_model,         // constant 0x14F
        auth_node_asset,         // constant 0x150
        auth_node_asset_motion,      // constant 0x152
        auth_node_character_node,        // constant 0x11B
        auth_node_element,       // constant 0xC9
        auth_node_stage,         // constant 0xAD
        auth_node_motion_base,       // constant 0x15F
        behavior_property_play,      // constant 0x106
        behavior_property_manager,       // constant 0x107
        auth_node_folder_condition,      // constant 0x184
        auth_node_asset_node,        // constant 0x1B2
        auth_node_root_path,         // constant 0x2DA
        auth_play_com,       // constant 0x5C
        auth_color_picker_tool,      // constant 0xD8
        auth_debug_com,      // constant 0x1F9
        post_effect_dof,         // constant 0xC0
        post_effect_glare,       // constant 0xC1
        post_effect_tonemap,         // constant 0xC2
        post_effect_filmgrain,       // constant 0xC3
        post_effect_color_correction,        // constant 0xC4
        post_effect_motion_blur,         // constant 0x116
        post_effect_feedback,        // constant 0x129
        post_effect_chromatic_aberration,        // constant 0x12A
        post_effect_vignette,        // constant 0x12B
        post_effect_ldr_color_correction,        // constant 0x194
        color_correction_range_manager,      // constant 0x1A6
        invisible_base,      // constant 0x63
        invisible_talk,      // constant 0x60
        invisible_allstage,      // constant 0x2A6
        reaction_entity_base,        // constant 0x15E
        timer_entity,        // constant 0x14D
        counter_entity,      // constant 0x14E
        vending_machine,         // constant 0x25E
        driftage,        // constant 0x2B8
        play_go_message,         // constant 0x2A9
        clone_talk,      // constant 0x2C4
        invisible_party_chat,        // constant 0x468
        particle_manager,        // constant 0xAE
        particle_resource,       // constant 0xAF
        particle_instance,       // constant 0xB0
        particle_instance_set,       // constant 0xCA
        particle_interface,      // constant 0xCB
        particle_dispose,        // constant 0x12F
        particle_ui,         // constant 0x405
        particle_viewer,         // constant 0xB9
        particle_emitter_guide,      // constant 0xC8
        ccc_manager,         // constant 0x79
        timeline_manager,        // constant 0x1E2
        physics_manager,         // constant 0xA7
        pause_manager,       // constant 0x1AF
        movie_tex_manager,       // constant 0xDD
        sound_manager,       // constant 0xFC
        sound_sphere,        // constant 0x18D
        sound_amb_change_range,      // constant 0x191
        movie_tex_dispose,       // constant 0x1BC
        volume_preset_range,         // constant 0x1C1
        sound_dispose_entity_base,       // constant 0x23D
        sound_amb_volume_range,      // constant 0x23E
        sound_line_source,       // constant 0x23F
        rumble_manager,      // constant 0x190
        msg_window_manager,      // constant 0x1C5
        post_effect_manager,         // constant 0xF3
        camera_filter_range_manager,         // constant 0x42C
        entity_FullBodyIKDbgMngr,        // constant 0x66
        test1,       // constant 0x67
        test2,       // constant 0x68
        hoge_entity,         // constant 0x69
        debug_scene_preparing,       // constant 0x3C2
        debug_scene_transition,      // constant 0x424
        editor_manager,      // constant 0x6A
        editor_entity,       // constant 0x6B
        editor_base,         // constant 0x80
        entity_encyclopedia,         // constant 0x6C
        test_sphere,         // constant 0xED
        test_rectangle,      // constant 0xEF
        test_trapezoid,      // constant 0xF2
        debug_manager,       // constant 0x16E
        debug_pick_manager,      // constant 0x2D7
        ui_debug_server_entity,      // constant 0x2DB
        ui_debug_server_for_sb_entity,       // constant 0x2E7
        debug_character_manager,         // constant 0x2DE
        artisan_host,        // constant 0x337
        primitive_editor_manager,        // constant 0xCD
        primitive_editor,        // constant 0xB1
        primitive_editor_system_multi_select,        // constant 0xCE
        primitive_editor_cpr_sphere,         // constant 0xAC
        primitive_editor_cpr_aabox,      // constant 0xB6
        primitive_editor_cpr_orbox,      // constant 0xB7
        primitive_editor_scaling_orbox,      // constant 0x183
        primitive_editor_point,      // constant 0xBC
        primitive_editor_point_and_direction,        // constant 0x2D0
        primitive_editor_point_and_orient,       // constant 0x13B
        primitive_editor_point_and_orient_and_scale,         // constant 0x2D6
        primitive_editor_point_and_radian,       // constant 0x2FA
        primitive_editor_points,         // constant 0xBD
        primitive_editor_points_and_radian,      // constant 0x2F8
        primitive_editor_cpr_capsule,        // constant 0xD2
        primitive_editor_cpr_segment,        // constant 0xDB
        primitive_editor_dievent_path,       // constant 0xBE
        primitive_editor_ccc_point,      // constant 0x2F9
        primitive_editor_ccc_range_sphere,       // constant 0xCF
        primitive_editor_ccc_range_orbox,        // constant 0x12E
        primitive_editor_ccc_range_cylinder,         // constant 0x19F
        primitive_editor_entity_sphere,      // constant 0xEE
        primitive_editor_entity_rectangle,       // constant 0xF0
        primitive_editor_entity_trapezoid,       // constant 0xF1
        primitive_editor_wall_plate,         // constant 0x18A
        primitive_editor_wall,       // constant 0x199
        primitive_editor_wall_capsule,       // constant 0x19A
        primitive_editor_wall_triangles,         // constant 0x1F0
        primitive_editor_wall_quadrangles,       // constant 0x1F4
        primitive_editor_range_sphere,       // constant 0x1D9
        primitive_editor_range_aabox,        // constant 0x1DA
        primitive_editor_range_orbox,        // constant 0x1DB
        primitive_editor_range_capsule,      // constant 0x1DC
        primitive_editor_height,         // constant 0x1F5
        primitive_editor_cpr_spheres,        // constant 0x239
        primitive_editor_pos_and_direction,      // constant 0x24A
        scene_test_stage,        // constant 0x115
        entity_effect_splash_liquid,         // constant 0x8C
        entity_effect_flow_dust,         // constant 0xA2
        entity_effect_glass_water,       // constant 0xBA
        entity_effect_sea,       // constant 0xD7
        entity_effect_hit_wound,         // constant 0x153
        entity_effect_sky_cloud,         // constant 0x16F
        entity_effect_sky_star,      // constant 0x1E1
        entity_effect_confetti,      // constant 0x17E
        entity_effect_flow_bubble,       // constant 0x282
        entity_effect_room_dust,         // constant 0x2DD
        entity_effect_charge_dust,       // constant 0x2EE
        entity_effect_rt_main_down_sampler,      // constant 0x2F3
        entity_effect_window_light,      // constant 0x2F4
        entity_effect_fall_snow,         // constant 0x304
        entity_effect_fall_rain,         // constant 0x307
        entity_effect_projection_pool_,      // constant 0x30F
        entity_effect_body_rain_splash,      // constant 0x374
        entity_effect_projection_light,      // constant 0x390
        entity_effect_corn_light,        // constant 0x3BC
        entity_effect_vr_space,      // constant 0x3C1
        entity_effect_caption,       // constant 0x3D7
        entity_effect_window_rain,       // constant 0x3E4
        entity_effect_ink_blur_canvas,       // constant 0x422
        entity_effect_delay_painting,        // constant 0x42E
        entity_effect_projection_paint_,         // constant 0x45E
        entity_effect_transition,        // constant 0x478
        entity_effect_bush_shaker,       // constant 0x4A3
        scene_phase_base,        // constant 0x2D9
        scene_lua_command_debug,         // constant 0x78
        street_map,      // constant 0x95
        street_range_base,       // constant 0x96
        street_range_rectangle,      // constant 0x97
        street_range_trapezoid,      // constant 0x9F
        street_range_quadrangle,         // constant 0xA1
        street_obstacle_manager,         // constant 0x19B
        street_obstacle_base,        // constant 0x99
        street_obstacle_circle,      // constant 0x9A
        street_obstacle_rectangle,       // constant 0x9B
        street_obstacle_rectangle_planting,      // constant 0x4A6
        street_map_area_manager,         // constant 0x252
        street_map_area,         // constant 0x253
        team_manager,        // constant 0xE1
        instant_chat_manager,        // constant 0x113
        curtain_range_manager,       // constant 0x122
        curtain_range,       // constant 0x123
        route_path_manager,      // constant 0x18B
        route_path,      // constant 0x18C
        encount_manager,         // constant 0x19E
        encount_battle,      // constant 0x205
        navimesh_collision_manager,      // constant 0x200
        navimesh_manager,        // constant 0x211
        npc_tag_manager,         // constant 0x20C
        general_car_manager,         // constant 0x214
        general_car_road,        // constant 0x215
        general_car_intersection_manager,        // constant 0x242
        general_car_intersection,        // constant 0x243
        npc_control_manager,         // constant 0x230
        npc_control,         // constant 0x231
        player_control_manager,      // constant 0x29F
        player_control,      // constant 0x2A0
        entity_render_target_effect_work,        // constant 0xCC
        dispose_folder,      // constant 0x10B
        dispose_workgroup,       // constant 0x10C
        range_manager,       // constant 0x13C
        range_base,      // constant 0x13E
        range,       // constant 0x157
        hp_recovery_range,       // constant 0x3ED
        wall_manager,        // constant 0x22C
        wall_base,       // constant 0x1F3
        wall,        // constant 0x178
        wall_plate,      // constant 0x188
        wall_capsule,        // constant 0x189
        wall_effect,         // constant 0x1C7
        wall_effect_no_fade,         // constant 0x479
        wall_effect_plate,       // constant 0x41B
        wall_effect_plate_no_fade,       // constant 0x47A
        wall_effect_drone,       // constant 0x338
        wall_triangles,      // constant 0x1F1
        wall_quadrangles,        // constant 0x1F6
        mirror_rect,         // constant 0x1FD
        dispose_manager,         // constant 0x1AC
        uniq_dispose_manager,        // constant 0x1EF
        lod_manager,         // constant 0x1B0
        simple_texture_entity,       // constant 0x158
        simple_ui_entity,        // constant 0x16A
        foreground_texture_entity,       // constant 0x236
        foreground_ui_texture_que,       // constant 0x23A
        simple_movie_entity,         // constant 0x26F
        lua_scene_vm_entity,         // constant 0x2D5
        inhibition_updater,      // constant 0x1A7
        save_data_manager,       // constant 0x197
        scene_info_handover,         // constant 0x1AB
        dead_effect,         // constant 0x1CA
        load_position_entity,        // constant 0x29D
        pause_id_manager,        // constant 0x29E
        result_result_entity,        // constant 0x2A2
        time_updater,        // constant 0x2A7
        battle_preload_entity,       // constant 0x2C0
        auth_skip_manager,       // constant 0x2CB
        loading_text_entity,         // constant 0x3D5
        link_any,        // constant 0x456
        part_time_job_quest_entity,      // constant 0x45C
        continue_ui_flow_holder,         // constant 0x3BB
        checkpoint_entity,       // constant 0x46A
        door_range_manager,      // constant 0x198
        sea_dispose,         // constant 0x15B
        sea_cap_dispose,         // constant 0x245
        sea_local_wave_dispose,      // constant 0x418
        sea_local_muddiness_dispose,         // constant 0x45A
        lua_ui_handle_entity,        // constant 0x19C
        lua_ui_flow_entity,      // constant 0x19D
        lua_ui_caption_entity,       // constant 0x221
        phase_list_entity,       // constant 0x2D8
        phase_arg_entity,        // constant 0x2CD
        chara_generate_manager,      // constant 0x1BE
        chara_generate,      // constant 0x1BF
        ai_manager,      // constant 0x1C2
        mind_control_manager,        // constant 0x2D1
        mind_control,        // constant 0x2D2
        audience_mind_maker,         // constant 0x2D3
        btl_audience_mind_maker,         // constant 0x2D4
        live_chase_manager,      // constant 0x32C
        sub_story_manager,       // constant 0x1C3
        mission_area_manager,        // constant 0x1DE
        mission_area,        // constant 0x1DF
        mission_manager,         // constant 0x1E3
        mission_base,        // constant 0x1E4
        random_point_manager,        // constant 0x2B9
        random_point,        // constant 0x2BA
        scene_fade_ctrl,         // constant 0x1BD
        scene_ex_status_callback_ctrl,       // constant 0x1CF
        tutorial_show,       // constant 0x1D3
        entity_sweeper,      // constant 0x1D2
        navi_mesh_loader,        // constant 0x1D4
        draw_suspend,        // constant 0x1DD
        callback_db_holder,      // constant 0x1E6
        callback_bool_void_entity,       // constant 0x1E7
        callback_void_void_entity,       // constant 0x1E8
        callback_bool_uint_ref_entity,       // constant 0x2BD
        callback_bool_uint_entity,       // constant 0x429
        side_case_manager,       // constant 0x343
        verification_manager,        // constant 0x38F
        survey_point_entity,         // constant 0x3A5
        exclusion_control_checker,       // constant 0x2DF
        render_texture_entity,       // constant 0x344
        video_call,      // constant 0x33B
        actor,       // constant 0x2CE
        sky_star_dispose,        // constant 0x1E5
        sky_cloud_dispose,       // constant 0x1F2
        traffic_signal_manager,      // constant 0x1F8
        water_manager,       // constant 0x201
        coin_locker_key_manager,         // constant 0x395
        coin_locker_key,         // constant 0x396
        qrcord_manager,      // constant 0x3AF
        qrcord,      // constant 0x3B0
        clock_pkg_holder,        // constant 0x206
        ladder,      // constant 0x20B
        ladder_manager,      // constant 0x218
        pickup_dispose_manager,      // constant 0x3B7
        pickup_diapose_base,         // constant 0x385
        skill_dispose,       // constant 0x386
        item_dispose,        // constant 0x257
        item_dispose_for_drone,      // constant 0x3EB
        item_dispose_secret_medicine,        // constant 0x3EE
        stage_area_monitoring,       // constant 0x2B0
        escalator_manager,       // constant 0x3B9
        escalator_base,      // constant 0x3AA
        escalator_stairs,        // constant 0x3AB
        escalator_footpath,      // constant 0x3AC
        mg_snack_manager,        // constant 0x217
        mg_snack_talk_player,        // constant 0x21A
        mg_snack_normal_info,        // constant 0x232
        mg_snack_normal_talk_result,         // constant 0x22E
        mg_snack_normal_talk_rankup,         // constant 0x26A
        mg_snack_event_talk,         // constant 0x26C
        mg_snack_event_talk_result,      // constant 0x235
        mg_toylets_manager,      // constant 0x30E
        mg_golf_driving_range,       // constant 0x342
        mg_darts_manager,        // constant 0x21E
        minigame_mahjong,        // constant 0x229
        mahjong_camera,      // constant 0x22A
        mg_karaoke_manager,      // constant 0x238
        minigame_sandlot_baseball_face_picture,      // constant 0x247
        ui_telop,        // constant 0x36A
        minigame_photo_shooting_mission_manager,         // constant 0x3C6
        minigame_photo_shooting_mission_entity,      // constant 0x3C7
        minigame_photo_shooting_mission_character,       // constant 0x3C8
        photo_mission_dispose_manager,       // constant 0x3CB
        photo_spot_entity,       // constant 0x3CC
        sugoroku_masu_entity,        // constant 0x36C
        sugoroku_masu_manager,       // constant 0x36D
        sugoroku_manager_entity,         // constant 0x37A
        sugoroku_effect_entity,      // constant 0x3A3
        sugoroku_effect_manager,         // constant 0x3A4
        sugoroku_dispose_point,      // constant 0x3C4
        sugoroku_dispose_point_manager,      // constant 0x3C5
        sugoroku_break_asset,        // constant 0x3D0
        sugoroku_break_asset_manager,        // constant 0x3D1
        effect_only_one_manager,         // constant 0x255
        mg_baseball_picture_holder,      // constant 0x26B
        minigame_hitting_controller,         // constant 0x29B
        minigame_hitting_effect_ball_track,      // constant 0x38D
        minigame_sandlot_baseball_manager,       // constant 0x270
        minigame_sandlot_baseball_menu_manager,      // constant 0x28D
        minigame_batting_center_manager,         // constant 0x2A4
        minigame_batting_center_movie_player,        // constant 0x375
        mg_offertory_box_entity,         // constant 0x280
        mg_offertory_box_camera,         // constant 0x281
        minigame_segaages,       // constant 0x2A3
        battle_tutorial,         // constant 0x2BB
        reward_manager,      // constant 0x2BE
        taiken_timer,        // constant 0x2C2
        minigame_cabaret_island_manager,         // constant 0x2E3
        minigame_blackjack,      // constant 0x2EC
        camera_blackjack,        // constant 0x2ED
        minigame_poker,      // constant 0x31A
        camera_poker,        // constant 0x31B
        lock_picking_manager,        // constant 0x32D
        file_port_manager,       // constant 0x2E6
        drone_manager,       // constant 0x2EA
        drone,       // constant 0x2EB
        drone_checkpoint,        // constant 0x2F5
        drone_navigation,        // constant 0x2F6
        drone_navigation_sub,        // constant 0x38B
        drone_startpos,      // constant 0x310
        drone_gate,      // constant 0x339
        drone_ui_select_course,      // constant 0x398
        drone_ui_select_ghost,       // constant 0x3C3
        drone_honeycomb,         // constant 0x39F
        drone_bee,       // constant 0x3A1
        drone_online_manager,        // constant 0x3C9
        stalking_manager,        // constant 0x2F2
        cover_point_manager,         // constant 0x2FF
        cover_point,         // constant 0x300
        chase_manager,       // constant 0x1C9
        run_trick_point_manager,         // constant 0x331
        run_trick_point,         // constant 0x332
        talk_text_man,       // constant 0x2F7
        camera_cabaret_island,       // constant 0x2FE
        window_light_dispose,        // constant 0x303
        fall_snow_dispose,       // constant 0x305
        fall_rain_dispose,       // constant 0x308
        projection_pool_dispose,         // constant 0x31D
        room_dust_dispose,       // constant 0x33F
        projection_light_dispose,        // constant 0x391
        effect_corn_light_dispose,       // constant 0x3BD
        effect_rain_fog_dispose,         // constant 0x3DE
        window_rain_dispose,         // constant 0x3E5
        window_rain_global_param_dispose,        // constant 0x3E8
        mg_shogi_entity,         // constant 0x312
        mg_shogi_asset_entity,       // constant 0x33D
        wrapping_collision_manager,      // constant 0x316
        wrapping_collision,      // constant 0x317
        minigame_hanafuda,       // constant 0x322
        hanafuda_camera,         // constant 0x329
        physics_world,       // constant 0x32A
        uniq_auth_manager,       // constant 0x330
        qsearch_manager,         // constant 0x335
        ufo_catcher_manager,         // constant 0x334
        ufo_catcher_world,       // constant 0x336
        friend_gauge_man,        // constant 0x345
        gimmick_juming_spear,        // constant 0x347
        gimmick_tougijo_fire,        // constant 0x355
        unlocking_adv_man,       // constant 0x349
        render_texture_drone,        // constant 0x34C
        minigame_oichokabu,      // constant 0x359
        oichokabu_camera,        // constant 0x35A
        onedari_manager,         // constant 0x35E
        fish_generate,       // constant 0x39D
        fish_generate_manager,       // constant 0x39E
        cloud_funding_building,      // constant 0x3B5
        scene_minigame_pinball,      // constant 0x3CD
        minigame_pinball,        // constant 0x3CE
        scene_minigame_hades,        // constant 0x46C
        scene_minigame_milligod,         // constant 0x487
        scene_minigame_souten,       // constant 0x488
        scene_minigame_beast,        // constant 0x489
        minigame_pachinko,       // constant 0x46D
        minigame_photo_shooting_mission_ui,      // constant 0x3CF
        minigame_decorating_office_manager,      // constant 0x3D2
        decorating_office_dispose_manager,       // constant 0x3D3
        decorating_area_entity,      // constant 0x3D4
        minigame_decorating_office_ui,       // constant 0x3D6
        dialog_information_manager,      // constant 0x3D8
        decisive_moment,         // constant 0x3DD
        effect_static_roof_mask_dispose,         // constant 0x3E7
        sp_provider,         // constant 0x3EC
        minigame_photo_shooting_mission_smartphone_accessor,         // constant 0x3F1
        tss_manager,         // constant 0x3F3
        tutorial_entity,         // constant 0x3FA
        staffroll_entity,        // constant 0x3FD
        ui_popup_number_manager,         // constant 0x40E
        dragon_cart_race_manager,        // constant 0x402
        dragon_cart,         // constant 0x403
        camera_dragon_cart,      // constant 0x404
        camera_dragon_cart_menu,         // constant 0x47E
        dragon_cart_entity_base,         // constant 0x44F
        dragon_cart_dispose_base,        // constant 0x412
        dragon_cart_dispose_itembox,         // constant 0x407
        dragon_cart_dispose_coin,        // constant 0x408
        dragon_cart_entity_crash_ring,       // constant 0x4A5
        dragon_cart_dispose_slope,       // constant 0x415
        dragon_cart_dispose_checkpoint_range,        // constant 0x419
        dragon_cart_dispose_mine_roomba,         // constant 0x41E
        dragon_cart_dispose_accel_pad,       // constant 0x41F
        dragon_cart_dispose_dart,        // constant 0x421
        dragon_cart_dispose_navigation,      // constant 0x423
        dragon_cart_dispose_navigation_ignore_range,         // constant 0x485
        dragon_cart_dispose_lotion_range,        // constant 0x490
        dragon_cart_dispose_upper_injection,         // constant 0x49A
        dragon_cart_entity_navigation_close_range,       // constant 0x486
        dragon_cart_entity_rocket,       // constant 0x426
        dragon_cart_entity_bullet,       // constant 0x433
        dragon_cart_entity_sky_bombing,      // constant 0x43B
        dragon_cart_entity_lotion_bottle,        // constant 0x43D
        dragon_cart_entity_lotion_range,         // constant 0x43E
        dragon_cart_specular_reflection,         // constant 0x450
        dragon_cart_dispose_item_respawn_range,      // constant 0x464
        dragon_cart_dispose_blast_drum,      // constant 0x47C
        dragon_cart_dispose_course_parts_base,       // constant 0x47F
        dragon_cart_dispose_course_parts_corner,         // constant 0x480
        dragon_cart_dispose_course_parts_box,        // constant 0x481
        dragon_cart_dispose_course_parts_triangle_wall,      // constant 0x482
        dragon_cart_dispose_courseout_range,         // constant 0x484
        vehicle_train_entity,        // constant 0x406
        effect_grain_noise,      // constant 0x41D
        effect_distortion,       // constant 0x42A
        mg_meigaza_manager,      // constant 0x410
        party_equip_entity,      // constant 0x420
        mg_homeless_manager,         // constant 0x427
        mg_homeless_can,         // constant 0x428
        mg_homeless_character,       // constant 0x438
        mg_homeless_player,      // constant 0x439
        mg_homeless_rival,       // constant 0x43A
        mg_homeless_map,         // constant 0x444
        mg_homeless_map_creater,         // constant 0x445
        mg_homeless_navi_map,        // constant 0x446
        mg_homeless_truck,       // constant 0x447
        mg_homeless_camera,      // constant 0x44A
        mg_homeless_anpan,       // constant 0x45B
        mg_homeless_lucky_npc,       // constant 0x460
        mg_homeless_character_talk,      // constant 0x496
        mg_homeless_can_spot,        // constant 0x4A0
        mg_homeless_can_line,        // constant 0x4A1
        talk_party_chat_manager,         // constant 0x42F
        talk_party_talk_manager,         // constant 0x440
        talk_notice_gauge_manager,       // constant 0x46F
        talk_trust_param_manager,        // constant 0x477
        talk_adv_hp_gauge_manager,       // constant 0x4A2
        adv_battle_gauge_manager,        // constant 0x4AA
        planter_dispose,         // constant 0x462
        planter_man,         // constant 0x466
        ui_create_item,      // constant 0x48A
        taxi_travel,         // constant 0x48F
        cabaret_yoasobi_man,         // constant 0x49D
        upstart_manager,         // constant 0x459
        light_ev_char,       // constant 0x453
        sea_debris_dispose,      // constant 0x454
        player_personality,      // constant 0x45F
        entity_effect_blast,         // constant 0x461
        score_ranking_manager,       // constant 0x463
        grass_dispose,       // constant 0x46B
        grass_area,      // constant 0x46E
        grass_draw_manager,      // constant 0x474
        crowd_tile_manager,      // constant 0x48B
        crowd_tile_group,        // constant 0x48C
        pause_auth_renderer_entity,      // constant 0x473
        collection_spot,         // constant 0x470
        speech_subtitle_manager,         // constant 0x47B
        picked_item,         // constant 0x483
        minigame_shikaku_manager,        // constant 0x498
        minigame_shikaku_main_manager,       // constant 0x49C
        smelting_manager,        // constant 0x49F
        planting_collision_manager,      // constant 0x4A7
        dlc_manager,         // constant 0x4A8
        option_input_entity,         // constant 0x4AC
        num,         // constant 0x4AF
    };
#endif
    }
