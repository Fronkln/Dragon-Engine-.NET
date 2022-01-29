using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public enum DEJob
    {
        GameVarUpdate = 0x0,
        SystemNormal = 0x1,
        DisposeParse = 0x2,
        DisposeLoad = 0x3,
        SceneJobRegister = 0x4,
        EditorUpdate = 0x5,
        EntityJobRegister = 0x6,
        StaticCollisionUpdate = 0x7,
        StaticCollisionFeedback = 0x8,
        PreUpdate = 0x9,
        CCCUpdate = 0xA,
        OctTreeUpdate = 0xB,
        Update = 0xC,
        UpdateCharacter = 0xD,
        PostUpdate = 0xE,
        PhysicsSetup = 0xF,
        Physics = 0x10,
        PhysicsFeedback = 0x11,
        UpdateCharacterAfterPhysics = 0x12,
        Camera = 0x13,
        CameraFeedback = 0x14,
        UpdateUI = 0x15,
        DrawSetup = 0x16,
        Draw = 0x17,
        DebugDraw = 0x18,
        Render = 0x19,
        Cleanup = 0x1A,
        Number = 0x1B,
        Unstable = 0xFD,
        Dynamic = 0xFE,
        Unknown = 0xFF,
    };
}
