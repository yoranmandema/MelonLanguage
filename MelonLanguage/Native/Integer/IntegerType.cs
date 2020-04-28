﻿namespace MelonLanguage.Native {
    public class IntegerType : MelonType {
        public override string Name { get; } = "int";

        public IntegerType(MelonEngine engine) : base(engine) {
        }

        [MelonFunction]
        public IntegerInstance Construct(int value) {
            return new IntegerInstance(Engine, this, value);
        }

        public static MelonObject Parse(MelonEngine engine, MelonObject self, Arguments arguments) {
            return engine.CreateInteger(int.Parse(arguments.GetAs<StringInstance>(0).value));
        }
    }
}