#pragma once
// compat shims for addon libs that target older imgui versions
#include "imgui.h"

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
