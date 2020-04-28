using System.Collections.Generic;

namespace MelonLanguage.Compiling {
    public class ParseContext {
        public List<int> instructions = new List<int>();
        public List<int> locals = new List<int>();
        public List<int> types = new List<int>();
        public Dictionary<int, int> branchLines = new Dictionary<int, int>();
        public LexicalEnvironment lexicalEnvironment;
        public int[] LocalTypes { get; private set; }
        public string[] LocalNames { get; private set; }

        public ParseContext(MelonEngine engine) {
            lexicalEnvironment = new LexicalEnvironment(engine);
        }

        public void PrepareLocals() {
            LocalTypes = lexicalEnvironment.CreateLocals(out string[] localNames);
            LocalNames = localNames;
        }
    }
}
