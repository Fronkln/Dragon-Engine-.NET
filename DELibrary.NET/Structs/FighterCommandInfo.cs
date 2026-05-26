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
        public IntPtr IdPtr;
        public string Id { get { return Marshal.PtrToStringAnsi(IdPtr); } }

        //Yes... This is very bad
        //But it works, damn it!
        public IntPtr AnimationTableStart { get {  return IdPtr + Id.Length + 1; } }
        public string Animation { get { return Marshal.PtrToStringAnsi (AnimationTableStart); } }

        public uint GmtID;
    }
}
