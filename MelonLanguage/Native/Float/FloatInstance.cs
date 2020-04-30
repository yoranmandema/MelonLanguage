namespace MelonLanguage.Native {
    public class FloatInstance : MelonInstance {
        public double value;

        public FloatInstance(MelonEngine engine, double val) : base(engine) {
            Type = engine.floatType;

            value = val;
        }

        public override string ToString() {
            return value.ToString();
        }
    }
}
