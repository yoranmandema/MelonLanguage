using Antlr4.Runtime;
using MelonLanguage.Grammar;
using MelonLanguage.Native;
using MelonLanguage.Runtime.Interpreter;
using MelonLanguage.Visitor;
using System;

namespace MelonLanguage {
    public class MelonEngine {

        public MelonObject CompletionValue { get; private set; }

        public string[] strings { get; private set; }

        public int[] Parse (string text) {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            MelonLexer speakLexer = new MelonLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
            MelonParser speakParser = new MelonParser(commonTokenStream);
            MelonParser.ProgramContext context = speakParser.program();
            MelonVisitor visitor = new MelonVisitor();

            visitor.Visit(context);

            strings = visitor.strings.ToArray();

            return visitor.instructions.ToArray();
        }

        public MelonEngine Execute (string text) {
            var instructions = Parse(text);

            var interpreter = new MelonInterpreter(strings);

            CompletionValue = interpreter.Execute(instructions);

            return this;
        }

        public MelonEngine Execute(int[] instructions) {
            var interpreter = new MelonInterpreter(strings);

            CompletionValue = interpreter.Execute(instructions);

            return this;
        }
    }
}
