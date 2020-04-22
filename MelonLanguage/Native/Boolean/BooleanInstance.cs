using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Native {
    public class BooleanInstance : MelonObject {
        public bool value;

        public BooleanInstance(bool val) {
            value = val;
        }

        public override string ToString() {
            return value ? "true" : "false";
        }
    }
}
