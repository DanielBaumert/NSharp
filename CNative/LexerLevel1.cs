namespace CNative;

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using static LexerNodeType;

internal enum LexerNodeType
{
    Space, 
    Word,
    Number,
    Symbol
}

internal struct LexerNode
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

internal class LexerLevel1
{
    private string _src;
    private Queue<LexerNode> _queue;

    public LexerLevel1(
        string src,
        ref Queue<LexerNode> level1LexerQueue)
    {
        _src = src;
        _queue = level1LexerQueue;
    }

    public void Analyse()
    {
        ReadOnlySpan<char> sourceCode = _src.ToCharArray();
        int i = 0;

        while (i < sourceCode.Length)
        {
            Unsafe.SkipInit(out LexerNode node);

            if (IsSpace(sourceCode, in i, ref node) 
                || IsDigit(_src, in i, ref node)
                || IsWord(_src, in i, ref node))
            {
                _queue.Enqueue(node);
            }
            else
            {
                node = new LexerNode(Symbol, i, i + 1);
                _queue.Enqueue(node);
            }

            i = node.End;
        }
    }
    
    private static bool IsSpace(ReadOnlySpan<char> src, in int i, ref LexerNode node) {
        if (src[i] is ' ' or '\t' or '\r' or '\n')
        {
            node = new LexerNode(Space, i, i + 1);
            return true;
        }

        return false;
    }

    private static bool IsDigit(ReadOnlySpan<char> src, in int i, ref LexerNode node) {
        if(src[i] is >= '0' and <= '9')
        {
            int end = i + 1;
            while (end < src.Length && src[end] is >= '0' and <= '9')
            {
                end++;
            }

            node = new LexerNode(Number, i, end);
            return true;
        }

        return false;
    }

    private static bool IsWord(ReadOnlySpan<char> src, in int i, ref LexerNode node)
    {
        if (src[i] is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'))
        {
            int end = i + 1;
            while (end < src.Length && src[end] is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'))
            {
                end++;
            }

            node = new LexerNode(Word, i, end);
            return true;
        }

        return false;
    }
}
