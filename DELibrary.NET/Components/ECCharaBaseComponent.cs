using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public class ECCharaBaseComponent : ECGameComponent
    {
        /// <summary>
        /// Return the owner as base character
        /// </summary>
        public new CharacterBase Owner
        {
            get
            {
                return new EntityHandle<CharacterBase>(base.Owner.UID);
            }
        }
    }
}
