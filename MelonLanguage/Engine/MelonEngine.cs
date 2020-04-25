using Antlr4.Runtime;
using MelonLanguage.Grammar;
using MelonLanguage.Native;
using MelonLanguage.Runtime;
using MelonLanguage.Runtime.Interpreter;
using MelonLanguage.Visitor;
using System.Collections.Generic;
using System.Linq;

namespace MelonLanguage {
    public class MelonEngine {
        public MelonObject CompletionValue { get; private set; }

        public Dictionary<int, string> Strings { get; private set; }

        public Dictionary<int, MelonType> Types { get; private set; }

        private IntegerType integerType;

        public MelonEngine () {
            Types = new Dictionary<int, MelonType>();

            integerType = AddType(new IntegerType(this));
        }

        public T AddType<T> (T type) where T : MelonType {
            var exists = Types.Values.FirstOrDefault(x => x.GetType() == type.GetType()) != null;

            if (exists) {
                throw new MelonException("Type already exists!");
            } else {
                Types[Types.Count] = type;
            }

            return type;
        }

        public IntegerInstance CreateInteger (int value) {
            return integerType.Construct(value);
        }

        public Context Parse(string text) {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            MelonLexer speakLexer = new MelonLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
            MelonParser speakParser = new MelonParser(commonTokenStream);
            MelonParser.ProgramContext context = speakParser.program();
            MelonVisitor visitor = new MelonVisitor(this);

            visitor.Parse(context);

            var localTypes = visitor.lexicalEnvironment.CreateLocals(out string[] localNames);

            return new Context(localNames, localTypes, visitor.instructions.ToArray());
        }

        public MelonEngine Execute(string text) {
            var context = Parse(text);
            var interpreter = new MelonInterpreter(this);

            CompletionValue = interpreter.Execute(context);

            return this;
        }

        public MelonEngine Execute(Context context) {
            var interpreter = new MelonInterpreter(this);

            CompletionValue = interpreter.Execute(context);

            context.Reset();

            return this;
        }
    }
}
