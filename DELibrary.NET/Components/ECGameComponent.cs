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
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_GAME_COMPONENT_GETTER_OWNER", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr DELib_Game_Component_Getter_Owner(IntPtr component);

        public virtual GameObject Owner
        {
            get
            {
                GameObject ent = new GameObject();
                ent._objectAddress = DELib_Game_Component_Getter_Owner(_objectAddress);

                return ent;
            }
        }
    }
}
