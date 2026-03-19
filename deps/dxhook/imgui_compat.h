#pragma once
// compat shims for addon libs that target older imgui versions
#include "imgui.h"
#include "imgui_internal.h"

#ifndef ImGuiKeyModFlags
typedef ImGuiKeyChord ImGuiKeyModFlags;
enum ImGuiKeyModFlags_ {
    ImGuiKeyModFlags_None  = 0,
    ImGuiKeyModFlags_Ctrl  = ImGuiMod_Ctrl,
    ImGuiKeyModFlags_Shift = ImGuiMod_Shift,
    ImGuiKeyModFlags_Alt   = ImGuiMod_Alt,
    ImGuiKeyModFlags_Super = ImGuiMod_Super
};
#endif

// _c POD struct typedefs - the cimgui generator (1.92.5+) emits these for
// pass-by-value params in addon wrappers (cimguizmo, cimplot, cimnodes, cimplot3d).
// When building against cimgui 1.92.4 (which doesn't define them), we provide them here.
#ifndef CIMGUI_DEFINE_ENUMS_AND_STRUCTS
// Only define when NOT using cimgui's own enum/struct block (i.e., compiling as C++)
#ifndef ImVec2_c_defined
#define ImVec2_c_defined
typedef struct ImVec2_c { float x, y; } ImVec2_c;
#endif
#ifndef ImVec4_c_defined
#define ImVec4_c_defined
typedef struct ImVec4_c { float x, y, z, w; } ImVec4_c;
#endif
#ifndef ImRect_c_defined
#define ImRect_c_defined
typedef struct ImRect_c { ImVec2_c Min; ImVec2_c Max; } ImRect_c;
#endif
#ifndef ImTextureRef_c_defined
#define ImTextureRef_c_defined
typedef struct ImTextureRef_c { ImTextureData* _TexData; ImTextureID _TexID; } ImTextureRef_c;
#endif
#endif

// CaptureMouseFromApp was renamed to SetNextFrameWantCaptureMouse in 1.89.x and removed in 1.92.x
#if IMGUI_VERSION_NUM >= 19200
namespace ImGui {
    inline void CaptureMouseFromApp(bool want_capture_mouse = true) {
        SetNextFrameWantCaptureMouse(want_capture_mouse);
    }
}
#endif
