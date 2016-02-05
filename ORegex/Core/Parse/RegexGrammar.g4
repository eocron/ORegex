grammar RegexGrammar;

options
{
	output=AST;
}

////////////////////////////////////////////////////////////////////////////////
////////////////////////////PARSER RULES////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

unOper      : term UNOper?
            ;

concat      : unOper+
            ;

binOper     : concat ('|' concat)*
            ;

expr        : BeginOper? binOper+ EndOper?
            ;

term        : atom
            | group
            ;

group       : OBR3 binOper+ CBR3
            ;

atom        : natom
            | any
            | OBR2 natom+ CBR2
            ;

any         : ANY
            ;

natom       : NAME
            ;

////////////////////////////////////////////////////////////////////////////////
////////////////////////////LEXER RULES/////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

ANY         : '.'
            ;

NAME        : OBR1 ID CBR1
            | OBR1 INT CBR1
            ;

ID          : ('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'0'..'9'|'_')*
            ;

INT         : '0'..'9'+
            ;

COMMENT     :   ( '//' ~('\n'|'\r')* '\r'? '\n'
                | '/*' ( options {greedy=false;} : . )* '*/'
                ) -> skip
            ;

WS          :   ( ' '
                | '\t'
                | '\r'
                | '\n'
                ) -> skip
            ;

OBR1        : '{';
CBR1        : '}';
                
OBR2        : '[' BeginOper?;
CBR2        : ']';

OBR3        : '(' BR3Oper?;
CBR3        : ')';
BR3Oper     : '?' CName
            | '?='
            | '?!'
            | '?<='
            | '?<!'
            ;

CName       : OBR4 ID CBR4
            | '\'' ID '\''
            ;
OBR4        : '<';
CBR4        : '>';

UNOper      : '*'                                                               //zero or infinity repetition
            | '+'                                                               //one or infinity repetition
            | '?'                                                               //zero or one
            | '*?'                                                              //zero or infinity repetition greedy
            | '+?'                                                              //one or infinity repetition greedy
            | '??'                                                              //zero or one greedy
            ;

BeginOper   : '^'
            ;

EndOper     : '$'
            ;
