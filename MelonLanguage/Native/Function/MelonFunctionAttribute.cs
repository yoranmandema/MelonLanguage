using System;

namespace MelonLanguage.Native {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MelonFunctionAttribute : Attribute {
    }
}
