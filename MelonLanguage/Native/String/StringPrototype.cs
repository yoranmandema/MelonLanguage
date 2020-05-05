namespace MelonLanguage.Native {
    public class StringPrototype : MelonPrototype {
        public StringPrototype(MelonEngine engine) : base(engine) {
            var properties = new PropertyDictionary {
                ["Length"] = new Property(new NativeFunctionInstance("Length", engine, Length))
            };

            SetProperties(properties);
        }

        public MelonObject Length(MelonObject self, Arguments arguments) {
            return Engine.CreateInteger((self as StringInstance).value.Length);
        }
    }
}
