// https://github.com/tom-2015/rpi-ws2812-server
grammar LightCommand;

script : setup init instructions? EOF ;

setup : WS* 'setup' WS+ DIGIT ',' DIGIT+ (',' DIGIT (',' DIGIT (',' DIGIT+ (',' DIGIT+)?)?)?)? SEMI;

init : WS* 'init' SEMI ;

instructions : threaded | body ;

threaded : WS* 'thread_start' SEMI body WS* 'thread_stop' SEMI ;

// body : render | rotate | rainbow | fill | delay | brightness | fade | gradient | random | blink | loop ;
body : render | loop ;

loop : WS* 'do' SEMI body WS* 'loop' (WS+ DIGIT+)? SEMI ;

render : WS* 'render' (WS+ DIGIT+ (WS+ DIGIT+ (WS+ COLOR)?)?)? SEMI ;

COLOR : [a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9] ;

DIGIT : [0-9] ;

SEMI : ';' ;

WS : ' ' ;