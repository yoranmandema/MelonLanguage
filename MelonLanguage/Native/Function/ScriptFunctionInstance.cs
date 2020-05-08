using MelonLanguage.Compiling;
using MelonLanguage.Runtime;

namespace MelonLanguage.Native {
    public class ScriptFunctionInstance : FunctionInstance {
        public ScriptFunctionInstance(string name, MelonEngine engine) : base(name, engine) {

        }

        public void SetContext(LexicalEnvironment lexicalEnvironment, ParseContext parseContext) {
            ParseContext = parseContext;
            Context = Engine.CreateContext(parseContext);
            ParseContext.LexicalEnvironment = Context.LexicalEnvironment = lexicalEnvironment;
        }

        public ParseContext ParseContext { get; internal set; }
        public Context Context { get; internal set; }


        public override MelonObject Run(MelonObject self, params MelonObject[] args) {
            var context = Context.Clone();

            context.SetArguments(args);

            Engine.Execute(context);

            return context.ReturnValue;
        }
    }
}
