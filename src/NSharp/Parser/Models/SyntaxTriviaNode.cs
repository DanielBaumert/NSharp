using NSharp.Lex;

namespace NSharp.Parser.Models;

internal abstract class SyntaxTriviaNode : SyntaxNode
{
    public LexerTokenType Type { get; init; }
}
