namespace MelonLanguage.Native {
    public class DecimalInstance : MelonObject {
        public double value;

        public DecimalInstance(double val) {
            value = val;
        }

        public override string ToString() {
            return value.ToString();
        }
    }
}
