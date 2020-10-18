using MelonLanguage.Native.Boolean;
using System;

namespace MelonLanguage.Native {
    public class BooleanType : MelonType, INativeType {
        public override string Name { get; } = "bool";
        public Type NativeType => typeof(bool);

        public BooleanType(MelonEngine engine) : base(engine) {
        }

        public void  InitProperties () {
            Prototype = new BooleanPrototype(Engine, this);
        }

        public BooleanInstance Construct(bool value) {
            return new BooleanInstance(Engine, value);
        }
    }
}
