using System;
using DragonEngineLibrary;

namespace Brawler.Moves
{
    //This exists because of a Y7 Bug
    //Grab syncs were broken
    internal class EnemyMoveSync
    {
        public FighterCommandID Start;
        public FighterCommandID HitSuccess;
    }
}
