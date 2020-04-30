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

        LDMEM,

        BR,
        BRTRUE,

        CALL,
        LDARG,
    }
}
