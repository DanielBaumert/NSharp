using NSharp.Lex;

namespace NSharp.Parser.Models;

internal class TriviaWhitespace : SyntaxTriviaNode
{
    public TriviaWhitespace()
    {
        Type = LexerTokenType.Whitespace;
    }
}
