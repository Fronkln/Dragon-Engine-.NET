using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public struct GameTick
    {
        public uint Tick;
        public float Frame
        {
            get
            {
                return AuthPlay.TickToFrame(Tick);
            }
            set
            {
                Tick = AuthPlay.FrameToTick(value);
            }
        }

        public GameTick(uint tick)
        {
            Tick = tick;
        }


        public GameTick(float frame)
        {
            Tick = AuthPlay.FrameToTick(frame);
        }

        public override string ToString()
        {
            return Tick.ToString();
        }
    }
}
