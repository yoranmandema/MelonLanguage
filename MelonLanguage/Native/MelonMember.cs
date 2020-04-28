using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Native {
    public class MelonMember {
        public string name;
        public MelonObject value;

        public MelonMember (string n, MelonObject v) {
            name = n;
            value = v;
        }
    }
}
