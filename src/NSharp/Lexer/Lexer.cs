using NSharp.Lex.Level;

namespace NSharp.Lex;

internal static class Lexer
{
    public static Queue<LexerToken> Analyse(in ReadOnlySpan<char> src)
    {
        Queue<LexerNode> level1Queue = new Queue<LexerNode>();
        Queue<LexerToken> level2Queue = new Queue<LexerToken>();

        LexerLevel1.Analyse(in src, level1Queue);
        LexerLevel2.Analyse(in src, level1Queue, level2Queue);

        return level2Queue;
    }
}
