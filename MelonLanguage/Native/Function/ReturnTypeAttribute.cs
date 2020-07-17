using System;

namespace MelonLanguage.Native.Function {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ReturnTypeAttribute : Attribute {
        public Type Type { get; }

        public ReturnTypeAttribute(Type type) {
            Type = type;
        }
    }
}
