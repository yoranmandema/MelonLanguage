namespace MelonLanguage.Native {
    public class VoidType : MelonType {
        public override string Name => "void";
        public override bool IsAssignable => false;

        public VoidType(MelonEngine engine) : base(engine) {
        }
    }
}
