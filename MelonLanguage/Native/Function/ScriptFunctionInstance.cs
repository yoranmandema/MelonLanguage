using MelonLanguage.Compiling;
using MelonLanguage.Runtime;

namespace MelonLanguage.Native {
    public class ScriptFunctionInstance : FunctionInstance {
        public ScriptFunctionInstance(string name, MelonEngine engine, LexicalEnvironment lexicalEnvironment, ParseContext parseContext) : base(name, engine) {
            ParseContext = parseContext;
            Context = engine.CreateContext(parseContext);
            ParseContext.LexicalEnvironment = Context.LexicalEnvironment = lexicalEnvironment;
        }

        public ParseContext ParseContext { get; internal set; }
        public Context Context { get; internal set; }

        public override MelonObject Run(MelonObject self, params MelonObject[] args) {
            Engine.Execute(Context);

            return null;
        }
    }
}
