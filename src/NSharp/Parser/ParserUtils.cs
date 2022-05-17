namespace NSharp.Parser.Utils;

using NSharp.Lex;
using static NSharp.Lex.LexerTokenType;
using NSharp.Parser.Models;

internal static class ParserUtils
{
    public static LexerToken Token;
    /// <summary>
    /// (\s|\t|\n|\r\n)
    /// </summary>
    public static bool IsSpace(in LexerToken token, out SyntaxTriviaNode node)
    {
        switch (token.Type)
	    {
            case Whitespace:
                node = new TriviaWhitespace();
                return true;
            case Tab:
                node = new TriviaTab();
                return true;
            case Newline:
                node = new TriviaNewline();
                return true;
        }

        node = null!;
        return false;
    }
}
