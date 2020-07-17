grammar Melon;

/*
 * Parser Rules
 */
program : block EOF ;
block : ( variableDefinition | functionDefinition | assignment | computedMemberAssigment | while  | expression | return)*;
 
while : WHILE expression '{' block '}' 
#whileStatement
;

functionDefinition : FN (ReturnType=name)? Name=name '(' (Parameters=parameters)? ')' '{' Block=block '}' 
#functionDefinitionStatement
;

parameters: parameter (',' parameter)*;

parameter:  (Type=type)? VARARGS? Name=name ('=' Expression=expression)?;

return: RETURN Expression=expression?
#returnStatement
;

variableDefinition: (Type=type | LET) Name=name ASSIGN expression 
#variableDefinitionStatement
;

type: name genericParameters? ;

genericParameters: '<' type (',' type)* '>' ;

assignment : name ASSIGN expression 
#assignStatement
;

computedMemberAssigment: name '[' Index=expression ']' ASSIGN Expression=expression 
#computedMemberAssignStatement
;

expressionGroup: (expression (',' expression)*)?;

expression: 
	LEFTPARENTHESIS expression RIGHTPARENTHESIS 
	#parenthesisExp
	| expression DOT name
	#memberAccessExp
	| expression '[' expression ']'																				
	#computedMemberAccessExp
	| Function=expression genericParameters? LEFTPARENTHESIS Arguments=expressionGroup  RIGHTPARENTHESIS
	#callExp
	| <assoc=right>	Left=expression Operation=EXPONENT Right=expression	
	#binaryOperationExp
    | Left=expression Operation=(ASTERISK|SLASH|REMAINDER) Right=expression				
	#binaryOperationExp
    | Left=expression Operation=(PLUS|MINUS) Right=expression				
	#binaryOperationExp
	| Left=expression Operation=(LESS|LESSEQ|GREATER|GREATEREQ)	Right=expression			
	#binaryOperationExp
    | Left=expression Operation=(EQUAL|NOTEQUAL)	Right=expression			
	#binaryOperationExp
	| array
	#arrayLiteral
	| name
	#nameExp
	| integer																									
	#integerLiteral
	| float																									
	#floatLiteral
	| string																									
	#stringLiteral
	| boolean																									
	#booleanLiteral
	| null																										
	#nullLiteral
;

array: '[' expressionGroup ']' ;

name returns [string value] : NAME { 
	$value = $NAME.text;
};

string returns [string value] : STRING { 
	if ($STRING.text.Length > 2) {
		var content = $STRING.text.Substring(1, $STRING.text.Length - 2);
		$value = System.Text.RegularExpressions.Regex.Unescape(content);
	} else {
		$value = "";
	}
};

float returns [double value] : FLOAT { 
	$value = double.Parse($FLOAT.text); 
};

integer returns [int value] : INTEGER {
	$value = int.Parse($INTEGER.text); 
};

boolean returns [bool value] : BOOLEAN { 
	$value = $BOOLEAN.text == "true" ? true : false; 
};

null returns [object value] : NULL { 
	$value = null; 
};

/*
 * Lexer Rules
 */

fragment LETTER			: [a-zA-Z] ;
fragment DIGIT			: [0-9] ;
fragment ESCAPED_QUOTE	: '\\"';
fragment TRUE			: 'true';
fragment FALSE			: 'false';

DOT						: '.';
COMMA					: ',';
LEFTPARENTHESIS			: '(';
RIGHTPARENTHESIS		: ')';

LET						: 'let' ;
FN						: 'fn' ;

IF						: 'if';
ELSE					: 'else';
WHILE					: 'while';

RETURN					: 'return';
VARARGS					: '...';

LESS					: '<';
LESSEQ					: '<=';
GREATER					: '>';
GREATEREQ				: '>=';
NOTEQUAL				: 'is not';
EQUAL					: 'is';

AND						: 'and';
OR						: 'or';

ASTERISK				: '*';
SLASH					: '/';
PLUS					: '+';
MINUS					: '-';
REMAINDER				: '%';
EXPONENT				: '**';

BITAND					: '&';
BITOR					: '^';
BITXOR					: '|'; 
BITSHIFTL				: '<<'; 
BITSHIFTR				: '>>'; 
BITSHIFTUR				: '>>>'; 

INCREMENT				: '++';
DECREMENT				: '--';
NOT						: '!' ;
BITNOT					: '~' ;

ASSIGN					: '=';

NULL					: 'null' ;

BOOLEAN					: TRUE | FALSE ;

NAME					: ('_' | LETTER) ('_' | LETTER | DIGIT)*;

INTEGER					: DIGIT+;

FLOAT					: DIGIT+ ('.' DIGIT+)?;

STRING					: '"' ~('"')* ('"' | {
	throw new System.Exception("Unterminated string!");
}) ;

WHITESPACE				: [ \n\t\r]+ -> channel(HIDDEN);

COMMENT					: '/*' .*? '*/' -> skip ;

LINE_COMMENT			: '//' ~[\r\n]* -> skip ;

// handle characters which failed to match any other token
ErrorCharacter : . ;