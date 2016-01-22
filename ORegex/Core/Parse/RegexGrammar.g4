grammar RegexGrammar;

///PARSER RULES///
unOper      : term UNOper?
            ;

binOper     : unOper ('|' unOper)*
            ;

expr        : binOper+
            ;

term        : atom
            | group
            ;

group       : OBR3 expr+ CBR3
            ;

atom        : NAME
            | ANY
            | OBR2 NAME+ CBR2
            ;

///LEXER RULES///

ANY :   '.'
    ;

NAME: OBR1 ID CBR1
    | OBR1 INT CBR1
    ;

ID  :	('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'0'..'9'|'_')*
    ;

INT :	'0'..'9'+
    ;

COMMENT
    :   ( '//' ~('\n'|'\r')* '\r'? '\n'
        | '/*' ( options {greedy=false;} : . )* '*/'
        ) -> skip
    ;

WS  :   ( ' '
        | '\t'
        | '\r'
        | '\n'
        ) -> skip
    ;

OBR1:   '{';
CBR1:   '}';
                
OBR2:   '[';
CBR2:   ']';

OBR3:   '(' BR3Oper?;
CBR3:   ')';
BR3Oper : '?=' 
        | '?' CName
        ;

CName   : OBR4 ID CBR4
        ;
OBR4:   '<';
CBR4:   '>';

UNOper  :   '*'
        |   '+'
        |   '*?'
        |   '+?'
        |   '?'
        ;

