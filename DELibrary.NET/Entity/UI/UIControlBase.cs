using DragonEngineLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public unsafe class UIControlBase : UnsafeObject
    {
        public UIPlayerBase GetPlayer()
        {
            ulong* addr = (ulong*)(Pointer.ToInt64() + 0xC8);
            return new UIPlayerBase((IntPtr)(*addr));
        }
    }
}
