using MelonLanguage.Native.Function;

namespace MelonLanguage.Native {
    public class IntegerType : MelonType {
        public override string Name { get; } = "int";

        public IntegerType(MelonEngine engine) : base(engine) {
            Prototype = new IntegerPrototype(engine);

            var properties = new PropertyDictionary() {
                ["Parse"] = new Property(new NativeFunctionInstance("Parse", engine, Parse)),
            };

            SetProperties(properties);
        }

        public IntegerInstance Construct(int value) {
            return new IntegerInstance(Engine, value);
        }

        public MelonObject Constructor(MelonObject self, Arguments arguments) {
            return new IntegerInstance(Engine, arguments.GetAs<IntegerInstance>(0).value);
        }

        [ReturnType(typeof(IntegerType))]
        [Parameter("string", typeof(StringType))]
        public MelonObject Parse(MelonObject self, Arguments arguments) {
            return Construct(int.Parse(arguments.GetAs<StringInstance>(0).value));
        }
    }
}
