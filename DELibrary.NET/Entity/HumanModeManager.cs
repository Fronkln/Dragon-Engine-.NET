using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class HumanModeManager
    {

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_GETTER_HUMAN", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HumanModeManager_Getter_Human(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOREADY", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_ToReady(IntPtr manager, bool no_blend);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HumanModeManager_To(IntPtr manager, IntPtr mode);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOATTACKMODE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToAttackMode(IntPtr manager, FighterCommandID fighterCommand);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOACTIONCOMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToActionCommand(IntPtr manager, FighterCommandID fighterCommand);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOEQUIPWEAPON", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToEquipWeapon(IntPtr manager, AssetID weapon);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOSTAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToStand(IntPtr manager, RequireType reqType);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOENDREADY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToEndReady(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOKAMAE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToKamae(IntPtr manager, bool no_blend, bool force_kamae, bool no_start_req);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOMOVE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToMove(IntPtr manager, bool no_blend);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOSWAY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToSway(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOBATTOU", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToBattou(IntPtr manager, AssetID id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISDOWN", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsDown(IntPtr manager);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISSTANDUP", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsStandup(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_GETTER_COMMANDSETMODEL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HumanModeManager_Getter_CommandsetModel(IntPtr manager);

        public enum RequireType
        {
            Normal = 0x0,
            Immediate = 0x1,
            Blend = 0x2,
        };

        public IntPtr Pointer;

        public Character Human
        {
            get
            {
                return new Character() { Pointer = DELib_HumanModeManager_Getter_Human(Pointer) };
            }
        }

        ///<summary>Commandset model responsible for fighter command related operations.</summary>
        public CommandSetModel CommandsetModel
        {
            get
            {
                return new CommandSetModel()
                {
                    _ptr = DELib_HumanModeManager_Getter_CommandsetModel(Pointer)

                };
            }
        }

        ///<summary>Don't use.</summary>
        public void To(HumanMode mode)
        {
            DELib_HumanModeManager_To(Pointer, mode.m_pointer);
        }

        ///<summary>Execute a Fighter Command attack.</summary>
        public void ToAttackMode(FighterCommandID inf)
        {
#if YLAD
            DELib_HumanModeManager_ToActionCommand(Pointer, inf);
#else
            DELib_HumanModeManager_ToAttackMode(Pointer, inf);
#endif
        }

        ///<summary>Equip a weapon.</summary>
        public void ToEquipWeapon(AssetID weapon)
        {
            DELib_HumanModeManager_ToEquipWeapon(Pointer, weapon);
        }

        ///<summary>"Readies" the character?.</summary>
        public bool ToReady(bool no_blend) { return DELib_HumanModeManager_ToReady(Pointer, no_blend); }
        ///<summary>Ends the current human mode.</summary>
        public void ToEndReady() => DELib_HumanModeManager_ToEndReady(Pointer);
        ///<summary>"Stands" the character?.</summary>
        public void ToStand(RequireType reqType) => DELib_HumanModeManager_ToStand(Pointer, reqType);
        ///<summary>Activate battle stance.</summary>
        public void ToKamae(bool no_blend, bool force_kamae, bool no_start_req) => DELib_HumanModeManager_ToKamae(Pointer, no_blend, force_kamae, no_start_req);
        ///<summary>Don't know what this does.</summary>
        public void ToMove(bool no_blend) => DELib_HumanModeManager_ToMove(Pointer, no_blend);
        ///<summary>Makes the character sidestep.</summary>
        public void ToSway() =>  DELib_HumanModeManager_ToSway(Pointer);
        ///<summary>Equip a weapon.</summary>
        public void ToBattou(AssetID asset) => DELib_HumanModeManager_ToBattou(Pointer, asset);

        ///<summary>Are we knocked down?</summary>
        public bool IsDown() { return DELib_HumanModeManager_IsDown(Pointer); }
        ///<summary>Are we getting up?</summary>
        public bool IsStandup() { return DELib_HumanModeManager_IsStandup(Pointer); }
    }
}
