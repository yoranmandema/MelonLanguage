using MelonLanguage.Compiling;
using System;

namespace MelonLanguage.Native {
    public class FunctionParameter {
        public string Name { get; }
        public TypeReference Type { get; }
        public bool IsVarargs { get; set; }
        public MelonObject DefaultValue { get; set; }
        public int GenericIndex { get; set; } = -1;

        public bool IsGeneric => GenericIndex > -1;

        public FunctionParameter(string name, TypeReference type, bool isVarargs = false) {
            Name = name;
            Type = type;
            IsVarargs = isVarargs;
        }

        public FunctionParameter(string name, int genericIndex, bool isVarargs = false) {
            Name = name;
            GenericIndex = genericIndex;
            IsVarargs = isVarargs;
        }
    }
}
