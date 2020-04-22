grammar Melon;

/*
 * Parser Rules
 */
program : block EOF ;
block : ( expression )*;

expression : 
	LEFTPARENTHESIS expression RIGHTPARENTHESIS 
	#parenthesisExp
	| expression DOT NAME
	#memberAccessExp
	| <assoc=right>	Left=expression Operation=EXPONENT Right=expression			
	#binaryOperationExp
    | Left=expression Operation=(ASTERISK|SLASH|REMAINDER) Right=expression				
	#binaryOperationExp
    | Left=expression Operation=(PLUS|MINUS) Right=expression				
	#binaryOperationExp
	| integer																									
	#integerLiteral
	| decimal																									
	#decimalLiteral
	| string																									
	#stringLiteral
	| boolean																									
	#booleanLiteral
	| null																										
	#nullLiteral
;

string returns [string value] : STRING { 
	if ($STRING.text.Length > 2) {
		var content = $STRING.text.Substring(1, $STRING.text.Length - 2);
		$value = System.Text.RegularExpressions.Regex.Unescape(content);
	} else {
		$value = "";
	}
} ;

decimal returns [double value] : DECIMAL { 
	$value = double.Parse($DECIMAL.text); 
} ;

integer returns [int value] : INTEGER {
	$value = int.Parse($INTEGER.text); 
} ;

boolean returns [bool value] : BOOLEAN { 
	$value = $BOOLEAN.text == "true" ? true : false; 
} ;

null returns [object value] : NULL { 
	$value = null; 
} ;

/*
 * Lexer Rules
 */

fragment LETTER			: [a-zA-Z] ;
fragment DIGIT			: [0-9] ;
fragment ESCAPED_QUOTE	: '\\"';
fragment TRUE			: 'true';
fragment FALSE			: 'false';

DOT						: '.' ;
COMMA					: ',' ;
LEFTPARENTHESIS			: '(' ;
RIGHTPARENTHESIS		: ')' ;

IF						: 'if' ;
ELSE					: 'else' ;

LESS					: '<'	;
LESSEQ					: '<='	;
GREATER					: '>'	;
GREATEREQ				: '>='	;
EQUAL					: '=='	;
NOTEQUAL				: '!='	;
IS						: 'is' ;

AND						: '&&' ;
OR						: '||' ;

ASTERISK				: '*'	;
SLASH					: '/'	;
PLUS					: '+'	;
MINUS					: '-'	;
REMAINDER				: '%'	;
EXPONENT				: '**'	;

BITAND					: '&' ;
BITOR					: '^' ;
BITXOR					: '|' ; 
BITSHIFTL				: '<<' ; 
BITSHIFTR				: '>>' ; 
BITSHIFTUR				: '>>>' ; 

INCREMENT				: '++'	;
DECREMENT				: '--'	;
NOT						: '!' ;
BITNOT					: '~' ;

ASSIGN					: '='	;

NULL					: 'null' ;

BOOLEAN					: TRUE | FALSE ;

NAME					: ('_' | LETTER) ('_' | LETTER | DIGIT)*;

INTEGER					: DIGIT+;

DECIMAL					: DIGIT+ ('.' DIGIT+)?;

STRING					: '"' ~('"')* ('"' | {
	throw new System.Exception("Unterminated string!");
}) ;

WHITESPACE				: [ \n\t\r]+ -> channel(HIDDEN);

COMMENT					: '/*' .*? '*/' -> skip ;

LINE_COMMENT			: '//' ~[\r\n]* -> skip ;

// handle characters which failed to match any other token
ErrorCharacter : . ;