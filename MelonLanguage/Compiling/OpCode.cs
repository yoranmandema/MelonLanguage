namespace MelonLanguage.Compiling {
    public enum OpCode : int {
        ADD,
        SUB,
        MUL,
        DIV,
        MOD,
        EXP,

        LDSTR,
        LDINT,
        LDFLO,
        LDBOOL,

        STLOC,
        LDLOC,
    }
}
