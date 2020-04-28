using MelonLanguage.Compiling;
using MelonLanguage.Extensions;
using MelonLanguage.Grammar;
using MelonLanguage.Native;
using MelonLanguage.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MelonLanguage.Visitor {
    public partial class MelonVisitor : MelonBaseVisitor<object> {

        private int instructionline;
        private readonly MelonEngine _engine;
        private readonly ExpressionSolver _expressionSolver;
        private ParseContext parseContext;

        internal static readonly Dictionary<string, OpCode> _opCodeText = new Dictionary<string, OpCode> {
            { "+", OpCode.ADD },
            { "-", OpCode.SUB },
            { "/", OpCode.DIV },
            { "*", OpCode.MUL },
            { "%", OpCode.MOD },
            { "**", OpCode.EXP },
            { "is", OpCode.CEQ },
            { "<", OpCode.CLT },
            { ">", OpCode.CGT },
        };

        internal static readonly Dictionary<OpCode, int> _opCodeArgs = new Dictionary<OpCode, int> {
            { OpCode.LDINT, 1 },
            { OpCode.LDSTR, 1 },
            { OpCode.LDBOOL, 1 },
            { OpCode.LDFLO, 2 },
            { OpCode.LDLOC, 1 },
            { OpCode.STLOC, 1 },
            { OpCode.LDTYP, 1 },
            { OpCode.GTMEM, 1 },
            { OpCode.BR, 1 },
            { OpCode.BRTRUE, 1 },
        };

        internal static readonly Dictionary<Type, OpCode> _opCodeLiteralTypes = new Dictionary<Type, OpCode> {
            { typeof(IntegerInstance), OpCode.LDINT },
            { typeof(FloatInstance), OpCode.LDFLO },
            { typeof(StringInstance), OpCode.LDSTR },
            { typeof(BooleanInstance), OpCode.LDBOOL },
        };

        public MelonVisitor(MelonEngine engine) {
            _engine = engine;
            _expressionSolver = new ExpressionSolver(engine);
        }

        public ParseContext Parse(MelonParser.ProgramContext context) {
            parseContext = new ParseContext(_engine);

            //foreach (var kv in _engine.Types) {
            //    parseContext.lexicalEnvironment.AddVariable(
            //        kv.Key,
            //        kv.Value.Name,
            //        _engine.melonType
            //    );
            //}

            Visit(context);

            parseContext.lexicalEnvironment = parseContext.lexicalEnvironment.Root;
            parseContext.PrepareLocals();

            return parseContext;
        }

        public override object VisitBlock(MelonParser.BlockContext context) {
            var env = parseContext.lexicalEnvironment;

            parseContext.lexicalEnvironment = new LexicalEnvironment(env);

            base.VisitBlock(context);

            parseContext.lexicalEnvironment = env;

            return DefaultResult;
        }

        public override object VisitWhileStatement(MelonParser.WhileStatementContext context) {

            parseContext.instructions.AddRange(new int[] { (int)OpCode.BR, 0 });

            int startLine = instructionline + 1;
            int startIndex = parseContext.instructions.Count;
            int brInstrArg = parseContext.instructions.Count - 1;

            Visit(context.block());

            int conditionIndex = parseContext.instructions.Count;
            parseContext.branchLines[conditionIndex] = instructionline + 1;
            parseContext.branchLines[startIndex] = startLine;

            Visit(context.expression());

            parseContext.instructions[brInstrArg] = conditionIndex;
            parseContext.instructions.AddRange(new int[] { (int)OpCode.BRTRUE, startIndex });

            return DefaultResult;
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

        public override object VisitVariableDefinitionStatement(MelonParser.VariableDefinitionStatementContext context) {
            Visit(context.expression());

            var typeName = context.name(0).value;
            var typeKv = _engine.Types.FirstOrDefault(x => x.Value.Name == typeName);

            if (typeKv.Value == null) {
                throw new MelonException($"Could not find type '{typeName}'");
            }

            var name = context.name(1).value;
            var variable = parseContext.lexicalEnvironment.GetVariable(name);

            int id;

            if (variable != null) {
                id = variable.id;
            }
            else {
                parseContext.locals.Add(typeKv.Key);
                id = parseContext.locals.Count - 1;

                var newVariable = new Variable {
                    id = id,
                    name = name,
                    type = typeKv.Value
                };

                parseContext.lexicalEnvironment.Variables.Add(name, newVariable);
            }

            parseContext.instructions.Add((int)OpCode.STLOC);
            parseContext.instructions.Add(id);
            instructionline++;

            return DefaultResult;
        }

        public override object VisitAssignStatement(MelonParser.AssignStatementContext context) {
            Visit(context.expression());

            var name = context.name().value;
            var variable = parseContext.lexicalEnvironment.GetVariable(name);

            int id;

            if (variable != null) {
                id = variable.id;
            }
            else {
                throw new MelonException($"Could not find variable '{name}'");
            }

            parseContext.instructions.Add((int)OpCode.STLOC);
            parseContext.instructions.Add(id);
            instructionline++;

            return DefaultResult;
        }

        public override object VisitNameExp(MelonParser.NameExpContext context) {
            var name = context.name().value;
            var variable = parseContext.lexicalEnvironment.GetVariable(name);
            var typeKv = _engine.Types.FirstOrDefault(t => t.Value.Name == name);

            instructionline++;

            if (variable != null) {
                parseContext.instructions.Add((int)OpCode.LDLOC);
                parseContext.instructions.Add(variable.id);

                return new ParseResult {
                    type = ParseResultTypes.Local,
                    typeReference = GetTypeReference(variable.type)
                };
            }
            else if (typeKv.Value != null) {
                parseContext.instructions.Add((int)OpCode.LDTYP);
                parseContext.instructions.Add(typeKv.Key);

                return new ParseResult {
                    type = ParseResultTypes.Type,
                    value = typeKv.Value,
                    typeReference = typeKv.Key
                };
            }
            else {
                throw new MelonException($"Variable '{name}' does not exist!");
            }
        }

        public override object VisitMemberAccessExp(MelonParser.MemberAccessExpContext context) {
            var subject = (ParseResult)Visit(context.expression());
            var memberName = context.name().GetText();

            if (subject.value != null) {
                var memberValue = subject.value.Members[memberName].value;

                Console.WriteLine(memberValue);

                parseContext.instructions.Add((int)OpCode.GTMEM);
                parseContext.instructions.Add(GetString(memberName));

                return new ParseResult {
                    value = memberValue,
                    typeReference = GetTypeReference(memberValue)
                };
            } else {
                parseContext.instructions.Add((int)OpCode.GTMEM);
                parseContext.instructions.Add(GetString(memberName));

                return DefaultResult;
            }
        }

        public override object VisitParenthesisExp(MelonParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override object VisitCallExp(MelonParser.CallExpContext context) {
            foreach (var expression in context.Arguments.expression()) {
                Visit(expression);
            }

            var function = Visit(context.Function);

            parseContext.instructions.Add((int)OpCode.CALL);

            return DefaultResult;
        }

        private int GetTypeReference (MelonObject obj) {
            if (obj is MelonInstance melonInstance) {
                return _engine.Types.KeyByValue(melonInstance.Type);
            } else if (obj is MelonType melonType) {
                return _engine.Types.KeyByValue(_engine.melonType);
            } else {
                return _engine.Types.KeyByValue(_engine.anyType);
            }
        }

        public override object VisitBinaryOperationExp(MelonParser.BinaryOperationExpContext context) {
            var left = Visit(context.Left);
            var right = Visit(context.Right);

            if (left is ParseResult leftResult && right is ParseResult rightResult) {
                if (leftResult.type == ParseResultTypes.Literal && rightResult.type == ParseResultTypes.Literal) {
                    // Remove left side instructions
                    var leftLiteralOp = _opCodeLiteralTypes[leftResult.value.GetType()];
                    parseContext.instructions.RemoveRange(parseContext.instructions.Count - _opCodeArgs[leftLiteralOp] - 1, _opCodeArgs[leftLiteralOp] + 1);
                    instructionline--;

                    // Remove right side instructions
                    var rightLiteralOp = _opCodeLiteralTypes[rightResult.value.GetType()];
                    parseContext.instructions.RemoveRange(parseContext.instructions.Count - _opCodeArgs[rightLiteralOp] - 1, _opCodeArgs[rightLiteralOp] + 1);
                    instructionline--;

                    // Emit instructions for result value
                    var result = _expressionSolver.Solve(_opCodeText[context.Operation.Text], leftResult.value, rightResult.value);

                    if (result is MelonErrorObject melonErrorObject) {
                        throw new Exception(melonErrorObject.message);
                    }

                    parseContext.instructions.AddRange(GetInstructionsForLiteralValue(result));
                    instructionline++;

                    return new ParseResult {
                        type = ParseResultTypes.Literal,
                        value = result,
                        typeReference = GetTypeReference(result)
                    };
                }
            }

            parseContext.instructions.AddRange(GetInstructionsForOperation(context.Operation.Text, out int lineIncrease));
            instructionline += lineIncrease;

            return new ParseResult {
                typeReference = 0
            };
        }

        private int[] GetInstructionsForOperation(string operation, out int lineIncrease) {
            lineIncrease = operation switch
            {
                "is not" => 3,
                _ => 1,
            };

            return operation switch
            {
                "+" => new int[] { (int)OpCode.ADD },
                "-" => new int[] { (int)OpCode.SUB },
                "*" => new int[] { (int)OpCode.MUL },
                "/" => new int[] { (int)OpCode.DIV },
                "%" => new int[] { (int)OpCode.MOD },
                "<" => new int[] { (int)OpCode.CLT },
                ">" => new int[] { (int)OpCode.CGT },
                "is" => new int[] { (int)OpCode.CEQ },
                "is not" => new int[] { (int)OpCode.CEQ, (int)OpCode.LDBOOL, 0, (int)OpCode.CEQ },
                _ => throw new MelonException($"Unknown operation '{operation}'"),
            };
        }

        private int GetString (string value) {
            int strKey = -1;

            if (_engine.Strings.ContainsValue(value)) {
                strKey = _engine.Strings.First(x => x.Value == value).Key;
            }
            else {
                _engine.Strings.Add(_engine.Strings.Count, value);

                strKey = _engine.Strings.Count - 1;
            }

            return strKey;
        }

        private void EmitLDSTR(string value) {
            parseContext.instructions.Add((int)OpCode.LDSTR);
            parseContext.instructions.Add(GetString(value));
            instructionline++;
        }

        public override object VisitStringLiteral(MelonParser.StringLiteralContext context) {
            EmitLDSTR(context.@string().value);

            var value = _engine.CreateString(context.@string().value);

            return new ParseResult {
                type = ParseResultTypes.Literal,
                value = value,
                typeReference = GetTypeReference(value)
            };
        }

        private void EmitLDBOOL(bool value) {
            parseContext.instructions.Add((int)OpCode.LDBOOL);
            parseContext.instructions.Add(value ? 1 : 0);
        }

        public override object VisitBooleanLiteral(MelonParser.BooleanLiteralContext context) {
            EmitLDBOOL(context.boolean().value);

            var value = _engine.CreateBoolean(context.boolean().value);

            return new ParseResult {
                type = ParseResultTypes.Literal,
                value = value,
                typeReference = GetTypeReference(value)
            };
        }

        private void EmitLDINT(int value) {
            parseContext.instructions.Add((int)OpCode.LDINT);
            parseContext.instructions.Add(value);
            instructionline++;
        }

        public override object VisitIntegerLiteral(MelonParser.IntegerLiteralContext context) {
            EmitLDINT(context.integer().value);

            var value = _engine.CreateInteger(context.integer().value);

            return new ParseResult {
                type = ParseResultTypes.Literal,
                value = value,
                typeReference = GetTypeReference(value)
            };
        }

        private void EmitLDDEC(double value) {
            parseContext.instructions.Add((int)OpCode.LDFLO);

            // Split double value into 2 int32s
            var bit64 = BitConverter.DoubleToInt64Bits(value);
            var left = (int)(bit64 >> 32);
            var right = (int)bit64;

            parseContext.instructions.Add(left);
            parseContext.instructions.Add(right);
            instructionline++;
        }

        public override object VisitFloatLiteral(MelonParser.FloatLiteralContext context) {
            EmitLDDEC(context.@float().value);

            var value = _engine.CreateFloat(context.@float().value);

            return new ParseResult {
                type = ParseResultTypes.Literal,
                value = value,
                typeReference = GetTypeReference(value)
            };
        }
    }
}
