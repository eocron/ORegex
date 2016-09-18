grammar RegexGrammar;

options
{
	output=AST;
}
@parser::header {#pragma warning disable 3021}
@lexer::header {#pragma warning disable 3021}
////////////////////////////////////////////////////////////////////////////////
////////////////////////////PARSER RULES////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

unOper      : term UNOper?
            ;

concat      : unOper+
            ;

binOper     : concat (BINOper concat)*
            ;

expr        : BeginOper? binOper EndOper?
            ;

term        : atom
            | group
            ;

group       : OBR3 binOper CBR3
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

ANY         : '.'                                                               //always true predicate
            ;

NAME        : OBR1 ID CBR1                                                      //named predicate
            | OBR1 INT CBR1                                                     //id predicate
            ;

ID          : ('a'..'z'|'A'..'Z'|'_' |'à'..'ÿ'|'À'..'ß') ('a'..'z'|'A'..'Z'|'0'..'9'|'_'|'à'..'ÿ'|'À'..'ß')*
            ;

INT         : '0'..'9'+
            ;

COMMENT     :   ( '//' ~('\n'|'\r')* '\r'? '\n'
                | '/*' ( . )*? '*/'
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
BR3Oper     : '?' CName                                                         //capture group quantifier
            | '?='                                                              //forward positive lookahead
            | '?!'                                                              //forward negative lookahead
            | '?<='                                                             //backward positive lookahead
            | '?<!'                                                             //backward negative lookahead
            ;

CName       : OBR4 ID CBR4                                                      //capture group name bracket syntax
            | Q1 ID Q1                                                          //capture group name quote syntax
            ;
OBR4        : '<';
CBR4        : '>';

Q1          : '\''
            ;

UNOper      : '*' Greed?                                                        //zero or infinity repetition
            | '+' Greed?                                                        //one or infinity repetition
            | '?' Greed?                                                        //zero or one
            | OBR1 INT ',' INT? CBR1 Greed?                                     //counted repetition
            ;

Greed       : '?'
            ;

BINOper     : '|'                                                               //or operator
            ;

BeginOper   : '^'                                                               //start of sequence quantifier
            ;

EndOper     : '$'                                                               //end of sequence quantifier
            ;

ErrorChar   : .
            ;
