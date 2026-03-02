using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Node
    {
        public uint Value;
        public Node* Previous;
        public Node* Next;
    }

    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public unsafe struct PXDNodeList
    {
        public Node* Top;
        public Node* Bottom;

        public List<uint> GetList()
        {
            List<uint> values = new List<uint>();

            for (Node* i = Top; i != null; i = i->Next)
            {
                values.Add(i->Value);
            }

            return values;
        }
    }
}
