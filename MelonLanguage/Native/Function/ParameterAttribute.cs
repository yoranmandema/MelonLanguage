using System;

namespace MelonLanguage.Native.Function {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ParameterAttribute : Attribute {
        public Type Type { get; set; }
        public string Name { get; set; }
        public bool IsVarargs { get; set; }

        public ParameterAttribute(string name, Type type, bool isVarargs = false) {
            Name = name;
            Type = type;
            IsVarargs = isVarargs;
        }
    }
}
