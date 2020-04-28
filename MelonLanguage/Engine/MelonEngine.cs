using Antlr4.Runtime;
using MelonLanguage.Compiling;
using MelonLanguage.Grammar;
using MelonLanguage.Native;
using MelonLanguage.Runtime;
using MelonLanguage.Runtime.Interpreter;
using MelonLanguage.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MelonLanguage {
    public class MelonEngine {
        public MelonObject CompletionValue { get; private set; }

        public Dictionary<int, string> Strings { get; private set; }

        public Dictionary<int, MelonType> Types { get; private set; }

        internal readonly MelonType melonType;
        internal readonly AnyType anyType;
        internal readonly IntegerType integerType;
        internal readonly FloatType floatType;
        internal readonly StringType stringType;
        internal readonly BooleanType booleanType;
        internal readonly FunctionType functionType;

        public List<TypeMapper> TypeMappers { get; } = new List<TypeMapper> {
            new TypeMapper(typeof(int), typeof(IntegerInstance), (e,i) => e.CreateInteger((int)i)),
            new TypeMapper(typeof(IntegerInstance), typeof(int), (e,i) => ((IntegerInstance)i).value),

            new TypeMapper(typeof(float), typeof(FloatInstance), (e,i) => e.CreateFloat((float)i)),
            new TypeMapper(typeof(double), typeof(FloatInstance), (e,i) => e.CreateFloat((double)i)),
            new TypeMapper(typeof(FloatInstance), typeof(float), (e,i) => (float)((FloatInstance)i).value),
            new TypeMapper(typeof(FloatInstance), typeof(double), (e,i) => ((FloatInstance)i).value),

            new TypeMapper(typeof(string), typeof(StringInstance), (e,i) => e.CreateString((string)i)),
            new TypeMapper(typeof(StringInstance), typeof(string), (e,i) => ((StringInstance)i).value),

            new TypeMapper(typeof(bool), typeof(BooleanInstance), (e,i) => e.CreateBoolean((bool)i)),
            new TypeMapper(typeof(BooleanInstance), typeof(bool), (e,i) => ((BooleanInstance)i).value),

        };

        public MelonEngine() {
            Strings = new Dictionary<int, string>();
            Types = new Dictionary<int, MelonType>();

            anyType = AddType(new AnyType(this));
            melonType = AddType(new MelonType(this));
            integerType = AddType(new IntegerType(this));
            floatType = AddType(new FloatType(this));
            stringType = AddType(new StringType(this));
            booleanType = AddType(new BooleanType(this));
            functionType = AddType(new FunctionType(this));
        }

        public T AddType<T>(T type) where T : MelonType {
            var exists = Types.Values.FirstOrDefault(x => x.GetType() == type.GetType()) != null;

            if (exists) {
                throw new MelonException("Type already exists!");
            }
            else {
                Types[Types.Count] = type;
            }

            return type;
        }

        public IntegerInstance CreateInteger(int value) {
            return integerType.Construct(value);
        }
        public FloatInstance CreateFloat(double value) {
            return floatType.Construct(value);
        }
        public StringInstance CreateString(string value) {
            return stringType.Construct(value);
        }

        public BooleanInstance CreateBoolean(bool value) {
            return booleanType.Construct(value);
        }

        public ParseContext Parse(string text) {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            MelonLexer speakLexer = new MelonLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
            MelonParser speakParser = new MelonParser(commonTokenStream);
            MelonParser.ProgramContext context = speakParser.program();
            MelonVisitor visitor = new MelonVisitor(this);

            return visitor.Parse(context);
        }

        public Context CreateContext(ParseContext parseContext) {
            return new Context(parseContext.LocalNames, parseContext.LocalTypes, parseContext.instructions.ToArray());
        }

        public MelonEngine Execute(string text) {
            var context = Parse(text);
            var interpreter = new MelonInterpreter(this);

            CompletionValue = interpreter.Execute(CreateContext(context));

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
