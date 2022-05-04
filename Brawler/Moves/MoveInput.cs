using System;
using DragonEngineLibrary;

namespace Brawler
{
    public struct MoveInput
    {
        public VirtualKey Key;
        public bool Hold;

        public MoveInput(VirtualKey key, bool hold)
        {
            Key = key;
            Hold = hold;
        }
    }
}
