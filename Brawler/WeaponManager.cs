using System;
using System.Collections.Generic;
using DragonEngineLibrary;

namespace Brawler
{
    public static class WeaponManager
    {
        public static Dictionary<AssetArmsCategoryID, Moveset> WeaponMovesets = new Dictionary<AssetArmsCategoryID, Moveset>();

        public static bool InputUpdate(AssetUnit weapon)
        {
            AssetID assetID = weapon.AssetID;
            AssetArmsCategoryID category = Asset.GetArmsCategory(assetID);

            if (assetID == AssetID.invalid)
                return false;

            Fighter kasuga = FighterManager.GetFighter(0);


            //dont have any movesets for this weapon, drop it
            if (!WeaponMovesets.ContainsKey(category))
            {
                DragonEngine.Log("Don't have moveset for arms category " + category + ", dropping");
                kasuga.DropWeapon(new DropWeaponOption(AttachmentCombinationID.right_weapon, false));
                return false;
            }

            BrawlerPlayer.CurrentMoveset = WeaponMovesets[category];

            return true;
        }

        public static void InitWeaponMovesets()
        {

            Moveset wepSetA = new Moveset
            (
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1340, 1), 1, 6),
                    new MoveString.AttackFrame(new FighterCommandID(1340, 2), 0.5f, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1340, 3), 0.5f, 0.8f, 0.6f),

                }, new MoveInput[]
                {
                    new MoveInput(VirtualKey.LeftButton, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );


            Moveset wepSetC = new Moveset
            (
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1341, 1), 1, 6),
                    new MoveString.AttackFrame(new FighterCommandID(1341, 2), 0.8f, 1.5f),
                    new MoveString.AttackFrame(new FighterCommandID(1341, 3), 0.8f, 1.5f, 2f),

                }, new MoveInput[]
                {
                    new MoveInput(VirtualKey.LeftButton, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );


            //bat
            Moveset wepSetD = new Moveset
            (
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1338, 1), 0.5f, 1),
                 //   new MoveString.AttackFrame(new FighterCommandID(1340, 2), 0.8f, 1.5f),
                    new MoveString.AttackFrame(new FighterCommandID(1338, 3), 0.8f, 1f, 1f),

                }, new MoveInput[]
                {
                    new MoveInput(VirtualKey.LeftButton, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );

            //sword
            Moveset wepSetE = new Moveset()
            {

            };

            //hammer
            Moveset wepSetG = new Moveset
            (
                new MoveString(new MoveString.AttackFrame[]
                {
                    new MoveString.AttackFrame(new FighterCommandID(1339, 1), 1, 6),
                    new MoveString.AttackFrame(new FighterCommandID(1339, 2), 0.5f, 0.8f),
                    new MoveString.AttackFrame(new FighterCommandID(1339, 3), 0.5f, 0.8f, 0.6f),

                }, new MoveInput[]
                {
                    new MoveInput(VirtualKey.LeftButton, false)
                }, MoveSimpleConditions.FighterIsNotDown)
            );

            WeaponMovesets[AssetArmsCategoryID.A] = wepSetA;
            WeaponMovesets[AssetArmsCategoryID.C] = wepSetC;
            WeaponMovesets[AssetArmsCategoryID.D] = wepSetD;
            WeaponMovesets[AssetArmsCategoryID.E] = wepSetE;
            WeaponMovesets[AssetArmsCategoryID.G] = wepSetG;
        }
    }
}
