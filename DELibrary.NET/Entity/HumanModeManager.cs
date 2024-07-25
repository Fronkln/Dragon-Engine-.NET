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


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOPICKUP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToPickup(IntPtr manager, IntPtr asset);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOBATTOU", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToBattou(IntPtr manager, AssetID id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISDOWN", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsDown(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISMOVE", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsMove(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISDAMAGE", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsDamage(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISATTACK", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsAttack(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISINPUTMOVE", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsInputMove(IntPtr manager);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISSTANDUP", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsStandup(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISWALKING", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsWalking(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISRUNNING", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsRunning(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISGUARDING", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsGuarding(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISPICKUP", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsPickup(IntPtr manager);
        
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_ISSYNC", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HumanModeManager_IsSync(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_GETTER_COMMANDSETMODEL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HumanModeManager_Getter_CommandsetModel(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_GETTER_TRANSITCOMMANDMODEL", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HumanModeManager_Getter_TransitCommandModel(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_GETTER_MODENAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HumanModeManager_Getter_ModeName(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_GETTER_CURRENTMODE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HumanModeManager_Getter_CurrentMode(IntPtr manager);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_GETTER_NEXTMODE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HumanModeManager_Getter_NextMode(IntPtr manager);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_CHANGEMODE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ChangeMode(IntPtr manager);


#if IW_AND_UP
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HUMANMODEMANAGER_TOSTYLECHANGE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HumanModeManager_ToStyleChange(IntPtr manager, int style);
#endif
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

        /// <summary>
        /// TransitCommand model responsible for checking fighter command commands.
        /// </summary>
        public TransitCommandModel TransitCommandModel
        {
            get
            {
                return new TransitCommandModel()
                {
                    _ptr = DELib_HumanModeManager_Getter_TransitCommandModel(Pointer)
                };
            }
        }


        public HumanMode CurrentMode
        {
            get
            {
                return new HumanMode() { m_pointer = DELib_HumanModeManager_Getter_CurrentMode(Pointer) };
            }
        }

        public HumanMode NextMode
        {
            get
            {
                return new HumanMode() { m_pointer = DELib_HumanModeManager_Getter_NextMode(Pointer) };
            }
        }

        public void ChangeMode()
        {
            DELib_HumanModeManager_ChangeMode(Pointer);
        }

        ///<summary>Don't use.</summary>
        public void To(HumanMode mode)
        {
            DELib_HumanModeManager_To(Pointer, mode.m_pointer);
        }

        ///<summary>Execute a Fighter Command attack.</summary>
        public void ToAttackMode(FighterCommandID inf)
        {
#if YLAD_AND_UP
            DELib_HumanModeManager_ToActionCommand(Pointer, inf);
#else
            DELib_HumanModeManager_ToAttackMode(Pointer, inf);
#endif
        }

        public bool IsInputMove()
        {
            return DELib_HumanModeManager_IsInputMove(Pointer);
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

        ///<summary>Makes the character pick up asset.</summary>
        public void ToPickup(AssetUnit asset) => DELib_HumanModeManager_ToPickup(Pointer, asset.Pointer);
        ///<summary>Equip a weapon.</summary>
        public void ToBattou(AssetID asset) => DELib_HumanModeManager_ToBattou(Pointer, asset);

        ///<summary>Are we knocked down?</summary>
        public bool IsDown() { return DELib_HumanModeManager_IsDown(Pointer); }
        ///<summary>Are we getting up?</summary>
        public bool IsStandup() { return DELib_HumanModeManager_IsStandup(Pointer); }

        ///<summary>Are we walking?</summary>
        public bool IsWalking() { return DELib_HumanModeManager_IsWalking(Pointer); }
        ///<summary>Are we running?</summary>
        public bool IsRunning() { return DELib_HumanModeManager_IsRunning(Pointer); }
        ///<summary>Are we guarding?</summary>
        public bool IsGuarding() { return DELib_HumanModeManager_IsGuarding(Pointer); }
        public bool IsPickup() { return DELib_HumanModeManager_IsPickup(Pointer); }

        /// <summary>
        /// Are we in a sync?
        /// </summary>
        /// <returns></returns>
        public bool IsSync() { return DELib_HumanModeManager_IsSync(Pointer); }

        public bool IsMove() { return DELib_HumanModeManager_IsMove(Pointer); }

        public bool IsAttack() { return DELib_HumanModeManager_IsAttack(Pointer); }

        public bool IsDamage() { return DELib_HumanModeManager_IsDamage(Pointer); }

        public unsafe void ToNormalStand()
        {
            IntPtr status = Human.Status.pointer;

            uint* flags = (uint*)(status.ToInt64() + 0x1150);
            uint flag = *flags;

            *flags = flag &= 0x1000;
        }

        public string GetCommandName()
        {
            return CurrentMode.GetCommandID().GetInfo().Id;
        }

#if IW
        public void ToStyleChange(int style)
        {
            DELib_HumanModeManager_ToStyleChange(Pointer, style);
        }
#endif
    }
}
