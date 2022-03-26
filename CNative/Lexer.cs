using System.Collections.Concurrent;

namespace CNative;

internal class Lexer
{
    private string _src;
    public Queue<LexerNode>  level1LexerQueue;
    public Queue<LexerToken> level2LexerQueue;
    public Lexer(string src)
    {
        _src = src;
    }

    public void Analyse()
    {
        level1LexerQueue = new Queue<LexerNode>();
        level2LexerQueue = new Queue<LexerToken>();

        LexerLevel1 level1 = new LexerLevel1(_src, ref level1LexerQueue);
        LexerLevel2 level2 = new LexerLevel2(_src, ref level1LexerQueue, ref level2LexerQueue);

        level1.Analyse();
        level2.Analyse();
    }

    private static bool IsAlphabetic(ReadOnlySpan<char> src, int i, out int end)
    {
        if(src[i] is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'))
        {
            end = i + 1;
            return true;
        }

        end = 0;
        return false;
    }
    private static bool IsNumericLiteral(ReadOnlySpan<char> src, int i, out int end)
    {
        if (src[i] is >= '0' and <= '9')
        {
            end = i + 1;
            return true;
        }

        end = 0;
        return false;
    }
}
