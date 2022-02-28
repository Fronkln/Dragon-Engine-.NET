using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECGameComponent : EntityComponent
    {
        public new virtual GameObject Owner
        {
            get
            {
                return new EntityHandle<GameObject>(base.Owner.UID);
            }
        }
    }
}
