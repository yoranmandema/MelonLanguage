using MelonLanguage.Compiling;
using MelonLanguage.Native;
using MelonLanguage.Visitor;
using System.Linq;

namespace MelonLanguage.Runtime {
    public class ExpressionSolver {
        private readonly MelonEngine _engine;

        public ExpressionSolver(MelonEngine engine) {
            _engine = engine;
        }

        private MelonErrorObject ReturnError(OpCode op, MelonObject left, MelonObject right) {
            var operatorName = MelonVisitor._opCodeText.FirstOrDefault(x => x.Value == op).Key;

            return new MelonErrorObject($"No such operation: {left.GetType().Name} {operatorName} {right.GetType().Name}.");
        }

        public MelonObject Solve(OpCode op, MelonObject left, MelonObject right) {
            return op switch
            {
                OpCode.ADD => Add(left, right),
                OpCode.MUL => Mul(left, right),
                OpCode.CLT => LessThan(left, right),
                OpCode.CEQ => Equal(left, right),
                OpCode.CGT => GreaterThan(left, right),
                _ => ReturnError(op, left, right),
            };
        }
        public MelonObject Equal(MelonObject left, MelonObject right) {
            MelonObject result = (left, right) switch
            {
                (IntegerInstance l, IntegerInstance r) => _engine.CreateBoolean(l.value == r.value),
                (FloatInstance l, FloatInstance r) => _engine.CreateBoolean(l.value == r.value),
                (StringInstance l, StringInstance r) => _engine.CreateBoolean(l.value == r.value),
                (BooleanInstance l, BooleanInstance r) => _engine.CreateBoolean(l.value == r.value),
                _ => ReturnError(OpCode.ADD, left, right)
            };

            return result;
        }

        public MelonObject LessThan(MelonObject left, MelonObject right) {
            MelonObject result = (left, right) switch
            {
                (IntegerInstance l, IntegerInstance r) => _engine.CreateBoolean(l.value < r.value),
                (FloatInstance l, FloatInstance r) => _engine.CreateBoolean(l.value < r.value),
                _ => ReturnError(OpCode.ADD, left, right)
            };

            return result;
        }


        public MelonObject GreaterThan(MelonObject left, MelonObject right) {
            MelonObject result = (left, right) switch
            {
                (IntegerInstance l, IntegerInstance r) => _engine.CreateBoolean(l.value > r.value),
                (FloatInstance l, FloatInstance r) => _engine.CreateBoolean(l.value > r.value),
                _ => ReturnError(OpCode.ADD, left, right)
            };

            return result;
        }

        public MelonObject Add(MelonObject left, MelonObject right) {
            MelonObject result = (left, right) switch
            {
                (IntegerInstance l, IntegerInstance r) => _engine.CreateInteger(l.value + r.value),
                (FloatInstance l, FloatInstance r) => _engine.CreateFloat(l.value + r.value),
                (StringInstance l, StringInstance r) => _engine.CreateString(l.value + r.value),
                _ => ReturnError(OpCode.ADD, left, right)
            };


            return result;
        }

        public MelonObject Mul(MelonObject left, MelonObject right) {
            MelonObject result = (left, right) switch
            {
                (IntegerInstance l, IntegerInstance r) => _engine.CreateInteger(l.value * r.value),
                (FloatInstance l, FloatInstance r) => _engine.CreateFloat(l.value * r.value),
                _ => ReturnError(OpCode.MUL, left, right)
            };

            return result;
        }
    }
}
