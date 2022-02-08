using System;


namespace DragonEngineLibrary
{
    public class ECCharaComponent : ECCharaBaseComponent
    {
        /// <summary>
        /// Return the owner as base character
        /// </summary>
        public Character OwnerCharacter
        {
            get
            {
                return (Character)Owner;
            }
        }
    }
}
