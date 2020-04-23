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
        public Dictionary<int, string> strings = new Dictionary<int, string>();

        private readonly ExpressionSolver _expressionSolver = new ExpressionSolver();

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
            { OpCode.LDDEC, 2 }
        };

        internal static readonly Dictionary<Type, OpCode> _opCodeLiteralTypes = new Dictionary<Type, OpCode> {
            { typeof(IntegerInstance), OpCode.LDINT },
            { typeof(DecimalInstance), OpCode.LDDEC },
            { typeof(StringInstance), OpCode.LDSTR },
            { typeof(BooleanInstance), OpCode.LDBOOL },
        };

        private int[] GetInstructionsForLiteralValue(MelonObject value) {
            if (value is IntegerInstance integerInstance) {
                return new int[] {
                    (int)OpCode.LDINT, integerInstance.value
                };
            }
            else if (value is DecimalInstance decimalInstance) {
                // Split double value into 2 int32s
                var bit64 = BitConverter.DoubleToInt64Bits(decimalInstance.value);
                var left = (int)(bit64 >> 32);
                var right = (int)bit64;

                return new int[] {
                    (int)OpCode.LDDEC, left, right
                };
            }

            return new int[0];
        }

        public override object VisitParenthesisExp(MelonParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override object VisitBinaryOperationExp(MelonParser.BinaryOperationExpContext context) {
            if (Visit(context.Left) is ParseResult leftResult && Visit(context.Right) is ParseResult rightResult) {
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

            if (strings.ContainsValue(value)) {
                strKey = strings.First(x => x.Value == value).Key;
            }
            else {
                strings.Add(strings.Count, value);

                strKey = strings.Count;
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
                value = new IntegerInstance(context.integer().value)
            };
        }

        private void EmitLDDEC(double value) {
            instructions.Add((int)OpCode.LDDEC);

            // Split double value into 2 int32s
            var bit64 = BitConverter.DoubleToInt64Bits(value);
            var left = (int)(bit64 >> 32);
            var right = (int)bit64;

            instructions.Add(left);
            instructions.Add(right);
        }

        public override object VisitDecimalLiteral(MelonParser.DecimalLiteralContext context) {
            EmitLDDEC(context.@decimal().value);

            return new ParseResult {
                isLiteral = true,
                value = new DecimalInstance(context.@decimal().value)
            };
        }
    }
}
