﻿namespace MelonLanguage.Native {
    public class StringInstance : MelonObject {
        public string value;

        public StringInstance(string val) {
            value = val;
        }

        public override string ToString() {
            return value;
        }
    }
}