using MelonLanguage.Compiling;
using MelonLanguage.Grammar;
using MelonLanguage.Native;
using MelonLanguage.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MelonLanguage.Visitor {
    public partial class MelonVisitor : MelonBaseVisitor<object> {
        public List<int> instructions = new List<int>();
        public List<int> locals = new List<int>();
        public LexicalEnvironment lexicalEnvironment;

        private readonly MelonEngine _engine;
        private readonly ExpressionSolver _expressionSolver;

        internal static readonly Dictionary<string, OpCode> _opCodeText = new Dictionary<string, OpCode> {
            { "+", OpCode.ADD },
            { "-", OpCode.SUB },
            { "/", OpCode.DIV },
            { "*", OpCode.MUL },
            { "%", OpCode.MOD },
            { "**", OpCode.EXP }
        };

        internal static readonly Dictionary<OpCode, int> _opCodeArgs = new Dictionary<OpCode, int> {
            { OpCode.LDINT, 1 },
            { OpCode.LDSTR, 1 },
            { OpCode.LDBOOL, 1 },
            { OpCode.LDFLO, 2 }
        };

        internal static readonly Dictionary<Type, OpCode> _opCodeLiteralTypes = new Dictionary<Type, OpCode> {
            { typeof(IntegerInstance), OpCode.LDINT },
            { typeof(FloatInstance), OpCode.LDFLO },
            { typeof(StringInstance), OpCode.LDSTR },
            { typeof(BooleanInstance), OpCode.LDBOOL },
        };

        public MelonVisitor (MelonEngine engine) {
            _engine = engine;
            _expressionSolver = new ExpressionSolver(engine);
            lexicalEnvironment = new LexicalEnvironment(engine);
        }

        public void Parse (MelonParser.ProgramContext context) {
            Visit(context);

            lexicalEnvironment = lexicalEnvironment.Root;
        }

        public override object VisitBlock(MelonParser.BlockContext context) {
            lexicalEnvironment = new LexicalEnvironment(lexicalEnvironment);

            return base.VisitBlock(context);
        }

        private int[] GetInstructionsForLiteralValue(MelonObject value) {
            if (value is IntegerInstance integerInstance) {
                return new int[] {
                    (int)OpCode.LDINT, integerInstance.value
                };
            }
            else if (value is FloatInstance decimalInstance) {
                // Split double value into 2 int32s
                var bit64 = BitConverter.DoubleToInt64Bits(decimalInstance.value);
                var left = (int)(bit64 >> 32);
                var right = (int)bit64;

                return new int[] {
                    (int)OpCode.LDFLO, left, right
                };
            }

            return new int[0];
        }

        public override object VisitAssignStatement(MelonParser.AssignStatementContext context) {
            Visit(context.expression());

            var typeName = context.name(0).value;
            var typeKv = _engine.Types.FirstOrDefault(x => x.Value.Name == typeName);

            if (typeKv.Value == null) throw new MelonException($"Could not find type '{typeName}'");

            var name = context.name(1).value;
            var variable = lexicalEnvironment.GetVariable(name);

            int id;

            if (variable != null) {
                id = variable.id;
            } else {
                locals.Add(typeKv.Key);
                id = locals.Count - 1;

                var newVariable = new Variable {
                    id = id,
                    name = name,
                    type = typeKv.Value
                };

                lexicalEnvironment.Variables.Add(name, newVariable);
            }

            instructions.Add((int)OpCode.STLOC);
            instructions.Add(id);

            return DefaultResult;
        }

        public override object VisitNameExp(MelonParser.NameExpContext context) {
            var name = context.name().value;
            var variable = lexicalEnvironment.GetVariable(name);

            Console.WriteLine($"Reference to {name}");
            Console.WriteLine(context.parent.GetText());
            
            int id;

            if (variable != null) {
                id = variable.id;

            } else {
                throw new MelonException($"Variable '{name}' does not exist!");
            }

            instructions.Add((int)OpCode.LDLOC);
            instructions.Add(id);

            return DefaultResult;
        }

        public override object VisitParenthesisExp(MelonParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override object VisitBinaryOperationExp(MelonParser.BinaryOperationExpContext context) {
            var left = Visit(context.Left);
            var right = Visit(context.Right);

            if (left is ParseResult leftResult && right is ParseResult rightResult) {
                if (leftResult.isLiteral && rightResult.isLiteral) {
                    // Remove left side instructions
                    var leftLiteralOp = _opCodeLiteralTypes[leftResult.value.GetType()];
                    instructions.RemoveRange(instructions.Count - _opCodeArgs[leftLiteralOp] - 1, _opCodeArgs[leftLiteralOp] + 1);

                    // Remove right side instructions
                    var rightLiteralOp = _opCodeLiteralTypes[rightResult.value.GetType()];
                    instructions.RemoveRange(instructions.Count - _opCodeArgs[rightLiteralOp] - 1, _opCodeArgs[rightLiteralOp] + 1);

                    // Emit instructions for result value
                    var result = _expressionSolver.Solve(_opCodeText[context.Operation.Text], leftResult.value, rightResult.value);

                    if (result is MelonErrorObject melonErrorObject) {
                        throw new Exception(melonErrorObject.message);
                    }

                    instructions.AddRange(GetInstructionsForLiteralValue(result));

                    return new ParseResult {
                        isLiteral = true,
                        value = result
                    };
                }
            }

            instructions.Add((int)_opCodeText[context.Operation.Text]);

            return DefaultResult;
        }

        private void EmitLDSTR(string value) {
            int strKey = -1;

            if (_engine.Strings.ContainsValue(value)) {
                strKey = _engine.Strings.First(x => x.Value == value).Key;
            }
            else {
                _engine.Strings.Add(_engine.Strings.Count, value);

                strKey = _engine.Strings.Count;
            }

            instructions.Add((int)OpCode.LDSTR);
            instructions.Add(strKey);
        }

        public override object VisitStringLiteral(MelonParser.StringLiteralContext context) {
            EmitLDSTR(context.@string().value);

            return new ParseResult {
                isLiteral = true,
                value = new StringInstance(context.@string().value)
            };
        }

        private void EmitLDBOOL(bool value) {
            instructions.Add((int)OpCode.LDBOOL);
            instructions.Add(value ? 1 : 0);
        }

        public override object VisitBooleanLiteral(MelonParser.BooleanLiteralContext context) {
            EmitLDBOOL(context.boolean().value);

            return new ParseResult {
                isLiteral = true,
                value = new BooleanInstance(context.boolean().value)
            };
        }

        private void EmitLDINT(int value) {
            instructions.Add((int)OpCode.LDINT);
            instructions.Add(value);
        }

        public override object VisitIntegerLiteral(MelonParser.IntegerLiteralContext context) {
            EmitLDINT(context.integer().value);

            return new ParseResult {
                isLiteral = true,
                value = _engine.CreateInteger(context.integer().value)
            };
        }

        private void EmitLDDEC(double value) {
            instructions.Add((int)OpCode.LDFLO);

            // Split double value into 2 int32s
            var bit64 = BitConverter.DoubleToInt64Bits(value);
            var left = (int)(bit64 >> 32);
            var right = (int)bit64;

            instructions.Add(left);
            instructions.Add(right);
        }

        public override object VisitFloatLiteral(MelonParser.FloatLiteralContext context) {
            EmitLDDEC(context.@float().value);

            return new ParseResult {
                isLiteral = true,
                value = new FloatInstance(context.@float().value)
            };
        }
    }
}
