using NSharp.Lex;

namespace NSharp.Parser.Utils;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
internal class ParserFuntionAttribute : Attribute
{
    public LexerTokenType Type { get; init; }
    public ParserFuntionAttribute(LexerTokenType type)
    {
        Type = type;
    }
}
 