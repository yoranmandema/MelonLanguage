namespace MelonLanguage.Native {
    public class StringInstance : MelonInstance {
        public string value;

        public StringInstance(MelonEngine engine, MelonType type, string val) : base(engine, type) {
            value = val;
        }

        public override string ToString() {
            return value;
        }
    }
}
