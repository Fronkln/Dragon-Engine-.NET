using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public struct FighterCommandInfo
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string Id;
    }
}
