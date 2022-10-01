using DragonEngineLibrary;

namespace Brawler
{
    [System.Serializable]
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
