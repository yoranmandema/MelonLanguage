namespace MelonLanguage.Native {
    public class BooleanType : MelonType {
        public override string Name { get; } = "bool";

        public BooleanType(MelonEngine engine) : base(engine) {
        }

        [MelonFunction]
        public BooleanInstance Construct(bool value) {
            return new BooleanInstance(Engine, this, value);
        }
    }
}
