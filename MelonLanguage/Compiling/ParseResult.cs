﻿using MelonLanguage.Compiling;
using MelonLanguage.Native;

namespace MelonLanguage.Visitor {
    public struct ParseResult {
        public MelonObject value;
        public MelonObject self;
        public TypeReference selfTypeReference;
        public TypeReference typeReference;
        public ParseResultTypes type;

        public override string ToString() {
            return $"{type} {typeReference}";
        }
    }
}
