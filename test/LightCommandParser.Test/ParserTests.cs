using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Xunit;
using Xunit.Abstractions;

namespace LightCommandParser.Test
{
    public class ParserTests
    {
        public ParserTests(ITestOutputHelper output)
        {
            this.Output = output;
        }

        public ITestOutputHelper Output { get; private set; }

        [Theory]
        [InlineData("setup 1")]
        [InlineData("setup 1,100,3;init")]
        public void InvalidScripts(string script)
        {
            var listener = AnalyzeScript(script);
            Assert.NotEqual(0, listener.ErrorCount);
        }

        [Theory]
        [InlineData("setup 1,100;init;")]
        [InlineData("setup 1,100,3;init;")]
        [InlineData("setup 1,100,3,0,255,18;init;")]
        [InlineData("setup 1,100,3;init;render;")]
        [InlineData("setup 1,100,3;init;thread_start;render;thread_stop;")]
        public void ValidScripts(string script)
        {
            var listener = AnalyzeScript(script);
            if (listener.ErrorCount != 0)
            {
                foreach (var error in listener.Errors)
                {
                    this.Output.WriteLine("error: {0}", error);
                }
            }

            Assert.Equal(0, listener.ErrorCount);
        }

        private static ValidationListener AnalyzeScript(string script)
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

        private class ValidationListener : BaseErrorListener
        {
            public int ErrorCount { get; private set; }

            public IList<string> Errors { get; } = new List<string>();

            public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
            {
                this.ErrorCount++;
                this.Errors.Add(msg);
            }
        }
    }
}
