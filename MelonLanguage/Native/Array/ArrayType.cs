using System;

namespace MelonLanguage.Native {
    public class ArrayType : MelonType, INativeType {
        public override string Name { get; } = "Array";
        public Type NativeType => typeof(Array);

        public ArrayType(MelonEngine engine) : base(engine) {

        }
        public void InitProperties() {
            Prototype = new ArrayPrototype(Engine, this);
        }

        public ArrayInstance Construct(MelonObject[] values) {
            return new ArrayInstance(Engine, values);
        }
    }
}

