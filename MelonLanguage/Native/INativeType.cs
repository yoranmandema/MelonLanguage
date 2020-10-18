using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Native {
    interface INativeType {
        public Type NativeType { get; }
        public void InitProperties();
    }
}
