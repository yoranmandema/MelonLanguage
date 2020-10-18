using System;

namespace MelonLanguage.Native {
    public class StringType : MelonType, INativeType {
        public override string Name { get; } = "string";
        public Type NativeType => typeof(string);

        public StringType(MelonEngine engine) : base(engine) {
        }

        public void InitProperties () {
            Prototype = new StringPrototype(Engine);
        }

        public StringInstance Construct(string value) {
            return new StringInstance(Engine, value);
        }
    }
}
