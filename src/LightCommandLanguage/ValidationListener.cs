using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace LightCommandLanguage
{
    public class ValidationListener : BaseErrorListener
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
