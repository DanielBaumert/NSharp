using NSharp.Lex;
using System.Runtime.CompilerServices;

namespace NSharp.Parser;

using static Utils.ParserUtils;
using static Lex.LexerTokenType;
using static Lex.LexerNodeType;

public abstract class SyntaxNode {
    internal List<SyntaxNode> _children = new List<SyntaxNode>();
}

public class CompilationUnit : SyntaxNode
{
    private List<UsingDirective> _usings = new List<UsingDirective>();
    private FileScopedNamespaceDeclation? _namespace = null;

    public void AddUsing(string usingPath) 
    {
        UsingDirective usingDirective = new UsingDirective(usingPath);
        _children.Add(usingDirective);
        _usings.Add(usingDirective);
    }

    public void AddNamespace(string namespaceName)
    {
        if(_namespace != null)
        {
            throw new Exception();
        }

        _namespace = new FileScopedNamespaceDeclation(namespaceName);
    }
}



public class QualifiedName : SyntaxNode
{

} 

public class UsingDirective : SyntaxNode
{
    public string Path { get; init; }

    internal UsingDirective(string path)
    {
        Path = path;
    }
}

public class FileScopedNamespaceDeclation : CompilationUnit
{
    public string Name { get; init; }
    internal FileScopedNamespaceDeclation(string namespaceName)
    {
        Name = namespaceName;
    }
}

internal class Parser
{

    public Parser()
    { }

    public void Parse(string src)
    {
        ReadOnlySpan<char> srcro = src.ToCharArray();

        Queue<LexerToken> tokens = Lexer.Analyse(in srcro);

        CompilationUnit compilationUnit = new CompilationUnit();

        while(tokens.TryDequeue(out LexerToken token))
        {
            switch (token.Type)
	        {
                case LexerTokenType.Using:

                case LexerTokenType.Namespace: 

                case LexerTokenType.Whitespace or LexerTokenType.Newline:
                    continue;
                default:
                    throw new Exception();
	        }   


        }
        
    }

    /// <summary>
    /// <code>
    /// Using        := [StaticUsing|AliasUsing|ImportUsing]
    /// StaticUsing  := 'using'[Space]'static'[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
    /// AliasUsing   := 'using'[Space][Identifier][Space]'='[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
    /// ImportUsing  := 'using'[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
    /// </code></summary>
    private static void Using(in ReadOnlySpan<char> src, Queue<LexerToken> tokens)
    {
        Unsafe.SkipInit(out LexerToken token);

        while (tokens.TryDequeue(out token) && IsSpace(in token)); // [Space]

        switch (token.Type)
        {
            // StaticUsing := using[Space]static?[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
            case Static:
                // START: [Space][Identifier][Space]
                while (tokens.TryDequeue(out token) && IsSpace(in token)); // [Space]
                if (token.Kind is not Word || !IsIdentifier(in src, in token)) // not [Identifier]
                {
                    throw new Exception();
                }
                while (tokens.TryDequeue(out token) && IsSpace(in token)) ; // [Space]
                // END: [Space][Identifier][Space]

                //START: ('.'[Space][identifier][Space])*
                while (token.Type is Dot) // '.'
                {
                    while (tokens.TryDequeue(out token) && IsSpace(in token)) ; // [Space]

                    if (token.Kind is not Word || !IsIdentifier(in src, in token)) // not [Identifier]
                    {
                        throw new Exception();
                    }

                    while (tokens.TryDequeue(out token) && IsSpace(in token)) ; // [Space]
                }
                //END: ('.'[Space][identifier][Space])*

                if (token.Type is not Semicolon)
                {
                    throw new Exception();
                }

                break;
            // AliasUsing   := using[Space][Identifier][Space]'='[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
            // ImportUsing:= using[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
            case Unknown:
                
                break;
        }

        throw new Exception();

        if (token.Kind is not Word || !IsIdentifier(in src, in token)) // not [Identifier]
        {
            throw new Exception();
        }

        while (tokens.TryDequeue(out token) && IsSpace(in token)) ; // [Space]

        //START: ('.'[Space][identifier][Space])*
        while (token.Type is Dot) // '.'
        {
            while (tokens.TryDequeue(out token) && IsSpace(in token)) ; // [Space]

            if(token.Kind is not Word || !IsIdentifier(in src, in token)) // not [Identifier]
            {
                throw new Exception();
            }

            while (tokens.TryDequeue(out token) && IsSpace(in token)) ; // [Space]
        }
        //END: ('.'[Space][identifier][Space])*
            
        if(token.Type is not Semicolon)
        {
            throw new Exception();
        }
    }


    /// <summary>
    /// <code>
    /// Identifier := [Space][_A-Za-z][_A-Za-z0-9]*[Space]
    /// </code></summary>
    private static bool Identifier(in ReadOnlySpan<char> src, Queue<LexerToken> tokens)
    {
        Unsafe.SkipInit(out LexerToken token);
        // [Space]
        while (tokens.TryDequeue(out token) && IsSpace(in token)) ;

        if (token.Type is InlineCommentStart)
        {
            while (tokens.TryDequeue(out token) && token.Type is not InlineCommentEnd) ;

        }

        // [_A-Za- z][_A-Za - z0 - 9]*
        if (token.Kind is not Word)
        {
            throw new Exception();
        }

        int i = token.Start;
        // starts with [_A-Za-z]
        if (src[i] is not ('_' or (>= 'A' and <= 'Z') or (>= 'a' and <= 'z')))
        {
            throw new Exception();
        }

        i++;
        while (i < token.End)
        {
            // followed by [_A-Za-z0-9]*
            if (src[i] is not ('_' or (>= 'A' and <= 'Z') or (>= 'a' and <= 'z') or (>= '0' and <= '9')))
            {
                return false;
            }

            i++;
        }

        while (tokens.TryDequeue(out token) && IsSpace(in token)) ; // [Space]

        return true;
    }

    
}
