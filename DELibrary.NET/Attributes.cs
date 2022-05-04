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

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DECompatibility : Attribute
    {
        public DEGames compatibleGames;

        public DECompatibility(DEGames compatibility)
        {
            compatibleGames = compatibility;
        }
    }
}
