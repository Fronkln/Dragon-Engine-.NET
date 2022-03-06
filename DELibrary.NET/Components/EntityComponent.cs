﻿using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class EntityComponent : CTask
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_COMPONENT_GETTER_OWNER", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_Entity_Component_Getter_Owner(IntPtr component);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_COMPONENT_RELEASE_ENTITY_COMPONENT", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_Entity_Component_ReleaseEntityComponent(IntPtr component, bool b_remove_parent_handle, bool b_immediate);

        public enum ECSlotID : ushort
        {

            invalid,         // constant 0x0
            basic,       // constant 0x1
            scene_callback,      // constant 0x2
            render_mesh,         // constant 0x3
            render_ui_texture,       // constant 0x4
            oct_tree_register,       // constant 0x5
            oct_tree_collect,        // constant 0x6
            oct_tree_collect_character,      // constant 0x7
            oct_tree_collect_asset,      // constant 0x8
            oct_tree_collect_asset_turn_face,        // constant 0x9
            oct_tree_collect_asset_pickup,       // constant 0xA
            oct_tree_collect_asset_have_physics_component,       // constant 0xB
            oct_tree_collect_asset_under_ready,      // constant 0xC
            oct_tree_collect_obstacle,       // constant 0xD
            collision_collect_camera,        // constant 0xE
            controller_pad_view_local,       // constant 0xF
            controller_pad,      // constant 0x10
            controller_pad2,         // constant 0x11
            controller_pad_tutorial,         // constant 0x12
            controller_process,      // constant 0x13
            ik_control,      // constant 0x14
            jaunt,       // constant 0x15
            posture,         // constant 0x16
            swathe,      // constant 0x17
            sup,         // constant 0x18
            motion_fullbody_ik,      // constant 0x19
            eMotion,         // constant 0x1A
            face_anim,       // constant 0x1B
            cloth_manager,       // constant 0x1C
            motion_turn_face,        // constant 0x1D
            move_push_back,      // constant 0x1E
            asset_program_category_base,         // constant 0x1F
            asset_geometry_base,         // constant 0x20
            asset_arms,      // constant 0x21
            asset_glass,         // constant 0x22
            asset_light,         // constant 0x23
            asset_motion,        // constant 0x24
            asset_texture_replace,       // constant 0x25
            asset_se,        // constant 0x26
            asset_particle,      // constant 0x27
            asset_particle_general,      // constant 0x28
            asset_effect_special,        // constant 0x29
            asset_shadow_occlusion,      // constant 0x2A
            asset_preset_child,      // constant 0x2B
            asset_physics_base,      // constant 0x2C
            asset_collision,         // constant 0x2D
            asset_simple_animation,      // constant 0x2E
            asset_durability,        // constant 0x2F
            asset_infinite_arms,         // constant 0x30
            asset_range_preset,      // constant 0x31
            cloth_controller,        // constant 0x32
            physics_character_proxy,         // constant 0x33
            physics_ragdoll,         // constant 0x34
            physics_posture,         // constant 0x35
            ccc,         // constant 0x36
            camera_basic,        // constant 0x37
            play_camera_motion,      // constant 0x38
            effect_rim_flash,        // constant 0x39
            effect_body_damage,      // constant 0x3A
            effect_body_wet,         // constant 0x3B
            effect_fur,      // constant 0x3C
            effect_hit_wound,        // constant 0x3D
            aii,         // constant 0x3E
            mind_control_sensor,         // constant 0x3F
            play_simple_motion,      // constant 0x40
            body_coordinator,        // constant 0x41
            light_color_anim,        // constant 0x42
            street_checker,      // constant 0x43
            team_connecter,      // constant 0x44
            soldier_info,        // constant 0x45
            battle_target_decide,        // constant 0x46
            battle_status,       // constant 0x47
            battle_collision,        // constant 0x48
            uii,         // constant 0x49
            ui_life_gauge,       // constant 0x4A
            ui_enemy_life_gauge,         // constant 0x4B
            life_gauge_sp_hit,       // constant 0x4C
            reflex_sensor,       // constant 0x4D
            character_se,        // constant 0x4E
            character_speech,        // constant 0x4F
            character_kuchipaku,         // constant 0x50
            character_particle_foot,         // constant 0x51
            character_particle_ground,       // constant 0x52
            character_particle_ground_bath,      // constant 0x53
            character_particle_ground_sea,       // constant 0x54
            character_particle_ground_dream,         // constant 0x55
            character_particle_blood,        // constant 0x56
            character_particle_bush,         // constant 0x57
            character_effect_event,      // constant 0x58
            character_visible_ratio,         // constant 0x59
            player_state,        // constant 0x5A
            collision_collect_hact,      // constant 0x5B
            haact,       // constant 0x5C
            instant_chat,        // constant 0x5D
            speed_meter,         // constant 0x5E
            map_icon,        // constant 0x5F
            navigate,        // constant 0x60
            scene_ext_callback,      // constant 0x61
            save_data_callback,      // constant 0x62
            system_data_callback,        // constant 0x63
            camera_shake,        // constant 0x64
            parts_action_coordinator,        // constant 0x65
            auth_talk_replace,       // constant 0x66
            auth_folder_condition_prg_flg,       // constant 0x67
            auth_hact_condition,         // constant 0x68
            auth_talk_condition,         // constant 0x69
            ui_stamina_gauge,        // constant 0x6A
            ccc_icon,        // constant 0x6B
            pause_auth_renderer_entity_component,        // constant 0x6C
            map_model_renderer_entity_component,         // constant 0x6D
            eat_gauge,       // constant 0x6E
            ui_player_equip,         // constant 0x6F
            character_suspend,       // constant 0x70
            bool_void_db_callback,       // constant 0x71
            void_void_db_callback,       // constant 0x72
            camera_chain,        // constant 0x73
            mesh_color_change,       // constant 0x74
            swing_tree,      // constant 0x75
            movement,        // constant 0x76
            spatial_perception,      // constant 0x77
            auth_cinema_caption,         // constant 0x78
            black_board,         // constant 0x79
            artisan,         // constant 0x7A
            touch_assist,        // constant 0x7B
            attachment,      // constant 0x7C
            status,      // constant 0x7D
            behavior,        // constant 0x7E
            constructor,         // constant 0x7F
            sensor,      // constant 0x80
            Friend,      // constant 0x81
            unsafe_area_controller,      // constant 0x82
            icon,        // constant 0x83
            ui_controller,       // constant 0x84
            positiion,       // constant 0x85
            generator,       // constant 0x86
            look_conductor,      // constant 0x87
            deform,      // constant 0x88
            life_controller,         // constant 0x89
            auth_fill_light_play,        // constant 0x8A
            npc_control_auto_generator,      // constant 0x8B
            sensor_sight,        // constant 0x8C
            sensor_detector,         // constant 0x8D
            steering_behaviors,      // constant 0x8E
            recognition_collision,       // constant 0x8F
            check_box_list_checker,      // constant 0x90
            backwork_check_box_list_checker,         // constant 0x91
            characteristic_checker,      // constant 0x92
            verification_2d,         // constant 0x93
            verification_3d,         // constant 0x94
            agent,       // constant 0x95
            character_collision_controller,      // constant 0x96
            character_supporter,         // constant 0x97
            character_face_pattern_coordinator,      // constant 0x98
            character_turn_battle_ai,        // constant 0x99
            character_collision_dynamic_gct,         // constant 0x9A
            auth_pause_accessor,         // constant 0x9B
            character_bush_shaker,       // constant 0x9C
            selection,       // constant 0x9D
            controller_mouse,        // constant 0x9E
            tool_comm,       // constant 0x9F
            tool_com_port,       // constant 0xA0
            dispose,         // constant 0xA1
            mouse_select,        // constant 0xA2
            camera_debug,        // constant 0xA3
            debug_picker,        // constant 0xA4
            debug_pointing,      // constant 0xA5
            editor_posture,      // constant 0xA6
            motion_eating,       // constant 0xA7
            effect_glass_water,      // constant 0xA8
            effect_glass_water_wait_ready,       // constant 0xA9
            effect_sea_cap,      // constant 0xAA
            ik_selfy,        // constant 0xAB
            transform,       // constant 0xAC
            ec_test1,        // constant 0xAD
            ec_test2,        // constant 0xAE
            ec_test3,        // constant 0xAF
            effect_charge_dust,      // constant 0xB0
            effect_body_rain_splash,         // constant 0xB1
            orbox_visible_ratio,         // constant 0xB2
            asset_visible_ratio,         // constant 0xB3
            ec_timer_component,      // constant 0xB4
            ec_collision_segment_trace,      // constant 0xB5
            ec_caution,      // constant 0xB6
            ec_visible_screen_range,         // constant 0xB7
            ec_effect_highlight,         // constant 0xB8
            ec_npc_popup,        // constant 0xB9
            ec_tutorial_ui,      // constant 0xBA
            ec_progress_ui,      // constant 0xBB
            ec_character_replace,        // constant 0xBC
            ec_following_timer_component,        // constant 0xBD
            scene_select_pause_abort,        // constant 0xBE
            ec_scene_cloth_change,       // constant 0xBF
            motion_animal_ik,        // constant 0xC0
            ec_photo_stickers,       // constant 0xC1
            ec_slot_num,         // constant 0xC2
        };

        public virtual EntityBase Owner
        {
            get
            {
                EntityBase ent = new EntityBase();
                ent._objectAddress = DELib_Entity_Component_Getter_Owner(_objectAddress);

                return ent;
            }
        }

        public bool DestroyComponent(bool b_remove_parent_handle, bool b_immediate)
        {
            return DELib_Entity_Component_ReleaseEntityComponent(Pointer, b_remove_parent_handle, b_immediate);
        }
    }
}
