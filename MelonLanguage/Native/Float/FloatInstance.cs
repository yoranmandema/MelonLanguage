namespace MelonLanguage.Native {
    public class FloatInstance : MelonInstance {
        public double value;

        public FloatInstance(MelonEngine engine, MelonType type, double val) : base(engine, type) {
            value = val;
        }

        public override string ToString() {
            return value.ToString();
        }
    }
}
