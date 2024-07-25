using PIBLib;
using System;

namespace Y7DebugTools
{
    internal class PIBEditorNodePib : TreeNode
    {
        public BasePib Pib;

        public PIBEditorNodePib(BasePib pib)
        {
            Pib = pib;

            foreach(BasePibEmitter emitter in pib.Emitters)
            {
                AddChild(new PIBEditorNodePibEmitter(emitter));
            }
        }

        public void AddChild(TreeNode child)
        {
            if(child.Parent != null)
                child.Parent.Children.Remove(child);

            child.Parent = this;
            Children.Add(child);
        }

    }
}
