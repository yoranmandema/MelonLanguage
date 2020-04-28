using MelonLanguage.Compiling;
using MelonLanguage.Native;

namespace MelonLanguage.Visitor {
    internal struct ParseResult {
        public MelonObject value;
        public int typeReference;
        public ParseResultTypes type;

        public override string ToString() {
            return $"{type} {typeReference}";
        }
    }
}
