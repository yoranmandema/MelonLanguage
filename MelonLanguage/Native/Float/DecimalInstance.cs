namespace MelonLanguage.Native {
    public class FloatInstance : MelonObject {
        public double value;

        public FloatInstance(double val) {
            value = val;
        }

        public override string ToString() {
            return value.ToString();
        }
    }
}
