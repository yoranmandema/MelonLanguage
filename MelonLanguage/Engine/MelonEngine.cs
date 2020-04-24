using Antlr4.Runtime;
using MelonLanguage.Grammar;
using MelonLanguage.Native;
using MelonLanguage.Runtime.Interpreter;
using MelonLanguage.Visitor;
using System.Collections.Generic;

namespace MelonLanguage {
    public class MelonEngine {
        public MelonObject CompletionValue { get; private set; }

        public Dictionary<int, string> Strings { get; private set; }

        public int[] Parse(string text) {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            MelonLexer speakLexer = new MelonLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
            MelonParser speakParser = new MelonParser(commonTokenStream);
            MelonParser.ProgramContext context = speakParser.program();
            MelonVisitor visitor = new MelonVisitor(this);

            visitor.Visit(context);

            Strings = visitor.strings;

            return visitor.instructions.ToArray();
        }

        public MelonEngine Execute(string text) {
            var instructions = Parse(text);
            var interpreter = new MelonInterpreter(this);

            CompletionValue = interpreter.Execute(instructions);

            return this;
        }

        public MelonEngine Execute(int[] instructions) {
            var interpreter = new MelonInterpreter(this);

            CompletionValue = interpreter.Execute(instructions);

            return this;
        }
    }
}
