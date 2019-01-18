using System;
using Antlr4.Runtime;

namespace LightCommandParser
{
    public static class LightCommandAnalyzer
    {
        public static ValidationListener AnalyzeScript(string script)
        {
            var inputStream = new AntlrInputStream(script);
            var lexer = new LightCommandLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new LightCommandParser(tokenStream);
            var listener = new ValidationListener();
            parser.RemoveErrorListeners();
            parser.AddErrorListener(listener);
            parser.script();
            return listener;
        }
    }
}
