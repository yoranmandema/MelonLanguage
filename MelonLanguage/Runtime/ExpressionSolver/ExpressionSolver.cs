using MelonLanguage.Compiling;
using MelonLanguage.Native;
using MelonLanguage.Visitor;
using System;
using System.Linq;

namespace MelonLanguage.Runtime {
    public class ExpressionSolver {
        private readonly MelonEngine _engine;

        public ExpressionSolver (MelonEngine engine) {
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
                _ => ReturnError(op, left, right),
            };
        }

        public MelonObject Add(MelonObject left, MelonObject right) {
            MelonObject result = (left, right) switch
            {
                (IntegerInstance l, IntegerInstance r) => _engine.CreateInteger(l.value + r.value),
                (FloatInstance l, FloatInstance r) => new FloatInstance(l.value + r.value),
                (StringInstance l, StringInstance r) => new StringInstance(l.value + r.value),
                _ => ReturnError(OpCode.ADD, left, right)
            };


            return result;
        }

        public MelonObject Mul(MelonObject left, MelonObject right) {
            MelonObject result = (left, right) switch
            {
                (IntegerInstance l, IntegerInstance r) => _engine.CreateInteger(l.value * r.value),
                (FloatInstance l, FloatInstance r) => new FloatInstance(l.value * r.value),
                _ => ReturnError(OpCode.MUL, left, right)
            };

            return result;
        }
    }
}
