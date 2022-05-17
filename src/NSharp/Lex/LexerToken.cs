namespace NSharp.Lex;

using System.Diagnostics;

[DebuggerDisplay("Type = {Type} [{Start}..{End})")]
internal readonly struct LexerToken
{
    public readonly LexerTokenType Type;
    public readonly LexerNodeType Kind;
    public readonly int Start;
    public readonly int End;

    public LexerToken(LexerNodeType kind, LexerTokenType type, int start, int end)
    {
        Kind = kind;
        Type = type;
        Start = start;
        End = end;
    }
}
