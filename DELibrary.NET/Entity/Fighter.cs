using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    /// <summary>
    /// Fighter objects only exist on combat.
    /// </summary>
    public class Fighter
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_FIGHTER_GETTER_CHARACTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_Fighter_Getter_Character(IntPtr fighterPtr);

        public Character Character { get; internal set; }
        internal IntPtr _ptr;

        internal Fighter(IntPtr pointer)
        {
            _ptr = pointer;

            //do PInvoke once. i doubt the character pointer of a fighter will ever change.
            Character = new Character();
            Character._objectAddress = DELib_Fighter_Getter_Character(_ptr);
        }

        public bool IsPlayer()
        {
            return Character.Attributes.is_player;
        }


        /*
        //TODO, do the calculations from C# instead of calling a native func
        //When the time comes that is
        public bool IsEnemy()
        {
    
        }

        public bool IsAlly()
        {
            return !IsPlayer() && !IsEnemy();
        }
        */

        public FighterID GetID()
        {
            FighterID id = new FighterID();
            id.Handle = Character;

            return id;
        }
    }

    public struct FighterID
    {
        public EntityHandle<Character> Handle;
    }
}
