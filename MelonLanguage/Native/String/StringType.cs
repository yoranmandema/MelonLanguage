﻿namespace MelonLanguage.Native {
    public class StringType : MelonType {
        public override string Name { get; } = "string";

        public StringType(MelonEngine engine) : base(engine) {
            Prototype = new StringPrototype(engine);
        }
        public StringInstance Construct(string value) {
            return new StringInstance(Engine, value);
        }
    }
}
