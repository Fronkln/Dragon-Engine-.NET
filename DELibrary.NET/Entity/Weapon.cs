using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public class Weapon
    {
        public EntityHandle<AssetUnit> Unit;

        public override string ToString()
        {
            AssetID id = Unit.Get().AssetID;
            return (int)id + $" ({Asset.GetArmsCategory(id)})";
        }
    }
}
