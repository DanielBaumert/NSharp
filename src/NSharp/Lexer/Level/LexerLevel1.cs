using System.Runtime.CompilerServices;

namespace NSharp.Lex.Level;
using static LexerNodeType;

internal static class LexerLevel1
{
    public static void Analyse(in ReadOnlySpan<char> src, Queue<LexerNode> queue)
    {
        int i = 0;

        while (i < src.Length)
        {
            Unsafe.SkipInit(out LexerNode node);

            if (IsSpace(in src, in i, ref node) 
                || IsDigit(in src, in i, ref node)
                || IsWord(in src, in i, ref node))
            {
                queue.Enqueue(node);
            }
            else
            {
                node = new LexerNode(Symbol, i, i + 1);
                queue.Enqueue(node);
            }

            i = node.End;
        }
    }
    
    private static bool IsSpace(in ReadOnlySpan<char> src, in int i, ref LexerNode node) {
        if (src[i] is ' ' or '\t' or '\r' or '\n')
        {
            node = new LexerNode(Empty, i, i + 1);
            return true;
        }

        return false;
    }

    private static bool IsDigit(in ReadOnlySpan<char> src, in int i, ref LexerNode node) {
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

    private static bool IsWord(in ReadOnlySpan<char> src, in int i, ref LexerNode node)
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
