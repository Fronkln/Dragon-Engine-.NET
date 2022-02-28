using DragonEngineLibrary;


namespace Y7MP
{
    public static class MPBattle
    {
        public static Fighter HandleAttackerSelection(bool readOnly, bool getNextFighter)
        {
            if (FighterManager.IsBrawlerMode())
                BattleTurnManager.ReleaseMenu();

            return FighterManager.GetPlayer();
        }
    }
}
