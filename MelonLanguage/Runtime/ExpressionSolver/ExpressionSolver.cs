using MelonLanguage.Compiling;
using MelonLanguage.Native;
using MelonLanguage.Visitor;
using System.Linq;

namespace MelonLanguage.Runtime {
    public class ExpressionSolver {
        private MelonErrorObject ReturnError(OpCode op, MelonObject left, MelonObject right) {
            var operatorName = MelonVisitor._opCodeText.FirstOrDefault(x => x.Value == op).Key;

            return new MelonErrorObject($"No such operation: {left.GetType().Name} {operatorName} {right.GetType().Name}.");
        }

        public MelonObject Solve(OpCode op, MelonObject left, MelonObject right) {
            switch (op) {
                case OpCode.ADD:
                    return Add(left, right);
                case OpCode.MUL:
                    return Mul(left, right);
                default:
                    return ReturnError(op, left, right);
            }
        }

        public MelonObject Add(MelonObject left, MelonObject right) {
            if (left is IntegerInstance && right is IntegerInstance) {
                return new IntegerInstance((left as IntegerInstance).value + (right as IntegerInstance).value);
            }
            else if (left is IntegerInstance && right is DecimalInstance) {
                return new DecimalInstance((left as IntegerInstance).value + (right as DecimalInstance).value);
            }
            else if (left is DecimalInstance && right is IntegerInstance) {
                return new DecimalInstance((left as DecimalInstance).value + (right as IntegerInstance).value);
            }      

            return ReturnError(OpCode.ADD, left, right);
        }

        public MelonObject Mul(MelonObject left, MelonObject right) {
            if (left is IntegerInstance && right is IntegerInstance) {
                return new IntegerInstance((left as IntegerInstance).value * (right as IntegerInstance).value);
            }
            else if (left is IntegerInstance && right is DecimalInstance) {
                return new DecimalInstance((left as IntegerInstance).value * (right as DecimalInstance).value);
            }
            else if (left is DecimalInstance && right is IntegerInstance) {
                return new DecimalInstance((left as DecimalInstance).value * (right as IntegerInstance).value);
            }

            return ReturnError(OpCode.MUL, left, right);
        }
    }
}
