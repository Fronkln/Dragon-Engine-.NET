using DragonEngineLibrary;


namespace Y7MP
{
    public static class MPBattle
    {
        public static Fighter HandleAttackerSelection(bool readOnly, bool getNextFighter)
        {
            return FighterManager.GetPlayer();
        }
    }
}
