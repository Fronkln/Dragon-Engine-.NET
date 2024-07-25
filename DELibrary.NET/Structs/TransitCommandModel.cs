using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class TransitCommandModel
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_TRANSITCOMMANDMODEL_CHECKTRANSITCOMMAND", CallingConvention = CallingConvention.Cdecl)]
        internal static extern FighterCommandID DELib_TransitCommandModel_CheckTransitCommand(IntPtr model, Fighter fighter, FighterCommandID id);

        internal IntPtr _ptr;

        public FighterCommandID CheckTransitCommand(Fighter fighter, FighterCommandID id)
        {
            return DELib_TransitCommandModel_CheckTransitCommand(_ptr, fighter, id);
        }
    }
}
