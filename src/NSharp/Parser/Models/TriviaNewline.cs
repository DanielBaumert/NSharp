using NSharp.Lex;

namespace NSharp.Parser.Models;

internal class TriviaNewline : SyntaxTriviaNode
{
    public TriviaNewline()
    {
        Type = LexerTokenType.Newline;
    }
}
