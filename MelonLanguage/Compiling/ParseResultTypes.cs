namespace MelonLanguage.Compiling {
    public enum ParseResultTypes {
        None = 0b_0000_0000,
        Literal = 0b_0000_0001,
        Type = 0b_0000_0010,
        Local = 0b_0000_0100,
        Function = 0b_0000_1000,
    }
}
