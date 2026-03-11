cmake_minimum_required(VERSION 3.15)
cmake_policy(SET CMP0091 NEW)
project(cimgui)

set(CMAKE_CXX_STANDARD 11)

set(CIMGUI_DIR ${CMAKE_CURRENT_SOURCE_DIR}/../ImGui.NET-nativebuild/cimgui)
set(IMGUI_DIR ${CIMGUI_DIR}/imgui)
set(MINHOOK_DIR ${CMAKE_CURRENT_SOURCE_DIR}/../minhook)
set(CIMPLOT_DIR ${CMAKE_CURRENT_SOURCE_DIR}/../cimplot)
set(CIMGUIZMO_DIR ${CMAKE_CURRENT_SOURCE_DIR}/../cimguizmo)
set(CIMNODES_DIR ${CMAKE_CURRENT_SOURCE_DIR}/../cimnodes)

# MinHook (C library - separate target, no force-includes)
add_library(minhook STATIC
    ${MINHOOK_DIR}/src/buffer.c
    ${MINHOOK_DIR}/src/hook.c
    ${MINHOOK_DIR}/src/trampoline.c
    ${MINHOOK_DIR}/src/hde/hde32.c
    ${MINHOOK_DIR}/src/hde/hde64.c
)
target_include_directories(minhook PUBLIC ${MINHOOK_DIR}/include ${MINHOOK_DIR}/src)

# cimgui shared library (C++ only)
add_library(cimgui SHARED
    # imgui core
    ${CIMGUI_DIR}/cimgui.cpp
    ${IMGUI_DIR}/imgui.cpp
    ${IMGUI_DIR}/imgui_draw.cpp
    ${IMGUI_DIR}/imgui_demo.cpp
    ${IMGUI_DIR}/imgui_widgets.cpp
    ${IMGUI_DIR}/imgui_tables.cpp
    # backends
    ${IMGUI_DIR}/backends/imgui_impl_dx11.cpp
    ${IMGUI_DIR}/backends/imgui_impl_dx12.cpp
    ${IMGUI_DIR}/backends/imgui_impl_win32.cpp
    # implot
    ${CIMPLOT_DIR}/cimplot.cpp
    ${CIMPLOT_DIR}/implot/implot.cpp
    ${CIMPLOT_DIR}/implot/implot_items.cpp
    ${CIMPLOT_DIR}/implot/implot_demo.cpp
    # ImGuizmo
    ${CIMGUIZMO_DIR}/cimguizmo.cpp
    ${CIMGUIZMO_DIR}/ImGuizmo/ImGuizmo.cpp
    # imnodes
    ${CIMNODES_DIR}/cimnodes.cpp
    ${CIMNODES_DIR}/imnodes/imnodes.cpp
    # DX11 hook
    ${CMAKE_CURRENT_SOURCE_DIR}/dx11_hook.cpp
)

target_compile_definitions(cimgui PUBLIC
    IMGUI_DEFINE_MATH_OPERATORS
    IMGUI_IMPL_API=extern\ \"C\"\ __declspec\(dllexport\)
)

target_include_directories(cimgui PUBLIC
    ${CIMGUI_DIR}
    ${IMGUI_DIR}
    ${IMGUI_DIR}/backends
    ${CIMPLOT_DIR}
    ${CIMPLOT_DIR}/implot
    ${CIMGUIZMO_DIR}
    ${CIMGUIZMO_DIR}/ImGuizmo
    ${CIMNODES_DIR}
    ${CIMNODES_DIR}/imnodes
)

# compat header for addon libs targeting older imgui (ImGuiKeyModFlags etc)
target_compile_options(cimgui PRIVATE /FI${CMAKE_CURRENT_SOURCE_DIR}/imgui_compat.h)

set_target_properties(cimgui PROPERTIES PREFIX "")
target_link_libraries(cimgui minhook imm32 d3d11 d3d12 dxgi)

install(TARGETS cimgui
    RUNTIME DESTINATION .
    LIBRARY DESTINATION .
    ARCHIVE DESTINATION .
)
