namespace MelonLanguage.Native {
    public class IntegerInstance : MelonInstance {
        public int value;

        public IntegerInstance(MelonEngine engine, MelonType type, int val) : base(engine, type) {
            value = val;
        }

        public override string ToString() {
            return value.ToString();
        }
    }
}
