using System;

namespace MelonLanguage.Native {
    public class FunctionParameter {
        public string Name { get; }
        public Type Type { get; }
        public bool IsVarargs { get; set; }
        public MelonObject DefaultValue { get; set; }

        public FunctionParameter(string name, Type type, bool isVarargs = false) {
            Name = name;
            Type = type;
            IsVarargs = isVarargs;
        }
    }
}
