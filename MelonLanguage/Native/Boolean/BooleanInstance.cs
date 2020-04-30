namespace MelonLanguage.Native {
    public class BooleanInstance : MelonInstance {
        public bool value;

        public BooleanInstance(MelonEngine engine, bool val) : base(engine) {
            Type = engine.booleanType;

            value = val;
        }

        public override string ToString() {
            return value ? "true" : "false";
        }
    }
}
