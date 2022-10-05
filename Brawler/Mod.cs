using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using DragonEngineLibrary;
using Newtonsoft.Json;

namespace Brawler
{
    public class Mod : DragonEngineMod
    {

        public const float CriticalHPRatio = 0.3f;

        public static bool DebugAutomaticCombo = false;
        public static bool DebugNoUpdate = false;
        public static bool DebugShowMenu = false;
        public static bool DisableAttacksFromAI = false;

        private static JsonSerializerSettings m_jsonWriteSettings = new JsonSerializerSettings { Formatting = Formatting.Indented, DefaultValueHandling = DefaultValueHandling.Ignore };

        internal static Dictionary<BattleInput, KeyInfo> Input = new Dictionary<BattleInput, KeyInfo>();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        public static bool IsGameFocused;

        internal class KeyInfo
        {
            public VirtualKey Key;
            public bool Pressed;
            public bool Held;

            public float TimeHeld = 0;
            public float LastTimeSincePressed = 999999999;
        }

#if DEBUG
        public static void DrawMenu()
        {
            //if (!DebugShowMenu)
             //   return;

            /*
            if(ImGui.Begin("Like a Brawler"))
            {
                ImGui.Text("Move Executing: " + (BrawlerPlayer.m_lastMove != null ? BrawlerPlayer.m_lastMove.MoveExecuting() : false));
                ImGui.Text("Attack Cooldown: " + BrawlerPlayer.m_attackCooldown);
            }
            */
        }
#endif

        //https://stackoverflow.com/a/7162873/14569631
       public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
          
            if (activatedHandle == IntPtr.Zero)
                return false;       // No window is currently activated

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }

        public void InputThread()
        {
            while (true)
            {
                foreach (var kv in Input)
                {
                    bool pressed = DragonEngine.IsKeyDown(kv.Value.Key);

                    //Game update loop will clear it to false
                    if (pressed)
                        kv.Value.Pressed = pressed;

                    kv.Value.Held = DragonEngine.IsKeyHeld(kv.Value.Key);
                }

                if (DragonEngine.IsKeyDown(VirtualKey.Numpad7))
                {
                    bool toggle = !FighterManager.IsBrawlerMode();
                    FighterManager.ForceBrawlerMode(toggle);

                    DragonEngine.Log("Brawler Mode: " + toggle);
                }

                if (DragonEngine.IsKeyHeld(VirtualKey.LeftShift))
                {

                    if(DragonEngine.IsKeyHeld(VirtualKey.Y))
                    {
                        DebugNoUpdate = !DebugNoUpdate;
                        Console.WriteLine("No Update: " + DebugNoUpdate);
                    }

                    if (DragonEngine.IsKeyDown(VirtualKey.Numpad2))
                        DebugShowMenu = !DebugShowMenu;


                        if (DragonEngine.IsKeyDown(VirtualKey.G))
                    {
                        EffectEventManager.LoadScreen(0x1A);
                        EffectEventManager.PlayScreen(0x1A);
                        EffectEventManager.PlayScreen(0x21);

                        // DebugNoUpdate = !DebugNoUpdate;
                        //  DragonEngine.Log(DebugNoUpdate + " no update");

                        /*
                        unsafe
                        {
                            BattleDamageInfo inf = new BattleDamageInfo();
                            inf.damage_parts = 17;
                            inf.attack_parts = 10;
                            inf.attacker = DragonEngine.GetHumanPlayer().UID;
                            inf.type_ = 1;
                            inf.base_damage = 14;
                            inf.base_attack = 999;
                            inf.base_damage = 999;
                            inf.fDamageRandomRatio = 1;
                            inf.fHitOrderRatio = 1;
                            inf.fElementDamRatio = 1;
                            inf.fSkillPowRatio = 1;
                            inf.fDefenceRatio = 1;
                            inf.fSkillCriticalRatio = 1;
                            inf.fSkillAddCriticalRatio = 1;
                            inf.fItemAddCriticalRatio = 1;
                            inf.attr[0] = 2;
                            inf.attribute = 1;
                            inf.modify_param[0] = 1065353216;
                            inf.modify_param[1] = 1065353216;
                            inf.modify_param[2] = 1065353216;
                            inf.modify_param[3] = 1065353216;
                            inf.modify_param[4] = 1120403456;
                            inf.modify_param[5] = 1065353216;
                            inf.modify_param[6] = 1065353216;
                            inf.modify_param[7] = 1065353216;
                            inf.modify_param[8] = 1065353216;
                            inf.modify_param[9] = 1065353216;
                            inf.modify_param[10] = 1065353216;
                            inf.modify_param[11] = 1065353216;
                            inf.modify_param[12] = 1065353216;
                            inf.damage = 9999;
                            inf.power = 1;
                            inf.mot = 17034;
                            inf.is_authed = true;
                            inf.is_direct_set = true;



                            BrawlerBattleManager.Enemies[0].GetStatus().AddDamageInfo(inf);
                        }
                        */
                    }

                    if (DragonEngine.IsKeyDown(VirtualKey.Z))
                    {


                        Assembly assmb = Assembly.GetExecutingAssembly();
                        string path = Path.GetDirectoryName(assmb.Location);


                        List<JhrStyleChunk> styleChunks = new List<JhrStyleChunk>();

                        foreach (Style style in BrawlerPlayer.GetStyles())
                            styleChunks.Add(JhrCommand.ToJhrCFC(style));

                        File.WriteAllText(Path.Combine(path, "styles.json"), JsonConvert.SerializeObject(styleChunks.ToArray(), m_jsonWriteSettings));
                        File.WriteAllText(Path.Combine(path, "heat_actions.json"), JsonConvert.SerializeObject(HeatActionManager.HeatActionsList, m_jsonWriteSettings));
                        File.WriteAllText(Path.Combine(path, "weapon_moves.json"), JsonConvert.SerializeObject(JhrCommand.ToJhrCFC(WeaponManager.WeaponMovesets), m_jsonWriteSettings));
                        File.WriteAllText(Path.Combine(path, "job_themes.json"), JsonConvert.SerializeObject(BrawlerBattleManager.JobThemes, m_jsonWriteSettings));
                        File.WriteAllText(Path.Combine(path, "job_auras.json"), JsonConvert.SerializeObject(BrawlerBattleManager.AuraDefs, m_jsonWriteSettings));

                        Console.WriteLine("Wrote json.");
                    }

                    if (DragonEngine.IsKeyDown(VirtualKey.X))
                    {

                        Assembly assmb = Assembly.GetExecutingAssembly();
                        string path = Path.GetDirectoryName(assmb.Location);

                        JhrStyleChunk[] styleChunks = JsonConvert.DeserializeObject<JhrStyleChunk[]>(File.ReadAllText(Path.Combine(path, "styles.json")));
                        List<Style> styles = new List<Style>();

                        foreach(JhrStyleChunk chunk in styleChunks)
                            styles.Add(JhrCommand.ToStyle(chunk));


                        BrawlerPlayer.m_Styles = styles.ToArray();
                        
                        //later
                        //HeatActionManager.HeatActionsList = JsonConvert.DeserializeObject<Dictionary<AssetArmsCategoryID, HeatAction[]>>(File.ReadAllText(Path.Combine(path, "heat_actions.json")), m_jsonWriteSettings);
                        WeaponManager.WeaponMovesets = JhrCommand.ToWeaponSets(JsonConvert.DeserializeObject<YazawaWeaponSetChunk[]>(File.ReadAllText(Path.Combine(path, "weapon_moves.json")), m_jsonWriteSettings));
                        BrawlerBattleManager.JobThemes = JsonConvert.DeserializeObject<Dictionary<RPGJobID, JobTheme>>(File.ReadAllText(Path.Combine(path, "job_themes.json")), m_jsonWriteSettings);
                        BrawlerBattleManager.AuraDefs = JsonConvert.DeserializeObject<Dictionary<RPGJobID, AuraDefinition>>(File.ReadAllText(Path.Combine(path, "job_auras.json")), m_jsonWriteSettings);

                        BrawlerPlayer.m_lastMove = null;

                        Console.WriteLine("Read json.");
                    }


                    if (DragonEngine.IsKeyDown(VirtualKey.Numpad8))
                    {
                        ECBattleStatus status = DragonEngine.GetHumanPlayer().GetBattleStatus();
                        status.AttackPower = 0;

                        DragonEngine.Log("0 damage");
                    }

                    if (DragonEngine.IsKeyDown(VirtualKey.Numpad9))
                    {
                        ECBattleStatus status = DragonEngine.GetHumanPlayer().GetBattleStatus();
                        status.AttackPower = 200;

                        DragonEngine.Log("200 damage");
                    }

                    if (DragonEngine.IsKeyDown(VirtualKey.T))
                    {
                        DisableAttacksFromAI = !DisableAttacksFromAI;
                        DragonEngine.Log("AI Attack: " + !DisableAttacksFromAI); ;
                    }

                    if (DragonEngine.IsKeyDown(VirtualKey.P))
                    {
                        DebugAutomaticCombo = !DebugAutomaticCombo;
                        DragonEngine.Log("Automatic combo: " + DebugAutomaticCombo);
                    }
                }


                if (DebugNoUpdate)
                    continue;

                bool execBrawlerInput = ShouldExecBrawlerInput();

                if (FighterManager.IsBrawlerMode())
                {
                    if(execBrawlerInput)
                        BrawlerPlayer.InputUpdate();
                    else //28.09.2022: testing out if interruptions break anything
                    {
                        BrawlerPlayer.m_lastMove?.OnMoveEnd();
                        BrawlerPlayer.m_lastMove = null;
                    }
                }
            }
        }

        public static bool ShouldExecBrawlerInput()
        {
            Fighter kasugaFighter = BrawlerBattleManager.Kasuga;

            if (!kasugaFighter.IsValid() ||  BrawlerPlayer.Info.IsDead)
                return false;

            if (TutorialManager.IsTutorialPromptVisible())
                return false;

            if (kasugaFighter.Character.IsAnimDamage())
                return false;

            if (BrawlerPlayer.ThrowingWeapon)
                return false;

            //dont exec when synced
            if (BrawlerPlayer.Info.IsSync)
                return false;

            if (BattleTurnManager.CurrentPhase != BattleTurnManager.TurnPhase.Action)
                return false;

            if (BrawlerBattleManager.HActIsPlaying)
                return false;

            if (BrawlerBattleManager.BattleTime < BrawlerBattleManager.BattleStartTime)
                return false;

            if (BrawlerBattleManager.Enemies.Length <= 0)
                return false;

           // if (allEnemies.Where(x => x.IsDead()).ToArray().Length == allEnemies.Length)
              //  return false;

            return true;
        }


        public override void OnModInit()
        {
            DragonEngine.Initialize();

            BrawlerPlayer.Init();
            HeatActionManager.Init();
            WeaponManager.InitWeaponMovesets();

            DragonEngine.RegisterJob(BrawlerBattleManager.Update, DEJob.Update);
            DragonEngine.RegisterJob(EXFollowups.Update, DEJob.Update);
         

            BattleTurnManager.OverrideAttackerSelection(EnemyManager.OnAttackerSelect);
            FighterManager.ForceBrawlerMode(true);

            BrawlerHooks.Init();

#if DEBUG
            DragonEngineLibrary.Advanced.ImGui.RegisterUIUpdate(DrawMenu);
#endif


            Input = new Dictionary<BattleInput, KeyInfo>()
            {
                [BattleInput.LeftMouse] = new KeyInfo() { Key = VirtualKey.LeftButton },
                [BattleInput.RightMouse] = new KeyInfo() { Key = VirtualKey.RightButton },

                [BattleInput.Space] = new KeyInfo() { Key = VirtualKey.Space },
                [BattleInput.LeftShift] = new KeyInfo() { Key = VirtualKey.LeftShift },

                [BattleInput.Q] = new KeyInfo() { Key = VirtualKey.Q },
                [BattleInput.F] = new KeyInfo() { Key = VirtualKey.F },
                [BattleInput.E] = new KeyInfo() { Key = VirtualKey.E },
                [BattleInput.T] = new KeyInfo() { Key = VirtualKey.T}
            };


            Thread thread = new Thread(InputThread);
            thread.Start();

            Thread keyboardThread = new Thread(BrawlerKeyboardMovement.Update);
            keyboardThread.Start();
        }

    }
}
