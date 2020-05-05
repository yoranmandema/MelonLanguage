using System;

namespace MelonLanguage.Native {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MelonFunctionAttribute : Attribute {
        public Type ReturnType;
        public Type[] ParameterTypes;

        public MelonFunctionAttribute(Type returnType, params Type[] parameterTypes) {
            ReturnType = returnType;
            ParameterTypes = parameterTypes;
        }
    }
}
