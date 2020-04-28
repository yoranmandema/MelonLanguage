namespace MelonLanguage.Native {
    public interface IFunction {
        public MelonType ReturnType { get; }
        public FunctionParameter[] Parameters { get; }
        public MelonObject Run(params MelonObject[] args);
    }
}
