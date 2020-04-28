namespace MelonLanguage.Native {
    public class BooleanInstance : MelonInstance {
        public bool value;

        public BooleanInstance(MelonEngine engine, MelonType type, bool val) : base(engine, type) {
            value = val;
        }

        public override string ToString() {
            return value ? "true" : "false";
        }
    }
}
