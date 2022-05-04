using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{

    public static class RPG
    {
        ///<summary>Get the commandset ID of a job.</summary>
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_RPG_GET_JOB_COMMAND_SET", CallingConvention = CallingConvention.Cdecl)]
        public static extern BattleCommandSetID GetJobCommandSetID(Player.ID playerID, RPGJobID job);
    }
}
