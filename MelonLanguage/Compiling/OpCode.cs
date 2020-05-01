﻿namespace MelonLanguage.Compiling {
    [System.Flags]
    public enum OpCode : int {
        ADD,
        SUB,
        MUL,
        DIV,
        MOD,
        EXP,

        CLT,
        CGT,
        CEQ,

        LDSTR,
        LDINT,
        LDFLO,
        LDBOOL,

        STLOC,
        LDLOC,
        LDTYP,

        LDPRP,

        BR,
        BRTRUE,

        CALL,
        LDARG,
    }
}
