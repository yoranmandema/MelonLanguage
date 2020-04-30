using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Native {
    public class Property {
        public MelonObject value;

        public Property (MelonObject v) {
            value = v;
        }
    }
}
