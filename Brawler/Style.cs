using DragonEngineLibrary;

namespace Brawler
{
    public class Style
    {
        public MotionID SwapAnimation;
        public Moveset CommandSet;

        public Style(MotionID swapanim, Moveset moves)
        {
            SwapAnimation = swapanim;
            CommandSet = moves;
        }
    }
}
