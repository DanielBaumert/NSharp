using System.Runtime.CompilerServices;

namespace NSharp.Parser.Utils;

using NSharp.Lex;
using static NSharp.Lex.LexerTokenType;
using static NSharp.Lex.LexerNodeType;
using static ParserUtils;
using NSharp.Parser.Models;

internal unsafe class UsingParser
{
    /*
     * Using        := [StaticUsing|AliasUsing|ImportUsing]
     * StaticUsing  := 'using'[Space]'static'[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
     * AliasUsing   := 'using'[Space][Identifier][Space]'='[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
     * ImportUsing  := 'using'[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
     */

    private static void InternalParse(in ReadOnlySpan<char> src, Queue<LexerToken> tokens)
    {

    }
}

internal unsafe class IdentifierParser 
{
    private static SyntaxNodeBase InternalParse(in ReadOnlySpan<char> src, Queue<LexerToken> tokens)
    {
        Identifier identifier = new Identifier();
        // [Space]
        while (tokens.TryDequeue(out Token) && IsSpace(in Token, out SyntaxTriviaNode triviaNode))
        {
            identifier.Lead.Add(triviaNode);
        }

        // [_A-Za- z][_A-Za - z0 - 9]*
        if (Token.Kind is not Word)
        {
            throw new Exception();
        }

        int i = Token.Start;
        // starts with [_A-Za-z]
        if (src[i] is not ('_' or (>= 'A' and <= 'Z') or (>= 'a' and <= 'z')))
        {
            throw new Exception();
        }

        i++;
        while (i < Token.End)
        {
            // followed by [_A-Za-z0-9]*
            if (src[i] is not ('_' or (>= 'A' and <= 'Z') or (>= 'a' and <= 'z') or (>= '0' and <= '9')))
            {
                throw new Exception();
            }

            i++;
        }

        identifier.Value = new string(src[Token.Start..Token.End]);

        // [Space]
        while (tokens.TryDequeue(out Token) && IsSpace(in Token, out SyntaxTriviaNode triviaNode))
        {
            identifier.Trail.Add(triviaNode);
        }

        return identifier;
    }
}
