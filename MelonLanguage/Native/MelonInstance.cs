namespace MelonLanguage.Native {
    public abstract class MelonInstance : MelonObject {
        public MelonEngine Engine { get; private set; }
        public MelonType Type { get; }

        public MelonInstance(MelonEngine engine, MelonType type) {
            Engine = engine;
            Type = type;
        }
    }
}
