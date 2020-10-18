using MelonLanguage.Native.Function;
using System;

namespace MelonLanguage.Native {
    public class IntegerType : MelonType, INativeType {
        public override string Name { get; } = "int";
        public Type NativeType => typeof(int);

        public IntegerType(MelonEngine engine) : base(engine) {
        }

        public void InitProperties () {
            Prototype = new IntegerPrototype(Engine, this);

            var properties = new PropertyDictionary() {
                ["Parse"] = new Property(new NativeFunctionInstance("Parse", this, Engine, Parse)),
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
