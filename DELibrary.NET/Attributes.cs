using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{

    [AttributeUsage(AttributeTargets.Assembly)]
    public class DEModInfo : Attribute
    {
        public string ModName;
        public Type InitializationClass;

        public DEModInfo(string modName, Type modInitializationClass)
        {
            ModName = modName;
            InitializationClass = modInitializationClass;
        }
    }
}
