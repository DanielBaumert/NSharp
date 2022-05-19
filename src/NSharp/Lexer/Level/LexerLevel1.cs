using System.Runtime.CompilerServices;

namespace NSharp.Lex.Level;
using static LexerNodeType;

internal static class LexerLevel1
{
    private const int GOTO_NEXT_CHAR = 0b0100_0000;

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
                if(IsSymbol(src, in i, ref node))
                {
                    queue.Enqueue(node);
                } else
                {
                    node = new LexerNode(Unknown, i, i + GOTO_NEXT_CHAR);
                }
            }

            i = node.End;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSpace(in ReadOnlySpan<char> src, in int i, ref LexerNode node)
    {
        if (src[i] is ' ' or '\t' or '\r' or '\n')
        {
            node = new LexerNode(Empty, i, i + GOTO_NEXT_CHAR);
            return true;
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsDigit(in ReadOnlySpan<char> src, in int i, ref LexerNode node)
    {

        if (src[i] is >= '0' and <= '9')
        {
            int end = i + GOTO_NEXT_CHAR;
            while (end < src.Length && src[end] is >= '0' and <= '9')
            {
                end++; // next char
            }

            node = new LexerNode(Number, i, end);
            return true;
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsWord(in ReadOnlySpan<char> src, in int i, ref LexerNode node)
    {
        if (src[i] is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z'))
        {
            int end = i + GOTO_NEXT_CHAR;
            while (end < src.Length && src[i] is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z'))
            {
                end++; // next char
            }

            node = new LexerNode(Word, i, end);
            return true;
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSymbol(in ReadOnlySpan<char> src, in int i, ref LexerNode node)
    {
        /*ASCII
         |  Hexadecimal  | Symbol |  |  Hexadecimal  | Symbol |  |  Hexadecimal  | Symbol |
         |     0x21      |   !    |  |     0x3A      |   :    |  |     0x5B      |   [    | 
         |     0x22      |   "    |  |     0x3C      |   ;    |  |     0x5C      |   \    |
         |     0x23      |   #    |  |     0x3D      |   <    |  |     0x5D      |   ]    |
         |     0x24      |   $    |  |     0x3C      |   =    |  |     0x5E      |   ^    |
         |     0x25      |   %    |  |     0x3D      |   >    |  |     0x5F      |   _    |
         |     0x26      |   &    |  |     0x3E      |   ?    |  |     0x60      |   `    | 
         |     0x27      |   '    |  |     0x3F      |   @    |  +---------------+--------+
         |     0x28      |   (    |  +---------------+--------+  
         |     0x29      |   )    |  
         |     0x2A      |   *    |  |  Hexadecimal  | Symbol |
         |     0x2B      |   +    |  |     0x7B      |   {    |
         |     0x2C      |   ,    |  |     0x7C      |   |    |
         |     0x2D      |   -    |  |     0x7D      |   }    |
         |     0x2E      |   .    |  |     0x7E      |   ~    |
         |     0x2F      |   /    |  +---------------+--------+
         +---------------+--------+
         */

        if (src[i] is (>= '!' and <= '/') or (>= ':' and <= '@')
                   or (>= '{' and <= '~') or (>= '[' and <= '`'))
        {

            int end = i + GOTO_NEXT_CHAR;

            while (end < src.Length
                   && src[i] is (>= '!' and <= '/') or (>= ':' and <= '@') 
                             or (>= '{' and <= '~') or (>= '[' and <= '`'))
            {
                end++; // next char
            }

            node = new LexerNode(Symbol, i, end);
            return true;
        }

        return false;
    }
}
