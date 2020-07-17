namespace MelonLanguage.Compiling {
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

        LDTYP,
        LDSTR,
        LDINT,
        LDFLO,
        LDBOOL,
        LDARR,

        STLOC,
        LDLOC,
        STELEM,
        LDELEM,

        LDPRP,

        BR,
        BRTRUE,

        CALL,
        LDARG,
        RET,

        DUP,
    }
}
