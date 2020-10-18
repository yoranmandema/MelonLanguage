using System;

namespace MelonLanguage.Native {
    public class FloatType : MelonType, INativeType {
        public override string Name { get; } = "float";
        public Type NativeType => typeof(float);

        public FloatType(MelonEngine engine) : base(engine) {
        }

        public void InitProperties () {
            Prototype = new FloatPrototype(Engine, this);
        }

        public FloatInstance Construct(double value) {
            return new FloatInstance(Engine, value);
        }
    }
}
