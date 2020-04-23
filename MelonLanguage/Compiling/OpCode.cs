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
        LDDEC,
        LDBOOL
    }
}
