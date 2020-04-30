namespace MelonLanguage.Native {
    public class StringInstance : MelonInstance {
        public string value;

        public StringInstance(MelonEngine engine, string val) : base(engine) {
            Type = engine.stringType;

            value = val;
        }

        public override string ToString() {
            return value;
        }
    }
}
