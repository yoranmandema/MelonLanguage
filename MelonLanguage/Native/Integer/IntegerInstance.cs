namespace MelonLanguage.Native {
    public class IntegerInstance : MelonInstance {
        public int value;

        public IntegerInstance(MelonEngine engine, int val) : base(engine) {
            Type = engine.integerType;

            value = val;
        }

        public override string ToString() {
            return value.ToString();
        }
    }
}
