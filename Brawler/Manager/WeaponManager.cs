using System;
using System.Collections.Generic;
using DragonEngineLibrary;
using DragonEngineLibrary.Service;

namespace Brawler
{
    public static class WeaponManager
    {
        public static Dictionary<AssetArmsCategoryID, Moveset> WeaponMovesets = new Dictionary<AssetArmsCategoryID, Moveset>();

        private static DETask m_throwProcedure;

        public static bool InputUpdate(AssetUnit weapon)
        {
            AssetID assetID = weapon.AssetID;
            AssetArmsCategoryID category = Asset.GetArmsCategory(assetID);

            if (assetID == AssetID.invalid)
                return false;

            //Don't throw job weapon
            if (!BrawlerPlayer.IsEXGamer)
                if (Mod.Input[BattleInput.T].LastTimeSincePressed < 0.05f && !BrawlerPlayer.ThrowingWeapon)
                    BrawlerPlayer.ThrowWeapon();

            return true;
        }

        public static void Update(AssetUnit weapon)
        {
            AssetID assetID = weapon.AssetID;
            AssetArmsCategoryID category = Asset.GetArmsCategory(assetID);

            if (assetID == AssetID.invalid)
                return;

            Fighter kasuga = BrawlerBattleManager.Kasuga;
            bool wepMovesetExists = WeaponMovesets.ContainsKey(category);

            //dont have any movesets for this weapon, drop it
            if (!wepMovesetExists)
            {
#if !DEBUG
                DragonEngine.Log("Don't have moveset for arms category " + category + ", dropping");
                kasuga.DropWeapon(new DropWeaponOption(AttachmentCombinationID.right_weapon, false));
                return;
#else
                DragonEngine.Log("Don't have moveset for arms category " + category + ". Will not drop on debug");
#endif
            }
            else
                BrawlerPlayer.CurrentMoveset = WeaponMovesets[category];


            if (kasuga.Character.GetMotion().GmtID != 0 && BrawlerPlayer.ThrowingWeapon && m_throwProcedure == null)
            {
                ECMotion motion = BrawlerBattleManager.KasugaChara.GetMotion();

                MotionID gmt = motion.GmtID;
                MotionService.TimingResult throwTick = MotionService.SearchTimingDetail(0, motion.BepID, 82);

                if (!throwTick.IsValid())
                    throwTick.Start = 0;

                m_throwProcedure = new DETask
                    (
                    delegate
                    {
                        MotionPlayInfo playInf = motion.PlayInfo;
                        int tick = (int)playInf.tick_gmt_now_;

                        //Interrupted early
                        if (motion.GmtID == 0 || motion.GmtID != gmt)
                        {
                            m_throwProcedure.Fail = true;
                            return true;
                        }

                        return tick >= throwTick.Start;
                    },
                    delegate
                    {
                        if (!m_throwProcedure.Fail)
                            BrawlerBattleManager.Kasuga.ThrowEquipAsset(false, true);
                      
                        m_throwProcedure = null;
                        BrawlerPlayer.ThrowingWeapon = false;
                    });
            }
        }

        public static void InitWeaponMovesets()
        {

            Moveset wepSetA = new Moveset
            (
                RPGSkillID.invalid,
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1340, 1), false, 6),
                    new MoveString.AttackFrame(new FighterCommandID(1340, 2), false, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1340, 3), false, 0.8f, 0.6f),

                }, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );


            Moveset wepSetC = new Moveset
            (
                RPGSkillID.invalid,
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1341, 1), false, 1f),
                    new MoveString.AttackFrame(new FighterCommandID(1341, 2), true, 0.4f, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1341, 3), true, 0.7f, 0.8f),

                }, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );


            //bat
            Moveset wepSetD = new Moveset
            (
                RPGSkillID.invalid,
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1338, 1), false, 1),
                    new MoveString.AttackFrame(new FighterCommandID(1338, 2), true, 1, 1f),
                    new MoveString.AttackFrame(new FighterCommandID(1338, 3), true, 1f, 0.75f),

                }, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );

            //medium object
            Moveset wepSetH = new Moveset
            (
                RPGSkillID.invalid,
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1348, 1), new FighterCommandID(1348, 2), true, 1),
                    new MoveString.AttackFrame(new FighterCommandID(1348, 3), new FighterCommandID(1348, 4), false, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1348, 5), new FighterCommandID(1348, 6), false, 0.8f, 0.6f),
                    new MoveString.AttackFrame(new FighterCommandID(1348, 7), false, 0.8f, 0.6f),

                }, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );

            //sword
            Moveset wepSetE = new Moveset()
            {

            };

            //hammer
            Moveset wepSetG = new Moveset
            (
                RPGSkillID.invalid,
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1339, 1), false, 1),
                    new MoveString.AttackFrame(new FighterCommandID(1339, 2), false, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1339, 3), false, 0.8f, 0.6f),

                }, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );

            //medium object
            Moveset wepSetN = new Moveset
            (
                RPGSkillID.invalid,
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1347, 1), false, 1),
                    new MoveString.AttackFrame(new FighterCommandID(1347, 2), true, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1347, 3), false, 0.8f, 0.6f),

                }, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );

            Moveset wepSetY = new Moveset
            (
                RPGSkillID.invalid,
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1342, 1), false, 0.5f, 0.6f),

                }, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );

            Moveset wepSetQ = new Moveset
            (
                RPGSkillID.invalid,
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1343, 1), false, 1.5f),

                }, new MoveInput[]
                {
                    new MoveInput(BattleInput.LeftMouse, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );

            WeaponMovesets[AssetArmsCategoryID.A] = wepSetA;
            WeaponMovesets[AssetArmsCategoryID.C] = wepSetC;
            WeaponMovesets[AssetArmsCategoryID.D] = wepSetD;
            WeaponMovesets[AssetArmsCategoryID.E] = wepSetE;
            WeaponMovesets[AssetArmsCategoryID.G] = wepSetG;
            WeaponMovesets[AssetArmsCategoryID.H] = wepSetH;
            WeaponMovesets[AssetArmsCategoryID.N] = wepSetN;
            WeaponMovesets[AssetArmsCategoryID.Y] = wepSetY;
            WeaponMovesets[AssetArmsCategoryID.Q] = wepSetQ;
        }
    }
}
