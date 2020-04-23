namespace MelonLanguage.Native {
    public class IntegerInstance : MelonObject {
        public int value;

        public IntegerInstance(int val) {
            value = val;
        }

        public override string ToString() {
            return value.ToString();
        }
    }
}
