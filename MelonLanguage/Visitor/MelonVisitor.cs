using MelonLanguage.Compiling;
using MelonLanguage.Grammar;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MelonLanguage.Visitor {
    public partial class MelonVisitor : MelonBaseVisitor<object> {
        public List<int> instructions = new List<int>();
        public List<string> strings = new List<string>();

        private Dictionary<string, OpCode> opCodeText = new Dictionary<string, OpCode> {
            { "+", OpCode.ADD },
            { "-", OpCode.SUB },
            { "/", OpCode.DIV },
            { "*", OpCode.MUL },
            { "%", OpCode.MOD },
            { "**", OpCode.EXP }
        };

        public override object VisitBinaryOperationExp(MelonParser.BinaryOperationExpContext context) {
            base.VisitBinaryOperationExp(context);

            instructions.Add((int)opCodeText[context.Operation.Text]);

            return DefaultResult;
        }

        public override object VisitStringLiteral(MelonParser.StringLiteralContext context) {
            instructions.Add((int)OpCode.LDSTR);

            strings.Add(context.@string().value);

            instructions.Add(strings.Count - 1);

            return base.VisitStringLiteral(context);
        }

        public override object VisitBooleanLiteral(MelonParser.BooleanLiteralContext context) {
            instructions.Add((int)OpCode.LDBOOL);
            instructions.Add(context.boolean().value ? 1 : 0);

            return base.VisitBooleanLiteral(context);
        }

        public override object VisitIntegerLiteral(MelonParser.IntegerLiteralContext context) {
            instructions.Add((int)OpCode.LDINT);
            instructions.Add(context.integer().value);

            return base.VisitIntegerLiteral(context);
        }

        public override object VisitDecimalLiteral(MelonParser.DecimalLiteralContext context) {
            instructions.Add((int)OpCode.LDDEC);

            // Split double value into 2 int32s
            var bit64 = BitConverter.DoubleToInt64Bits(context.@decimal().value);
            var left = (int)(bit64 >> 32);
            var right = (int)bit64;

            instructions.Add(left);
            instructions.Add(right);

            return base.VisitDecimalLiteral(context);
        }
    }
}
