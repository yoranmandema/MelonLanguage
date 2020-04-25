using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Native {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MelonFunctionAttribute : Attribute {
    }
}
