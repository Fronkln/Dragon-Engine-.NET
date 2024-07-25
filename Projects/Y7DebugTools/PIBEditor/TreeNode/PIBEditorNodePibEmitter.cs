using PIBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y7DebugTools
{
    internal class PIBEditorNodePibEmitter : TreeNode
    {
        public BasePibEmitter Emitter;

        public PIBEditorNodePibEmitter(BasePibEmitter emitter)
        {
            Emitter = emitter;
            Text = $"Emitter ({emitter.GetEmitterType()})";
        }
    }
}
