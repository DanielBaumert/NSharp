namespace NSharp.Lex;

using System.Diagnostics;

[DebuggerDisplay("Type = {Type} [{Start}..{End})")]
internal readonly struct LexerToken
{
    public readonly LexerTokenType Type;
    public readonly int Start;
    public readonly int End;

    public LexerToken( LexerTokenType type, int start, int end)
    {
        Type = type;
        Start = start;
        End = end;
    }
}
