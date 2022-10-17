using System;
using DragonEngineLibrary;

namespace Brawler
{
    internal static class SpecialBattleManager
    {
        public static void OnCombatStart()
        {
            if (BrawlerBattleManager.Enemies.Length >= 0)
                if (BrawlerBattleManager.Enemies[0].Character.Attributes.soldier_data_id == CharacterNPCSoldierPersonalDataID.yazawa_btl01_0010_000_1)
                    Tutorial01();
        }


        public static void Tutorial01()
        {
            int m_blockCount = 0;

            //Testing
            TutorialManager.Initialize(
                new TutorialSegment[]
                {
                                new TutorialSegment()
                                {
                                    Instructions = "Test!",
                                    TimeToComplete = 0.25f,
                                    OnStart = delegate{BattleTurnManager.RequestHActEvent(new HActRequestOptions(){id = (TalkParamID)12902, is_force_play = true}); },
                                    Silent = true
                                },
                                new TutorialSegment()
                                {
                                    Instructions = "",
                                    TimeToComplete = 0.5f,
                                    OnStart = delegate{BattleTurnManager.RequestHActEvent(new HActRequestOptions(){id = (TalkParamID)12903, is_force_play = true}); },
                                    Silent = true
                                },
                                new TutorialSegment()
                                {
                                    Instructions = "Testing",
                                    Modifiers = TutorialModifier.PlayerDontTakeDamage | TutorialModifier.EnemyDontTakeDamage,
                                    TimeoutIsSuccess = true,
                                    IsCompleteDelegate = delegate{return false; },
                                    TimeToComplete = 20,
                                    UpdateDelegate =
                                    delegate
                                    {
                                        string lightFmt = TutorialManager.GetFormattedButtonStr(TutorialButton.LightAttack);
                                        TutorialManager.SetText($"{lightFmt}{lightFmt}{lightFmt}{lightFmt}\nRush Combo");
                                    }
                                },
                                new TutorialSegment()
                                {
                                    Instructions = "Testing",
                                    Modifiers = TutorialModifier.PlayerDontTakeDamage | TutorialModifier.EnemyDontTakeDamage,
                                    TimeoutIsSuccess = true,
                                    IsCompleteDelegate = delegate{return false; },
                                    TimeToComplete = 20,
                                    UpdateDelegate =
                                    delegate
                                    {
                                        string lightFmt = TutorialManager.GetFormattedButtonStr(TutorialButton.LightAttack);
                                        string heavyFmt = TutorialManager.GetFormattedButtonStr(TutorialButton.HeavyAttack);
                                        TutorialManager.SetText(
                                            $"{lightFmt}{heavyFmt} OR\n" +
                                            $"{lightFmt}{lightFmt}{heavyFmt} OR\n" +
                                            $"{lightFmt}{lightFmt}{lightFmt}{heavyFmt}\n" +
                                            $"Finishing Blows");
                                    }
                                },
                                new TutorialSegment()
                                {
                                    Instructions = "",
                                    Modifiers = TutorialModifier.PlayerDontTakeDamage | TutorialModifier.EnemyDontTakeDamage,
                                    TimeoutIsSuccess = false,
                                    IsCompleteDelegate = delegate{ return BrawlerPlayer.m_lastMove != null && BrawlerPlayer.m_lastMove is MoveSidestep; },
                                    TimeToComplete = 10,
                                    UpdateDelegate =
                                    delegate
                                    {
                                        string swayFmt = TutorialManager.GetFormattedButtonStr(TutorialButton.Dodge);
                                        TutorialManager.SetText($"{swayFmt}\nDodging");
                                    }
                                },
                                new TutorialSegment()
                                {
                                    Instructions = "",
                                    Modifiers = TutorialModifier.PlayerDontTakeDamage | TutorialModifier.EnemyDontTakeDamage,
                                    TimeoutIsSuccess = false,
                                    IsCompleteDelegate = delegate{return m_blockCount == 3; },
                                    TimeToComplete = 35,
                                    OnStart = delegate
                                    {
                                        BrawlerPlayer.OnPlayerGuard += delegate{ m_blockCount++; };
                                    },
                                    UpdateDelegate =
                                    delegate
                                    {
                                        string grdFmt = TutorialManager.GetFormattedButtonStr(TutorialButton.Block);
                                        TutorialManager.SetText($"Guard 3 attacks with {grdFmt}\nGuarding");
                                    }
                                },
                                new TutorialSegment()
                                {
                                    Instructions = "",
                                    Modifiers = TutorialModifier.PlayerDontTakeDamage | TutorialModifier.EnemyDontTakeDamage,
                                    TimeoutIsSuccess = true,
                                    IsCompleteDelegate = delegate{return false; },
                                    TimeToComplete = 25,
                                    UpdateDelegate =
                                    delegate
                                    {
                                        string grabFmt = TutorialManager.GetFormattedButtonStr(TutorialButton.Grab);
                                        TutorialManager.SetText($"Grab enemies with {grabFmt}\nGrabbing ");
                                    }
                                },
                                new TutorialSegment()
                                {
                                    Instructions = "Kick Ushio's ass!",
                                    IsCompleteDelegate = delegate{ return BrawlerBattleManager.Enemies.Length <= 0; },
                                    TimeToComplete = -1,
                                },
                });
        }
    }
}
