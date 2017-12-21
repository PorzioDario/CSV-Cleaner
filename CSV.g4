grammar CSV;

@parser::header {
using System;
}

csvFile
locals [int col = 0]	
	: hdr {$col = $hdr.nCol;} (row[$col])+ ; 

hdr 
returns [int nCol]
	: row[0] {$nCol = $row.count;};

row[int col]
returns [int count = 0, int fields = 0]
	: field {$fields += $field.value; $count++;} 
	( {$col==0 || $col>$count}? SEP field {$fields += $field.value; $count++;} )* ({$col!=0}? SEP)* '\r'? '\n' ;
	
field
returns [int value = 0]
    : TEXT {$value = 1;}
    | STRING {$value = 1;}
    |
    ;

SEP		:	';' ;
TEXT	:	~[;\n\r"]+ ;
STRING	:	'"' ('""'|~'"')* '"' ; 