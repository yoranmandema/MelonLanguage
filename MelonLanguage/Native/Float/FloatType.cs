namespace MelonLanguage.Native {
    public class FloatType : MelonType {
        public override string Name { get; } = "float";

        public FloatType(MelonEngine engine) : base(engine) {
            Prototype = new FloatPrototype(engine);
        }

        [MelonFunction]
        public FloatInstance Construct(double value) {
            return new FloatInstance(Engine, value);
        }

        public double Parse(MelonEngine engine, string str) {
            return double.Parse(str);
        }
    }
}
