// https://github.com/tom-2015/rpi-ws2812-server
grammar LightCommand;

script : setup init instructions? EOF ;

setup : WS* 'setup' WS+ DIGIT ',' DIGIT+ (',' DIGIT (',' DIGIT (',' DIGIT+ (',' DIGIT+)?)?)?)? SEMI;

init : WS* 'init' SEMI ;

instructions : threaded | body ;

threaded : WS* 'thread_start' SEMI body WS* 'thread_stop' SEMI ;

body : render | rotate | rainbow | fill | delay | brightness | fade | gradient | random | blink | loop ;

loop : WS* 'do' SEMI body WS* 'loop' (WS+ DIGIT+)? SEMI ;

render : WS* 'render' (WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ COLOR)?)?)? SEMI ;

rotate : WS* 'rotate' (WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ COLOR)?)?)?)? SEMI ;

rainbow : WS* 'rainbow' (WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+)?)?)?)? SEMI ;

fill : WS* 'fill' (WS+ DIGIT+ (',' WS+ COLOR (',' WS+ DIGIT+ (',' WS+ DIGIT+)?)?)?)? SEMI ;

delay : WS* 'delay' WS+ DIGIT+ SEMI ;

brightness : WS* 'brightness' (WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+)?)?)?)? SEMI ;

fade : WS* 'fade' (WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+)?)?)?)?)?)?)? SEMI ;

gradient : WS* 'gradient' WS+ DIGIT+ ',' WS+ RGBWL (',' WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+ (',' WS+ DIGIT+)?)?)?)? SEMI ;

random : WS* 'random' WS+ DIGIT+ ',' WS+ DIGIT+ ',' WS+ DIGIT+ ',' WS+ RGBWL+ SEMI ;

blink : WS* 'blink' WS+ DIGIT+ ',' WS+ COLOR ',' WS+ COLOR ',' WS+ DIGIT+ ',' WS+ DIGIT+ ',' WS+ DIGIT+ ',' WS+ DIGIT+ SEMI ;

COLOR : [a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9][a-fA-F0-9] ;

RGBWL : 'R' | 'G' | 'B' | 'W' | 'L' ;

DIGIT : [0-9] ;

SEMI : ';' ;

WS : ' ' ;