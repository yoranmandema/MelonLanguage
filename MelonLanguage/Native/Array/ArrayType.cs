namespace MelonLanguage.Native {
    public class ArrayType : MelonType {
        public override string Name { get; } = "Array";

        public ArrayType(MelonEngine engine) : base(engine) {
            Prototype = new ArrayPrototype(engine);
        }

        public ArrayInstance Construct(MelonObject[] values) {
            return new ArrayInstance(Engine, values);
        }
    }
}

