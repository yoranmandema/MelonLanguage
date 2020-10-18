namespace MelonLanguage.Native {
    public class MelonPrototype : MelonObject {
        public MelonType Type { get; set; }
        
        public MelonPrototype(MelonEngine engine, MelonType type) : base(engine) {
            Type = type;
        }
    }
}
