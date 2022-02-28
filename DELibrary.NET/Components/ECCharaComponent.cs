using System;


namespace DragonEngineLibrary
{
    public class ECCharaComponent : ECCharaBaseComponent
    {
        /// <summary>
        /// Return the owner as base character
        /// </summary>
        public new Character Owner
        {
            get
            {
                return new EntityHandle<Character>(base.Owner.UID);
            }
        }
    }
}
