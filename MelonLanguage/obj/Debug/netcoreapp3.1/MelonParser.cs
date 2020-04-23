//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.6
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from E:\GitHub\MelonLanguage\MelonLanguage\Grammar\Melon.g4 by ANTLR 4.6.6

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace MelonLanguage.Grammar {
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.6")]
[System.CLSCompliant(false)]
public partial class MelonParser : Parser {
	public const int
		DOT=1, COMMA=2, LEFTPARENTHESIS=3, RIGHTPARENTHESIS=4, IF=5, ELSE=6, LESS=7, 
		LESSEQ=8, GREATER=9, GREATEREQ=10, EQUAL=11, NOTEQUAL=12, IS=13, AND=14, 
		OR=15, ASTERISK=16, SLASH=17, PLUS=18, MINUS=19, REMAINDER=20, EXPONENT=21, 
		BITAND=22, BITOR=23, BITXOR=24, BITSHIFTL=25, BITSHIFTR=26, BITSHIFTUR=27, 
		INCREMENT=28, DECREMENT=29, NOT=30, BITNOT=31, ASSIGN=32, NULL=33, BOOLEAN=34, 
		NAME=35, INTEGER=36, DECIMAL=37, STRING=38, WHITESPACE=39, COMMENT=40, 
		LINE_COMMENT=41, ErrorCharacter=42;
	public const int
		RULE_program = 0, RULE_block = 1, RULE_expression = 2, RULE_string = 3, 
		RULE_decimal = 4, RULE_integer = 5, RULE_boolean = 6, RULE_null = 7;
	public static readonly string[] ruleNames = {
		"program", "block", "expression", "string", "decimal", "integer", "boolean", 
		"null"
	};

	private static readonly string[] _LiteralNames = {
		null, "'.'", "','", "'('", "')'", "'if'", "'else'", "'<'", "'<='", "'>'", 
		"'>='", "'=='", "'!='", "'is'", "'&&'", "'||'", "'*'", "'/'", "'+'", "'-'", 
		"'%'", "'**'", "'&'", "'^'", "'|'", "'<<'", "'>>'", "'>>>'", "'++'", "'--'", 
		"'!'", "'~'", "'='", "'null'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "DOT", "COMMA", "LEFTPARENTHESIS", "RIGHTPARENTHESIS", "IF", "ELSE", 
		"LESS", "LESSEQ", "GREATER", "GREATEREQ", "EQUAL", "NOTEQUAL", "IS", "AND", 
		"OR", "ASTERISK", "SLASH", "PLUS", "MINUS", "REMAINDER", "EXPONENT", "BITAND", 
		"BITOR", "BITXOR", "BITSHIFTL", "BITSHIFTR", "BITSHIFTUR", "INCREMENT", 
		"DECREMENT", "NOT", "BITNOT", "ASSIGN", "NULL", "BOOLEAN", "NAME", "INTEGER", 
		"DECIMAL", "STRING", "WHITESPACE", "COMMENT", "LINE_COMMENT", "ErrorCharacter"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[System.Obsolete("Use Vocabulary instead.")]
	public static readonly string[] tokenNames = GenerateTokenNames(DefaultVocabulary, _SymbolicNames.Length);

	private static string[] GenerateTokenNames(IVocabulary vocabulary, int length) {
		string[] tokenNames = new string[length];
		for (int i = 0; i < tokenNames.Length; i++) {
			tokenNames[i] = vocabulary.GetLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = vocabulary.GetSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}

		return tokenNames;
	}

	[System.Obsolete("Use IRecognizer.Vocabulary instead.")]
	public override string[] TokenNames
	{
		get
		{
			return tokenNames;
		}
	}

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "Melon.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	public MelonParser(ITokenStream input)
		: base(input)
	{
		_interp = new ParserATNSimulator(this,_ATN);
	}
	public partial class ProgramContext : ParserRuleContext {
		public BlockContext block() {
			return GetRuleContext<BlockContext>(0);
		}
		public ITerminalNode Eof() { return GetToken(MelonParser.Eof, 0); }
		public ProgramContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_program; } }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterProgram(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitProgram(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitProgram(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ProgramContext program() {
		ProgramContext _localctx = new ProgramContext(_ctx, State);
		EnterRule(_localctx, 0, RULE_program);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 16; block();
			State = 17; Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.ReportError(this, re);
			_errHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class BlockContext : ParserRuleContext {
		public ExpressionContext[] expression() {
			return GetRuleContexts<ExpressionContext>();
		}
		public ExpressionContext expression(int i) {
			return GetRuleContext<ExpressionContext>(i);
		}
		public BlockContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_block; } }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterBlock(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitBlock(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBlock(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public BlockContext block() {
		BlockContext _localctx = new BlockContext(_ctx, State);
		EnterRule(_localctx, 2, RULE_block);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 22;
			_errHandler.Sync(this);
			_la = _input.La(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << LEFTPARENTHESIS) | (1L << NULL) | (1L << BOOLEAN) | (1L << INTEGER) | (1L << DECIMAL) | (1L << STRING))) != 0)) {
				{
				{
				State = 19; expression(0);
				}
				}
				State = 24;
				_errHandler.Sync(this);
				_la = _input.La(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.ReportError(this, re);
			_errHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ExpressionContext : ParserRuleContext {
		public ExpressionContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_expression; } }
	 
		public ExpressionContext() { }
		public virtual void CopyFrom(ExpressionContext context) {
			base.CopyFrom(context);
		}
	}
	public partial class DecimalLiteralContext : ExpressionContext {
		public DecimalContext @decimal() {
			return GetRuleContext<DecimalContext>(0);
		}
		public DecimalLiteralContext(ExpressionContext context) { CopyFrom(context); }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterDecimalLiteral(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitDecimalLiteral(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitDecimalLiteral(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class NullLiteralContext : ExpressionContext {
		public NullContext @null() {
			return GetRuleContext<NullContext>(0);
		}
		public NullLiteralContext(ExpressionContext context) { CopyFrom(context); }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterNullLiteral(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitNullLiteral(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitNullLiteral(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class StringLiteralContext : ExpressionContext {
		public StringContext @string() {
			return GetRuleContext<StringContext>(0);
		}
		public StringLiteralContext(ExpressionContext context) { CopyFrom(context); }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterStringLiteral(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitStringLiteral(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStringLiteral(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class MemberAccessExpContext : ExpressionContext {
		public ExpressionContext expression() {
			return GetRuleContext<ExpressionContext>(0);
		}
		public ITerminalNode DOT() { return GetToken(MelonParser.DOT, 0); }
		public ITerminalNode NAME() { return GetToken(MelonParser.NAME, 0); }
		public MemberAccessExpContext(ExpressionContext context) { CopyFrom(context); }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterMemberAccessExp(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitMemberAccessExp(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitMemberAccessExp(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class ParenthesisExpContext : ExpressionContext {
		public ITerminalNode LEFTPARENTHESIS() { return GetToken(MelonParser.LEFTPARENTHESIS, 0); }
		public ExpressionContext expression() {
			return GetRuleContext<ExpressionContext>(0);
		}
		public ITerminalNode RIGHTPARENTHESIS() { return GetToken(MelonParser.RIGHTPARENTHESIS, 0); }
		public ParenthesisExpContext(ExpressionContext context) { CopyFrom(context); }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterParenthesisExp(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitParenthesisExp(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitParenthesisExp(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class IntegerLiteralContext : ExpressionContext {
		public IntegerContext integer() {
			return GetRuleContext<IntegerContext>(0);
		}
		public IntegerLiteralContext(ExpressionContext context) { CopyFrom(context); }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterIntegerLiteral(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitIntegerLiteral(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitIntegerLiteral(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class BinaryOperationExpContext : ExpressionContext {
		public ExpressionContext Left;
		public IToken Operation;
		public ExpressionContext Right;
		public ExpressionContext[] expression() {
			return GetRuleContexts<ExpressionContext>();
		}
		public ExpressionContext expression(int i) {
			return GetRuleContext<ExpressionContext>(i);
		}
		public ITerminalNode EXPONENT() { return GetToken(MelonParser.EXPONENT, 0); }
		public ITerminalNode ASTERISK() { return GetToken(MelonParser.ASTERISK, 0); }
		public ITerminalNode SLASH() { return GetToken(MelonParser.SLASH, 0); }
		public ITerminalNode REMAINDER() { return GetToken(MelonParser.REMAINDER, 0); }
		public ITerminalNode PLUS() { return GetToken(MelonParser.PLUS, 0); }
		public ITerminalNode MINUS() { return GetToken(MelonParser.MINUS, 0); }
		public BinaryOperationExpContext(ExpressionContext context) { CopyFrom(context); }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterBinaryOperationExp(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitBinaryOperationExp(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBinaryOperationExp(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class BooleanLiteralContext : ExpressionContext {
		public BooleanContext boolean() {
			return GetRuleContext<BooleanContext>(0);
		}
		public BooleanLiteralContext(ExpressionContext context) { CopyFrom(context); }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterBooleanLiteral(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitBooleanLiteral(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBooleanLiteral(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ExpressionContext expression() {
		return expression(0);
	}

	private ExpressionContext expression(int _p) {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = State;
		ExpressionContext _localctx = new ExpressionContext(_ctx, _parentState);
		ExpressionContext _prevctx = _localctx;
		int _startState = 4;
		EnterRecursionRule(_localctx, 4, RULE_expression, _p);
		int _la;
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 35;
			_errHandler.Sync(this);
			switch (_input.La(1)) {
			case LEFTPARENTHESIS:
				{
				_localctx = new ParenthesisExpContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;

				State = 26; Match(LEFTPARENTHESIS);
				State = 27; expression(0);
				State = 28; Match(RIGHTPARENTHESIS);
				}
				break;
			case INTEGER:
				{
				_localctx = new IntegerLiteralContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				State = 30; integer();
				}
				break;
			case DECIMAL:
				{
				_localctx = new DecimalLiteralContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				State = 31; @decimal();
				}
				break;
			case STRING:
				{
				_localctx = new StringLiteralContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				State = 32; @string();
				}
				break;
			case BOOLEAN:
				{
				_localctx = new BooleanLiteralContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				State = 33; boolean();
				}
				break;
			case NULL:
				{
				_localctx = new NullLiteralContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				State = 34; @null();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			_ctx.stop = _input.Lt(-1);
			State = 51;
			_errHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(_input,3,_ctx);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.InvalidAltNumber ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) TriggerExitRuleEvent();
					_prevctx = _localctx;
					{
					State = 49;
					_errHandler.Sync(this);
					switch ( Interpreter.AdaptivePredict(_input,2,_ctx) ) {
					case 1:
						{
						_localctx = new BinaryOperationExpContext(new ExpressionContext(_parentctx, _parentState));
						((BinaryOperationExpContext)_localctx).Left = _prevctx;
						PushNewRecursionContext(_localctx, _startState, RULE_expression);
						State = 37;
						if (!(Precpred(_ctx, 8))) throw new FailedPredicateException(this, "Precpred(_ctx, 8)");
						State = 38; ((BinaryOperationExpContext)_localctx).Operation = Match(EXPONENT);
						State = 39; ((BinaryOperationExpContext)_localctx).Right = expression(8);
						}
						break;

					case 2:
						{
						_localctx = new BinaryOperationExpContext(new ExpressionContext(_parentctx, _parentState));
						((BinaryOperationExpContext)_localctx).Left = _prevctx;
						PushNewRecursionContext(_localctx, _startState, RULE_expression);
						State = 40;
						if (!(Precpred(_ctx, 7))) throw new FailedPredicateException(this, "Precpred(_ctx, 7)");
						State = 41;
						((BinaryOperationExpContext)_localctx).Operation = _input.Lt(1);
						_la = _input.La(1);
						if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << ASTERISK) | (1L << SLASH) | (1L << REMAINDER))) != 0)) ) {
							((BinaryOperationExpContext)_localctx).Operation = _errHandler.RecoverInline(this);
						} else {
							if (_input.La(1) == TokenConstants.Eof) {
								matchedEOF = true;
							}

							_errHandler.ReportMatch(this);
							Consume();
						}
						State = 42; ((BinaryOperationExpContext)_localctx).Right = expression(8);
						}
						break;

					case 3:
						{
						_localctx = new BinaryOperationExpContext(new ExpressionContext(_parentctx, _parentState));
						((BinaryOperationExpContext)_localctx).Left = _prevctx;
						PushNewRecursionContext(_localctx, _startState, RULE_expression);
						State = 43;
						if (!(Precpred(_ctx, 6))) throw new FailedPredicateException(this, "Precpred(_ctx, 6)");
						State = 44;
						((BinaryOperationExpContext)_localctx).Operation = _input.Lt(1);
						_la = _input.La(1);
						if ( !(_la==PLUS || _la==MINUS) ) {
							((BinaryOperationExpContext)_localctx).Operation = _errHandler.RecoverInline(this);
						} else {
							if (_input.La(1) == TokenConstants.Eof) {
								matchedEOF = true;
							}

							_errHandler.ReportMatch(this);
							Consume();
						}
						State = 45; ((BinaryOperationExpContext)_localctx).Right = expression(7);
						}
						break;

					case 4:
						{
						_localctx = new MemberAccessExpContext(new ExpressionContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, RULE_expression);
						State = 46;
						if (!(Precpred(_ctx, 9))) throw new FailedPredicateException(this, "Precpred(_ctx, 9)");
						State = 47; Match(DOT);
						State = 48; Match(NAME);
						}
						break;
					}
					} 
				}
				State = 53;
				_errHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(_input,3,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.ReportError(this, re);
			_errHandler.Recover(this, re);
		}
		finally {
			UnrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public partial class StringContext : ParserRuleContext {
		public string value;
		public IToken _STRING;
		public ITerminalNode STRING() { return GetToken(MelonParser.STRING, 0); }
		public StringContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_string; } }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterString(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitString(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitString(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public StringContext @string() {
		StringContext _localctx = new StringContext(_ctx, State);
		EnterRule(_localctx, 6, RULE_string);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 54; _localctx._STRING = Match(STRING);
			 
				if ((_localctx._STRING!=null?_localctx._STRING.Text:null).Length > 2) {
					var content = (_localctx._STRING!=null?_localctx._STRING.Text:null).Substring(1, (_localctx._STRING!=null?_localctx._STRING.Text:null).Length - 2);
					_localctx.value =  System.Text.RegularExpressions.Regex.Unescape(content);
				} else {
					_localctx.value =  "";
				}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.ReportError(this, re);
			_errHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class DecimalContext : ParserRuleContext {
		public double value;
		public IToken _DECIMAL;
		public ITerminalNode DECIMAL() { return GetToken(MelonParser.DECIMAL, 0); }
		public DecimalContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_decimal; } }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterDecimal(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitDecimal(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitDecimal(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public DecimalContext @decimal() {
		DecimalContext _localctx = new DecimalContext(_ctx, State);
		EnterRule(_localctx, 8, RULE_decimal);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 57; _localctx._DECIMAL = Match(DECIMAL);
			 
				_localctx.value =  double.Parse((_localctx._DECIMAL!=null?_localctx._DECIMAL.Text:null)); 

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.ReportError(this, re);
			_errHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class IntegerContext : ParserRuleContext {
		public int value;
		public IToken _INTEGER;
		public ITerminalNode INTEGER() { return GetToken(MelonParser.INTEGER, 0); }
		public IntegerContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_integer; } }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterInteger(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitInteger(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitInteger(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public IntegerContext integer() {
		IntegerContext _localctx = new IntegerContext(_ctx, State);
		EnterRule(_localctx, 10, RULE_integer);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 60; _localctx._INTEGER = Match(INTEGER);

				_localctx.value =  int.Parse((_localctx._INTEGER!=null?_localctx._INTEGER.Text:null)); 

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.ReportError(this, re);
			_errHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class BooleanContext : ParserRuleContext {
		public bool value;
		public IToken _BOOLEAN;
		public ITerminalNode BOOLEAN() { return GetToken(MelonParser.BOOLEAN, 0); }
		public BooleanContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_boolean; } }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterBoolean(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitBoolean(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBoolean(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public BooleanContext boolean() {
		BooleanContext _localctx = new BooleanContext(_ctx, State);
		EnterRule(_localctx, 12, RULE_boolean);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 63; _localctx._BOOLEAN = Match(BOOLEAN);
			 
				_localctx.value =  (_localctx._BOOLEAN!=null?_localctx._BOOLEAN.Text:null) == "true" ? true : false; 

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.ReportError(this, re);
			_errHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class NullContext : ParserRuleContext {
		public object value;
		public ITerminalNode NULL() { return GetToken(MelonParser.NULL, 0); }
		public NullContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_null; } }
		public override void EnterRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.EnterNull(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IMelonListener typedListener = listener as IMelonListener;
			if (typedListener != null) typedListener.ExitNull(this);
		}
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMelonVisitor<TResult> typedVisitor = visitor as IMelonVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitNull(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public NullContext @null() {
		NullContext _localctx = new NullContext(_ctx, State);
		EnterRule(_localctx, 14, RULE_null);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 66; Match(NULL);
			 
				_localctx.value =  null; 

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.ReportError(this, re);
			_errHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public override bool Sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 2: return expression_sempred((ExpressionContext)_localctx, predIndex);
		}
		return true;
	}
	private bool expression_sempred(ExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0: return Precpred(_ctx, 8);

		case 1: return Precpred(_ctx, 7);

		case 2: return Precpred(_ctx, 6);

		case 3: return Precpred(_ctx, 9);
		}
		return true;
	}

	public static readonly string _serializedATN =
		"\x3\xAF6F\x8320\x479D\xB75C\x4880\x1605\x191C\xAB37\x3,H\x4\x2\t\x2\x4"+
		"\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b\x4\t\t\t\x3"+
		"\x2\x3\x2\x3\x2\x3\x3\a\x3\x17\n\x3\f\x3\xE\x3\x1A\v\x3\x3\x4\x3\x4\x3"+
		"\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x5\x4&\n\x4\x3\x4\x3\x4"+
		"\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\x3\x4\a\x4\x34"+
		"\n\x4\f\x4\xE\x4\x37\v\x4\x3\x5\x3\x5\x3\x5\x3\x6\x3\x6\x3\x6\x3\a\x3"+
		"\a\x3\a\x3\b\x3\b\x3\b\x3\t\x3\t\x3\t\x3\t\x2\x2\x3\x6\n\x2\x2\x4\x2\x6"+
		"\x2\b\x2\n\x2\f\x2\xE\x2\x10\x2\x2\x4\x4\x2\x12\x13\x16\x16\x3\x2\x14"+
		"\x15I\x2\x12\x3\x2\x2\x2\x4\x18\x3\x2\x2\x2\x6%\x3\x2\x2\x2\b\x38\x3\x2"+
		"\x2\x2\n;\x3\x2\x2\x2\f>\x3\x2\x2\x2\xE\x41\x3\x2\x2\x2\x10\x44\x3\x2"+
		"\x2\x2\x12\x13\x5\x4\x3\x2\x13\x14\a\x2\x2\x3\x14\x3\x3\x2\x2\x2\x15\x17"+
		"\x5\x6\x4\x2\x16\x15\x3\x2\x2\x2\x17\x1A\x3\x2\x2\x2\x18\x16\x3\x2\x2"+
		"\x2\x18\x19\x3\x2\x2\x2\x19\x5\x3\x2\x2\x2\x1A\x18\x3\x2\x2\x2\x1B\x1C"+
		"\b\x4\x1\x2\x1C\x1D\a\x5\x2\x2\x1D\x1E\x5\x6\x4\x2\x1E\x1F\a\x6\x2\x2"+
		"\x1F&\x3\x2\x2\x2 &\x5\f\a\x2!&\x5\n\x6\x2\"&\x5\b\x5\x2#&\x5\xE\b\x2"+
		"$&\x5\x10\t\x2%\x1B\x3\x2\x2\x2% \x3\x2\x2\x2%!\x3\x2\x2\x2%\"\x3\x2\x2"+
		"\x2%#\x3\x2\x2\x2%$\x3\x2\x2\x2&\x35\x3\x2\x2\x2\'(\f\n\x2\x2()\a\x17"+
		"\x2\x2)\x34\x5\x6\x4\n*+\f\t\x2\x2+,\t\x2\x2\x2,\x34\x5\x6\x4\n-.\f\b"+
		"\x2\x2./\t\x3\x2\x2/\x34\x5\x6\x4\t\x30\x31\f\v\x2\x2\x31\x32\a\x3\x2"+
		"\x2\x32\x34\a%\x2\x2\x33\'\x3\x2\x2\x2\x33*\x3\x2\x2\x2\x33-\x3\x2\x2"+
		"\x2\x33\x30\x3\x2\x2\x2\x34\x37\x3\x2\x2\x2\x35\x33\x3\x2\x2\x2\x35\x36"+
		"\x3\x2\x2\x2\x36\a\x3\x2\x2\x2\x37\x35\x3\x2\x2\x2\x38\x39\a(\x2\x2\x39"+
		":\b\x5\x1\x2:\t\x3\x2\x2\x2;<\a\'\x2\x2<=\b\x6\x1\x2=\v\x3\x2\x2\x2>?"+
		"\a&\x2\x2?@\b\a\x1\x2@\r\x3\x2\x2\x2\x41\x42\a$\x2\x2\x42\x43\b\b\x1\x2"+
		"\x43\xF\x3\x2\x2\x2\x44\x45\a#\x2\x2\x45\x46\b\t\x1\x2\x46\x11\x3\x2\x2"+
		"\x2\x6\x18%\x33\x35";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
} // namespace MelonLanguage.Grammar