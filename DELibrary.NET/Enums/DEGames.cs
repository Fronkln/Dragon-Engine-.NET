using System;

namespace DragonEngineLibrary
{
    [Flags]
    public enum DEGames
    {
        Y6,
        YK2,
        JE,
        YLAD,
        LJ,
        Y8,
        KenzanKiwami,
        All = Y6 | YK2 | JE | YLAD | LJ | Y8 | KenzanKiwami
    }
}
