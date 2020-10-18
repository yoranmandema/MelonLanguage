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

        public Dictionary<int, string> Strings { get; }

        public Dictionary<int, MelonType> Types { get; }

        public LexicalEnvironment GlobalEnvironment { get; internal set; }

        internal readonly MelonType melonType;
        internal readonly VoidType voidType;
        internal readonly AnyType anyType;
        internal readonly ArrayType arrayType;
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
            GlobalEnvironment = new LexicalEnvironment(this, true);
            Strings = new Dictionary<int, string>();
            Types = new Dictionary<int, MelonType>();

            voidType = AddType(new VoidType(this));
            arrayType = AddType(new ArrayType(this));
            anyType = AddType(new AnyType(this));
            melonType = AddType(new MelonType(this));
            functionType = AddType(new FunctionType(this));
            integerType = AddType(new IntegerType(this));
            floatType = AddType(new FloatType(this));
            stringType = AddType(new StringType(this));
            booleanType = AddType(new BooleanType(this));
        }

        public T AddType<T>(T type) where T : MelonType {
            var exists = Types.Values.Any(x => x.GetType() == type.GetType());

            if (exists) {
                throw new MelonException("Type already exists!");
            }
            else {
                Types[Types.Count] = type;
            }

            return type;
        }

        public MelonType GetType(int id) {
            return Types[id];
        }

        public int GetTypeID(Type type) {
            var typeKV = Types.Values.FirstOrDefault(x => x.GetType() == type);

            if (typeKV == null) {
                throw new MelonException($"Type '{type}' not present");
            }

            return Types.First(x => x.Value.GetType() == type).Key;
        }

        public int GetTypeID(MelonType type) {
            var typeKV = Types.Values.FirstOrDefault(x => x == type);

            if (typeKV == null) {
                throw new MelonException("Type already exists");
            }

            return Types.First(x => x.Value == type).Key;
        }

        public MelonEngine FastAdd(string name, MelonObject value) {
            GlobalEnvironment.AddVariable(name, value, new TypeReference(this, GetTypeFromValue(value)));

            return this;
        }

        public MelonType GetTypeFromValue(MelonObject value) {
            if (value is MelonInstance melonInstance) {
                return melonInstance.Type;
            }
            else if (value is MelonType melonType) {
                return melonType;
            }
            else {
                return anyType;
            }
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

        internal ArrayInstance CreateArray(MelonObject[] values) {
            return arrayType.Construct(values);
        }

        public ParseContext Parse(string text) {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            MelonLexer speakLexer = new MelonLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
            MelonParser speakParser = new MelonParser(commonTokenStream);
            MelonParser.ProgramContext context = speakParser.program();
            MelonVisitor visitor = new MelonVisitor(this);

            return visitor.Parse(context, GlobalEnvironment);
        }

        public Context CreateContext(ParseContext parseContext) {
            return new Context(parseContext);
        }

        public MelonEngine Execute(string text) {
            var parseContext = Parse(text);
            var context = CreateContext(parseContext);
            var interpreter = new MelonInterpreter(this);

            interpreter.Execute(context);

            CompletionValue = interpreter.CompletionValue;

            return this;
        }

        public MelonEngine Execute(Context context) {
            var interpreter = new MelonInterpreter(this);

            interpreter.Execute(context);

            CompletionValue = interpreter.CompletionValue;

            context.Reset();

            return this;
        }
    }
}
