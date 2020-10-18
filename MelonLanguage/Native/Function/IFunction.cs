using MelonLanguage.Compiling;

namespace MelonLanguage.Native {
    public interface IFunction {
        public TypeReference ReturnType { get; }
        public FunctionParameter[] Parameters { get; }
        public MelonObject Run(params MelonObject[] args);
    }
}
