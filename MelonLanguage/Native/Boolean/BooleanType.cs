using MelonLanguage.Native.Boolean;

namespace MelonLanguage.Native {
    public class BooleanType : MelonType {
        public override string Name { get; } = "bool";

        public BooleanType(MelonEngine engine) : base(engine) {
            Prototype = new BooleanPrototype(engine);
        }

        public BooleanInstance Construct(bool value) {
            return new BooleanInstance(Engine, value);
        }
    }
}
