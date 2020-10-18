using Antlr4.Runtime.Tree;
using MelonLanguage.Compiling;
using MelonLanguage.Extensions;
using MelonLanguage.Grammar;
using MelonLanguage.Native;
using MelonLanguage.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MelonLanguage.Visitor {
    public partial class MelonVisitor : MelonBaseVisitor<ParseResult> {
        private int instructionline;
        private readonly MelonEngine _engine;
        private readonly ExpressionSolver _expressionSolver;
        private ParseContext parseContext;
        private readonly ScriptFunctionInstance _scriptFunction;

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
            { OpCode.LDPRP, 1 },
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

        public MelonVisitor(MelonEngine engine, ScriptFunctionInstance scriptFunction) : this(engine) {
            _scriptFunction = scriptFunction;
        }

        public ParseContext Parse(IParseTree context, LexicalEnvironment parentEnvironment) {
            parseContext = new ParseContext(_engine, parentEnvironment);

            if (_scriptFunction != null) {
                foreach (var kv in parentEnvironment.Variables) {
                    parseContext.AddVariableReference(kv.Value, VariableReferenceType.Argument);
                }
            }

            Visit(context);

            parseContext.LexicalEnvironment = parseContext.LexicalEnvironment.Root;

            return parseContext;
        }

        public override ParseResult VisitBlock(MelonParser.BlockContext context) {
            var env = parseContext.LexicalEnvironment;

            parseContext.LexicalEnvironment = new LexicalEnvironment(env, false);

            base.VisitBlock(context);

            parseContext.LexicalEnvironment = env;

            return DefaultResult;
        }

        public override ParseResult VisitWhileStatement(MelonParser.WhileStatementContext context) {
            parseContext.instructions.AddRange(new int[] { (int)OpCode.BR, 0 });

            int startLine = instructionline + 1;
            int startIndex = parseContext.instructions.Count;
            int brInstrArg = parseContext.instructions.Count - 1;

            Visit(context.block());

            int conditionIndex = parseContext.instructions.Count;
            parseContext.BranchLines[conditionIndex] = instructionline + 1;
            parseContext.BranchLines[startIndex] = startLine;

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
            else if (value is StringInstance stringInstance) {
                return new int[] {
                    (int)OpCode.LDSTR, GetString(stringInstance.value)
                };
            }
            else if (value is BooleanInstance booleanInstance) {
                return new int[] {
                    (int)OpCode.LDBOOL, booleanInstance.value ? 1 : 0
                };
            }

            throw new NotImplementedException($"Literal instructions not implemented for {value.GetType()}");

            //return new int[0];
        }

        private TypeReference CreateTypeReference(MelonParser.TypeContext typeContext) {
            var type = _engine.Types.FirstOrDefault(x => x.Value.Name == typeContext.name().value).Value;

            TypeReference typeReference = new TypeReference(_engine, type);

            if (typeContext.genericParameters()?.type() != null) {
                typeReference.GenericTypes = new TypeReference[typeContext.genericParameters().type().Length];

                for (int i = 0; i < typeContext.genericParameters().type().Length; i++) {
                    if (typeContext.genericParameters().type(i) != null) {
                        typeReference.GenericTypes[i] = CreateTypeReference(typeContext.genericParameters().type(i));
                    }
                }
            }

            return typeReference;
        }

        public override ParseResult VisitVariableDefinitionStatement(MelonParser.VariableDefinitionStatementContext context) {
            var expressionResult = Visit(context.expression());
            MelonType type;
            int typeId;
            TypeReference typeReference = null;

            if (context.Type != null) {
                var typeName = context.Type.name().value;
                var typeKv = _engine.Types.FirstOrDefault(x => x.Value.Name == typeName);

                if (typeKv.Value == null) {
                    throw new MelonException($"Could not find type '{typeName}'");
                }

                if (typeKv.Value == _engine.voidType) {
                    throw new MelonException($"Cannot assign to void");
                }

                if (expressionResult.typeReference.TypeId != typeKv.Key && typeKv.Key != 0) {
                    throw new MelonException($"Variable of type '{typeName}' can't be assigned to '{expressionResult.typeReference.Type.Name}'");
                }

                typeReference = CreateTypeReference(context.Type);

                type = typeKv.Value;
                typeId = typeKv.Key;
            }
            else {
                type = expressionResult.typeReference.Type;
                typeReference = new TypeReference(_engine, type);

                if (type == _engine.voidType) {
                    throw new MelonException($"Cannot assign to void");
                }

                typeId = expressionResult.typeReference.TypeId;
            }

            var name = context.Name.value;
            var variable = parseContext.LexicalEnvironment.GetVariable(name);

            int id;

            if (expressionResult.value is IGeneric generic) {
                generic.GenericTypes = typeReference.GenericTypes;
            }

            if (variable != null) {
                throw new MelonException($"Variable '{name}' already defined.");
            }
            else {
                variable = parseContext.LexicalEnvironment.AddVariable(name, expressionResult.value, typeReference);

                id = parseContext.AddVariableReference(variable, VariableReferenceType.Local);
            }

            parseContext.instructions.Add((int)OpCode.STLOC);
            parseContext.instructions.Add(id);
            instructionline++;

            return DefaultResult;
        }

        public override ParseResult VisitAssignStatement(MelonParser.AssignStatementContext context) {
            var expressionResult = Visit(context.expression());

            var name = context.name().value;
            var variable = parseContext.LexicalEnvironment.GetVariable(name);
            var variableReference = parseContext.Variables.First(x => x.Value.Variable == variable).Value;

            if (expressionResult.typeReference.Type != variable.type.Type) {
                throw new MelonException($"Variable of type '{variable.type.Type.Name}' can't be assigned to '{expressionResult.typeReference.Type.Name}'");
            }

            int id;

            if (variable != null) {
                id = parseContext.GetVariableReference(variableReference);
                variable.value = expressionResult.value;
            }
            else {
                throw new MelonException($"Could not find variable '{name}'");
            }

            parseContext.instructions.Add((int)OpCode.STLOC);
            parseContext.instructions.Add(id);
            instructionline++;

            return DefaultResult;
        }

        public override ParseResult VisitNameExp(MelonParser.NameExpContext context) {
            var name = context.name().value;
            var variable = parseContext.LexicalEnvironment.GetVariable(name);
            var typeKv = _engine.Types.FirstOrDefault(t => t.Value.Name == name);

            instructionline++;

            if (variable != null) {
                int id = -1;

                if (parseContext.Variables.Any(x => x.Value.Variable == variable)) {
                    id = parseContext.Variables.First(x => x.Value.Variable == variable).Key;
                }
                else {
                    id = parseContext.AddVariableReference(variable, VariableReferenceType.Outer);
                }

                parseContext.instructions.Add((int)OpCode.LDLOC);
                parseContext.instructions.Add(id);

                return new ParseResult {
                    value = variable.value,
                    type = ParseResultTypes.Local,
                    typeReference = variable.type
                };
            }
            else if (typeKv.Value != null) {
                parseContext.instructions.Add((int)OpCode.LDTYP);
                parseContext.instructions.Add(typeKv.Key);

                return new ParseResult {
                    type = ParseResultTypes.Type,
                    value = typeKv.Value,
                    typeReference = new TypeReference(_engine, typeKv.Key)
                };
            }
            else {
                throw new MelonException($"Variable '{name}' does not exist!");
            }
        }

        public override ParseResult VisitMemberAccessExp(MelonParser.MemberAccessExpContext context) {
            ParseResult left = Visit(context.expression());

            string memberName = context.name().GetText();

            parseContext.instructions.Add((int)OpCode.LDPRP);
            parseContext.instructions.Add(GetString(memberName));

            instructionline++;

            if (left.typeReference.TypeId == 0) {
                return new ParseResult {
                    typeReference = new TypeReference(_engine, 0)
                };
            }

            var prototype = left.typeReference.Type.Prototype;

            if (left.value?.Properties.ContainsKey(memberName) == true) {
                var memberValue = left.value.GetProperty(memberName).value;

                return new ParseResult {
                    value = memberValue,
                    self = left.value,
                    selfTypeReference = left.typeReference,
                    typeReference = new TypeReference(_engine, GetTypeReference(memberValue))
                };
            }
            else if (prototype?.Properties.ContainsKey(memberName) == true) {
                var memberValue = prototype.GetProperty(memberName).value;

                return new ParseResult {
                    value = memberValue,
                    self = left.value,
                    selfTypeReference = left.typeReference,
                    typeReference = new TypeReference(_engine, GetTypeReference(memberValue))
                };
            }
            else {
                throw new MelonException($"Object does not contain property '{memberName}'");
            }
        }

        public override ParseResult VisitParenthesisExp(MelonParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override ParseResult VisitCallExp(MelonParser.CallExpContext context) {
            var functionResult = Visit(context.Function);

            TypeReference returnType = null;
            FunctionInstance function = null;

            if (functionResult.value != null && functionResult.value is FunctionInstance func) {
                function = func;

                if (function.ReturnType != null) {
                    returnType = function.ReturnType;
                }
            }

            var args = context.Arguments.expression().Reverse().ToArray();

            if (function.ParameterTypes?.Length != args.Length && !function.ParameterTypes.Last().IsVarargs) {
                throw new MelonException($"Argument count mismatch");
            }

            for (int i = 0; i < args.Length; i++) {
                var expressionResult = Visit(args[i]);
                parseContext.instructions.Add((int)OpCode.LDARG);
                instructionline++;
                var parameterType = function.ParameterTypes[i];

                if (i < function.ParameterTypes?.Length) {                   
                    if (parameterType.IsGeneric) {
                        var genericType = (functionResult.self as IGeneric).GenericTypes[parameterType.GenericIndex];

                        if (!expressionResult.typeReference.IsEqualTo(genericType)) {
                            throw new MelonException($"Expected argument of type '{genericType}'");
                        }
                    }
                    else {
                        if (!parameterType.Type.IsEqualTo(expressionResult.typeReference)) {
                            throw new MelonException($"Expected argument of type '{parameterType.Type}'");
                        }
                    }
                }
            }

            parseContext.instructions.Add((int)OpCode.CALL);
            instructionline++;

            return new ParseResult {
                typeReference = returnType
            };
        }

        public override ParseResult VisitFunctionDefinitionStatement(MelonParser.FunctionDefinitionStatementContext context) {
            string name = context.Name.GetText();

            LexicalEnvironment functionEnvironment = new LexicalEnvironment(parseContext.LexicalEnvironment, true);

            var functionParameters = new List<FunctionParameter>();

            if (context.Parameters != null) {
                for (var i = 0; i < context.Parameters.parameter().Length; i++) {
                    var parameter = context.Parameters.parameter(i);

                    bool isVarargs = parameter.VARARGS() != null;

                    if (isVarargs && i < context.Parameters.parameter().Length - 1) {
                        throw new MelonException("Varargs parameter can only appear once and has to be the last parameter");
                    }

                    MelonType type = _engine.anyType;

                    if (parameter.Type != null) {
                        var typeName = parameter.Type.name().value;
                        var typeKv = _engine.Types.FirstOrDefault(x => x.Value.Name == typeName);

                        if (typeKv.Value == null) {
                            throw new MelonException($"Could not find type '{typeName}'");
                        }

                        type = typeKv.Value;
                    }

                    var typeRef = new TypeReference(_engine, type);

                    functionEnvironment.AddVariable(parameter.Name.value, null, typeRef);
                    functionParameters.Add(new FunctionParameter(parameter.Name.value, typeRef, isVarargs));
                }
            }

            var function = new ScriptFunctionInstance(name, _engine) {
                ParameterTypes = functionParameters.ToArray()
            };

            var variable = parseContext.LexicalEnvironment.AddVariable(name, function, new TypeReference(_engine, _engine.functionType));
            parseContext.AddVariableReference(variable, VariableReferenceType.Local);

            if (context.ReturnType != null) {
                var typeName = context.ReturnType.value;
                var typeKv = _engine.Types.FirstOrDefault(x => x.Value.Name == typeName);

                if (typeKv.Value == null) {
                    throw new MelonException($"Could not find type '{typeName}'");
                }

                function.ReturnType = new TypeReference(_engine,typeKv.Value);
            }

            MelonVisitor visitor = new MelonVisitor(_engine, function);
            ParseContext functionParseContext = visitor.Parse(context.Block, functionEnvironment);

            function.SetContext(functionEnvironment, functionParseContext);

            return DefaultResult;
        }

        public override ParseResult VisitReturnStatement(MelonParser.ReturnStatementContext context) {
            if (_scriptFunction == null) {
                throw new MelonException("Return statement must be inside function");
            }

            if (context.Expression != null) {
                var expressionResult = Visit(context.Expression);

                if (_scriptFunction.ReturnType.GetType() != typeof(AnyType)) {
                    var isValidReturnType = _scriptFunction.ReturnType.IsEqualTo(expressionResult.typeReference);

                    if (!isValidReturnType) {
                        throw new MelonException($"Expression must return value of type '{_scriptFunction.ReturnType}'");
                    }
                }
            }
            else if (_scriptFunction.ReturnType != null && _scriptFunction.ReturnType.GetType() != typeof(VoidType)) {
                throw new MelonException($"Function with return type must return a value");
            }

            parseContext.instructions.Add((int)OpCode.RET);

            return DefaultResult;
        }

        private int GetTypeReference(MelonObject obj) {
            if (obj is MelonInstance melonInstance) {
                return _engine.Types.KeyByValue(melonInstance.Type);
            }
            else if (obj is MelonType melonType) {
                return _engine.Types.KeyByValue(melonType);
            }
            else {
                return _engine.Types.KeyByValue(_engine.anyType);
            }
        }

        public override ParseResult VisitBinaryOperationExp(MelonParser.BinaryOperationExpContext context) {
            var left = Visit(context.Left);
            var right = Visit(context.Right);
            var opCode = _opCodeText[context.Operation.Text];
            var operatorName = MelonVisitor._opCodeText.FirstOrDefault(x => x.Value == opCode).Key;

            var leftType = left.typeReference.Type;
            var rightType = right.typeReference.Type;

            var getOutComeType = _expressionSolver.GetTypeForOperation(opCode, leftType, rightType);

            if (getOutComeType == null && (left.typeReference.TypeId != 0 && right.typeReference.TypeId != 0)) {
                throw new MelonException($"No such operation: '{leftType.Name}' {operatorName} '{rightType.Name}'.");
            }

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
                        throw new MelonException($"No such operation: '{leftType.Name}' {operatorName} '{rightType.Name}'.");
                    }

                    parseContext.instructions.AddRange(GetInstructionsForLiteralValue(result));
                    instructionline++;

                    return new ParseResult {
                        type = ParseResultTypes.Literal,
                        value = result,
                        typeReference = new TypeReference(_engine, GetTypeReference(result))
                    };
                }
            }

            parseContext.instructions.AddRange(GetInstructionsForOperation(context.Operation.Text, out int lineIncrease));
            instructionline += lineIncrease;

            var typeReference = _engine.Types.FirstOrDefault(t => t.Value.GetType() == getOutComeType).Key;

            return new ParseResult {
                typeReference = new TypeReference(_engine, typeReference)
            };
        }

        public override ParseResult VisitArrayLiteral(MelonParser.ArrayLiteralContext context) {
            parseContext.instructions.Add((int)OpCode.LDARR);
            instructionline++;

            var value = _engine.CreateArray();

            for (int i = 0; i < context.array().expressionGroup().expression()?.Length; i++) {
                parseContext.instructions.Add((int)OpCode.DUP);
                instructionline++;

                var expressionResult = Visit(context.array().expressionGroup().expression(i));

                value.SetValue(i, expressionResult.value);

                EmitLDINT(i);

                parseContext.instructions.Add((int)OpCode.STELEM);
                instructionline++;
            }


            return new ParseResult {
                value = value,
                typeReference = new TypeReference(_engine, _engine.arrayType)
            };
        }

        public override ParseResult VisitComputedMemberAccessExp(MelonParser.ComputedMemberAccessExpContext context) {
            var subjectResult = Visit(context.expression(0));
            var indexResult = Visit(context.expression(1));

            parseContext.instructions.Add((int)OpCode.LDELEM);
            instructionline++;

            return new ParseResult {
                typeReference = new TypeReference(_engine, _engine.anyType)
            };
        }

        public override ParseResult VisitComputedMemberAssignStatement(MelonParser.ComputedMemberAssignStatementContext context) {
            var name = context.name().value;
            var variable = parseContext.LexicalEnvironment.GetVariable(name);
            var variableReference = parseContext.Variables.First(x => x.Value.Variable == variable).Value;

            //if (_engine.GetType(expressionResult.typeReference) != variable.type) {
            //    throw new MelonException($"Variable of type '{variable.type.Name}' can't be assigned to '{_engine.GetType(expressionResult.typeReference).Name}'");
            //}

            int id;

            if (variable != null) {
                id = parseContext.GetVariableReference(variableReference);
            }
            else {
                throw new MelonException($"Could not find variable '{name}'");
            }

            parseContext.instructions.Add((int)OpCode.LDLOC);
            parseContext.instructions.Add(id);
            instructionline++;

            var expressionResult = Visit(context.Expression);
            var indexResult = Visit(context.Index);

            parseContext.instructions.Add((int)OpCode.STELEM);
            instructionline++;

            return DefaultResult;
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

        private int GetString(string value) {
            if (_engine.Strings.ContainsValue(value)) {
                return _engine.Strings.First(x => x.Value == value).Key;
            }
            else {
                _engine.Strings.Add(_engine.Strings.Count, value);

                return _engine.Strings.Count - 1;
            }
        }

        private void EmitLDSTR(string value) {
            parseContext.instructions.Add((int)OpCode.LDSTR);
            parseContext.instructions.Add(GetString(value));
            instructionline++;
        }

        public override ParseResult VisitStringLiteral(MelonParser.StringLiteralContext context) {
            EmitLDSTR(context.@string().value);

            var value = _engine.CreateString(context.@string().value);

            return new ParseResult {
                type = ParseResultTypes.Literal,
                value = value,
                typeReference = new TypeReference(_engine, GetTypeReference(value))
            };
        }

        private void EmitLDBOOL(bool value) {
            parseContext.instructions.Add((int)OpCode.LDBOOL);
            parseContext.instructions.Add(value ? 1 : 0);
        }

        public override ParseResult VisitBooleanLiteral(MelonParser.BooleanLiteralContext context) {
            EmitLDBOOL(context.boolean().value);

            var value = _engine.CreateBoolean(context.boolean().value);

            return new ParseResult {
                type = ParseResultTypes.Literal,
                value = value,
                typeReference = new TypeReference(_engine, GetTypeReference(value))
            };
        }

        private void EmitLDINT(int value) {
            parseContext.instructions.Add((int)OpCode.LDINT);
            parseContext.instructions.Add(value);
            instructionline++;
        }

        public override ParseResult VisitIntegerLiteral(MelonParser.IntegerLiteralContext context) {
            EmitLDINT(context.integer().value);

            var value = _engine.CreateInteger(context.integer().value);

            return new ParseResult {
                type = ParseResultTypes.Literal,
                value = value,
                typeReference = new TypeReference(_engine, GetTypeReference(value))
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

        public override ParseResult VisitFloatLiteral(MelonParser.FloatLiteralContext context) {
            EmitLDDEC(context.@float().value);

            var value = _engine.CreateFloat(context.@float().value);

            return new ParseResult {
                type = ParseResultTypes.Literal,
                value = value,
                typeReference = new TypeReference(_engine, GetTypeReference(value))
            };
        }
    }
}
