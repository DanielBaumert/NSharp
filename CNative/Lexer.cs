using System.Collections.Concurrent;

namespace CNative;

internal class Lexer
{
    private string _src;
    public ConcurrentQueue<LexerNode>  level1LexerQueue;
    public ConcurrentQueue<LexerToken> level2LexerQueue;
    public Lexer(string src)
    {
        _src = src;
    }

    public async Task AnalyseAsync()
    {
        level1LexerQueue = new ConcurrentQueue<LexerNode>();
        level2LexerQueue = new ConcurrentQueue<LexerToken>();

        LexerLevel1 level1 = new LexerLevel1(_src, ref level1LexerQueue);
        LexerLevel2 level2 = new LexerLevel2(_src, ref level1LexerQueue, ref level2LexerQueue);

        CancellationTokenSource cts = new CancellationTokenSource();

        Task level1Task = level1.AnalyseAsync(cts, cts.Token);
        Task level2Task = level2.AnalyseAsync(cts.Token);

        await level1Task;
        await level2Task;
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
