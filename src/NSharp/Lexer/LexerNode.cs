using System.Diagnostics;

namespace NSharp.Lex;

[DebuggerDisplay("Type = {Type} [{Start}..{End})")]
internal readonly struct LexerNode
{
    public readonly LexerNodeType Type;
    public readonly int Start;
    public readonly int End;

    public LexerNode(LexerNodeType type, int start, int end)
    {
        Type = type;
        Start = start;
        End = end;
    }
}
