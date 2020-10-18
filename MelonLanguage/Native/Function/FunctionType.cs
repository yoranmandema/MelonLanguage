namespace MelonLanguage.Native {
    public class FunctionType : MelonType {
        public override string Name => "function";
        public FunctionType(MelonEngine engine) : base(engine) {
            Prototype = new FunctionPrototype(engine, this);
        }
    }
}
