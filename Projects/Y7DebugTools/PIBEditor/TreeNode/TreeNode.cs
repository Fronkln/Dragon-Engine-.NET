using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y7DebugTools
{
    internal class TreeNode
    {
        public TreeNode Parent;
        public List<TreeNode> Children = new List<TreeNode>();

        public string Text;
    }
}
