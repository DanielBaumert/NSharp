using NSharp.Lex;

namespace NSharp.Parser.Models;

internal class TriviaTab : SyntaxTriviaNode
{
    public TriviaTab()
    {
        Type = LexerTokenType.Tab;
    }
}