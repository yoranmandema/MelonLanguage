using MelonLanguage.Native.Function;

namespace MelonLanguage.Native {
    public class StringPrototype : MelonPrototype {
        public StringPrototype(MelonEngine engine, MelonType type) : base(engine, type) {
            var properties = new PropertyDictionary {
                ["Length"] = new Property(new NativeFunctionInstance("Length", type, engine, Length))
            };

            SetProperties(properties);
        }

        [ReturnType(typeof(IntegerType))]
        public MelonObject Length(MelonObject self, Arguments arguments) {
            return Engine.CreateInteger((self as StringInstance).value.Length);
        }
    }
}
