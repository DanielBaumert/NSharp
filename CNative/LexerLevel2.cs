namespace CNative;

using System.Collections.Concurrent;
using System.Diagnostics;
using static LexerTokenType;

public enum LexerTokenType
{
    // keyword
    /// <summary> asm </summary>
    ASM,
    /// <summary> byte </summary>
    Byte,
    /// <summary> bool </summary>
    Bool,
    /// <summary> class </summary>
    Class,
    /// <summary> char </summary>
    Char,
    /// <summary> const </summary>
    Const,
    /// <summary> do </summary>
    Do,
    /// <summary> double </summary>
    Double,
    /// <summary> enum </summary>
    Enum,
    /// <summary> false </summary>
    False,
    /// <summary> for </summary>
    For,
    /// <summary> foreach </summary>
    Foreach,
    /// <summary> float </summary>
    Float,
    /// <summary> get </summary>
    Get,
    /// <summary> interface </summary>
    Interface,
    /// <summary> int </summary>
    Int,
    /// <summary> if </summary>
    If,
    /// <summary> long </summary>
    Long,
    /// <summary> null </summary>
    Null,
    /// <summary> public </summary>
    Public,
    /// <summary> private </summary>
    Private,
    /// <summary> return </summary>
    Return,
    /// <summary> set </summary>
    Set,
    /// <summary> struct </summary>
    Struct,
    /// <summary> sbyte </summary>
    SByte,
    /// <summary> stackalloc </summary>
    Stackalloc,
    /// <summary> static </summary>
    Static,
    /// <summary> true </summary>
    True,
    /// <summary> this </summary>
    This,
    /// <summary> ushort </summary>
    UShort,
    /// <summary> uint </summary>
    UInt,
    /// <summary> ulong </summary>
    Ulong,
    /// <summary> using </summary>
    Using,
    /// <summary> var </summary>
    Var,
    /// <summary> void </summary>
    Void,
    /// <summary> while </summary>
    While,
    /// <summary> #define </summary>
    PragmaDefine,
    /// <summary> #if </summary>
    PragmaIf,
    /// <summary> #endif </summary>
    PragmaEndIf,
    /// <summary> [_A-Za-z][_A-Za-z0-9]* </summary>
    Identifier,
    /// <summary> \s </summary>
    Space,
    /// <summary> \t </summary>
    Tab,
    /// <summary> \n or \r\n </summary>
    Newline,
    // IsCompareOperator
    /// <summary> &lt;= </summary>
    LessThen,
    /// <summary> >= </summary>
    GreaterThen,
    /// <summary> == </summary>
    EqualsThen,
    /// <summary> != </summary>
    NotEqualsThen,
    // Assign
    /// <summary> = </summary>
    Assign,
    // BinaryAssign
    /// <summary> &= </summary>
    AndEquals,
    /// <summary> |= </summary>
    OrEquals,
    /// <summary> ^= </summary>
    XorEquals,
    /// <summary> ~= </summary>
    InvertEquals,
    // MathmaticalAssign
    /// <summary> += </summary>
    PlusEquals,
    /// <summary> -= </summary>
    MinusEquals,
    /// <summary> *= </summary>
    MultiplicationEquals,
    /// <summary> **= </summary>
    PowerEquals,
    /// <summary> /= </summary>
    DivisionEquals,
    /// <summary> &= </summary>
    ModuloEquals,
    // BinaryOperator
    /// <summary> ~ </summary>
    Invert,
    /// <summary> ^ </summary>
    Xor,
    /// <summary> | </summary>
    Or,
    /// <summary> &#38; </summary>
    And,
    // MathmaticalOperator
    /// <summary> + </summary>
    Plus,
    /// <summary> - </summary>
    Minus,
    /// <summary> * </summary>
    Multiplication,
    /// <summary> ** </summary>
    Power,
    /// <summary> / </summary>
    Division,
    /// <summary> % </summary>
    Modulo,
    // Symbols
    /// <summary> . </summary>
    Dot,
    /// <summary> , </summary>
    Comma,
    /// <summary> ' </summary>
    SingleQuotationMark,
    /// <summary> " </summary>
    DoubleQuotationMark,
    /// <summary> : </summary>
    Colon,
    /// <summary> ; </summary>
    Semicolon,
    /// <summary> ! </summary>
    ExclamationMark,
    /// <summary> ( </summary>
    OpenParenthesis,
    /// <summary> ) </summary>
    CloseParenthesis,
    /// <summary> [ </summary>
    OpenBrackets,
    /// <summary> ] </summary>
    CloseBrackets,
    /// <summary> { </summary>
    OpenBraces,
    /// <summary> } </summary>
    CloseBraces,
    /// <summary> [0-9]+ </summary>
    Numbers,
    /// <summary> //[^$]* </summary>
    SingleLineComment,
    /// <summary> /* </summary>
    InlineCommentStart,
    /// <summary> */ </summary>
    InlineCommentEnd,

    Unknown = -1
}

[DebuggerDisplay("Type = {Type}")]
internal struct LexerToken
{
    public readonly LexerTokenType Type;
    public readonly int Start;
    public readonly int End;

    public LexerToken(LexerTokenType type, int start, int end)
    {
        Type = type;
        Start = start;
        End = end;
    }
}


internal class LexerLevel2
{
    private string _src;

    private ConcurrentQueue<LexerNode> _level1Queue;
    private ConcurrentQueue<LexerToken> _level2Queue;

    public LexerLevel2(
        string src,
        ref ConcurrentQueue<LexerNode> level1LexerQueue,
        ref ConcurrentQueue<LexerToken> level2LexerQueue)
    {
        _src = src;

        _level1Queue = level1LexerQueue;
        _level2Queue = level2LexerQueue;
    }


    public async Task AnalyseAsync(CancellationToken ct = default)
    {
        await Task.Run(() =>
        {
            int end;
            LexerTokenType type;

            ReadOnlySpan<char> src = _src.ToCharArray();
            
            while (!ct.IsCancellationRequested || _level1Queue.Count > 0)
            {
                // TODO: out params and if's
                LexerNode node = GetNextLvl1Node();
                switch (node.Type)
                {
                    case LexerNodeType.Space:
                        if (IsSpace(src, node.Start, out end, out type))
                        {
                            AddLevel2Token(node.Start, node.End, type);
                        }
                        else
                        {
                            AddLevel2Token(node.Start, node.End, Unknown);
                        }
                        break;
                    case LexerNodeType.Word:
                        if (IsKeyword(src, node.Start, out end, out type) && node.End == end)
                        {
                            AddLevel2Token(node.Start, node.End, type);
                        }
                        else
                        {
                            AddLevel2Token(node.Start, node.End, Unknown);
                        }
                        break;
                    case LexerNodeType.Number:
                        AddLevel2Token(node.Start, node.End, Numbers);
                        break;
                    case LexerNodeType.Symbol:
                        if (IsSymbol(src, node.Start, out end, out type))
                        {
                            if(end > node.End)
                            {
                                // skip next symbol
                                int delta = end - node.Start;
                                while (delta > 0)
                                {
                                    GetNextLvl1Node();
                                    delta--;
                                }
                                // end skip
                            }

                            AddLevel2Token(node.Start, end, type);
                        }
                        else
                        {
                            AddLevel2Token(node.Start, node.End, Unknown);
                        }
                        break;
                    default:
                        throw new NotSupportedException(node.Type.ToString());
                }
            }
        }, ct);
    }

    private LexerNode GetNextLvl1Node()
    {
        // TODO: 
        LexerNode node;
        while(!_level1Queue.TryDequeue(out node))
        { }
        return node;
    }

    private void AddLevel2Token(int start, int end, LexerTokenType type)
    {
        _level2Queue.Enqueue(new LexerToken(type, start, end));
    }

    public static bool IsKeyword(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        // IDEE: a = 0, b = 1 => array of func
        switch (src[i])
        {
            case 'a': // asm
                return IsAsm(src, i, out end, out type);
            case 'b': // byte, bool
                return IsByteOrBool(src, i, out end, out type);
            case 'c': // class, char
                return IsClassOrCharOrConst(src, i, out end, out type);
            case 'd': //  do...while, double
                return IsDoOrDouble(src, i, out end, out type);
            case 'e': // enum
                return IsEnum(src, i, out end, out type);
            case 'f': // false, for, foreach, float
                return IsFalseOrForOrForeachOrFloat(src, i, out end, out type);
            case 'g': // get
                return IsGet(src, i, out end, out type);
            case 'i': // interface, int, if
                return IsInterfaceOrIntOrIf(src, i, out end, out type);
            case 'l': // long
                return IsLong(src, i, out end, out type);
            case 'n': // null
                return IsNull(src, i, out end, out type);
            case 'p': // public, private 
                return IsPublicOrPrivate(src, i, out end, out type);
            case 'r': // return
                return IsReturn(src, i, out end, out type);
            case 's': // struct, sbyte, stackalloc, static
                return IsSetOrStructOrStackallocOrStaticOrSByte(src, i, out end, out type);
            case 't': // true, this
                return IsTrueOrThis(src, i, out end, out type);
            case 'u': // ushort, uint, ulong, using [union]
                return IsUsingOrUShortOrUIntOrULong(src, i, out end, out type);
            case 'v': // var, void
                return IsVarOrVoid(src, i, out end, out type);
            case 'w': // while
                return IsWhile(src, i, out end, out type);
            case '#': // #define, #if, #IfEnd
                return IsPragma(src, i, out end, out type);
            default:
                end = 0;
                type = Unknown;
                return false;
        }
    }

    private static bool IsAsm(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if(src[i] is 's' && src[i + 1] is 'm')
        {
            end = i + 2;
            type = ASM;
            return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsByteOrBool(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'y': // byte
                if (src[i + 1] is 't' && src[i + 2] is 'e')
                {
                    end = i + 3;
                    type = Byte;
                    return true;
                }
                break;
            case 'o':  // bool
                if (src[i + 1] is 'o' && src[i + 2] is 'l')
                {
                    end = i + 3;
                    type = Bool;
                    return true;
                }
                break;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsClassOrCharOrConst(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'l': // Class
                if (src[i + 1] is 'a' && src[i + 2] is 's' && src[i + 3] is 's')
                {
                    end = i + 4;
                    type = Class;
                    return true;
                }
                break;
            case 'h': // Char
                if (src[i + 1] is 'a' && src[i + 2] is 'r')
                {
                    end = i + 3;
                    type = Char;
                    return true;
                }
                break;
            case 'o': // Const
                if (src[i + 1] is 'n' && src[i + 2] is 's' && src[i + 3] is 't')
                {
                    end = i + 4;
                    type = Const;
                    return true;
                }
                break;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsDoOrDouble(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if (src[i] is 'o') // do
        {
            if(src[i + 1] is 'u' && src[i + 2] is 'b' && src[i + 3] is 'l' && src[i + 4] is 'e')
            {
                end = i + 5;
                type = Double;
                return true;
            }
            end = i + 1;
            type = Do;
            return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsEnum(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if (src[i] is 'n' && src[i + 1] is 'u' && src[i + 2] is 'm') // enum
        {
            end = i + 3;
            type = Enum;
            return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsFalseOrForOrForeachOrFloat(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'a': // false
                if (src[i + 1] is 'l' && src[i + 2] is 's' && src[i + 3] is 'e')
                {
                    end = i + 4;
                    type = False;
                    return true;
                }
                break;
            case 'o': // for or foreach
                if (src[i + 1] is 'r')
                {
                    if (src[i + 2] is 'e' && src[i + 3] is 'a' && src[i + 4] is 'c' && src[i + 5] is 'h') // foreach
                    {
                        end = i + 6;
                        type = Foreach;
                        return true;
                    }

                    // for
                    end = i + 2;
                    type = For;
                    return true;
                }
                break;
            case 'l': // float
                if(src[i + 1] is 'o' && src[i + 2] is 'a' && src[i + 3] is 't')
                {
                    end = i + 4;
                    type = Float;
                    return true;
                }
                break;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsGet(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if (src[i] is 'e' && src[i + 1] is 't') // get
        {
            end = i + 2;
            type = Get;
            return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsInterfaceOrIntOrIf(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'n': // interface
                if (src[i + 1] is 't')
                {
                    if (src[i + 2] is 'e' && src[i + 3] is 'r' && src[i + 4] is 'f' && src[i + 5] is 'a' && src[i + 6] is 'c' && src[i + 7] is 'e')
                    {
                        end = i + 8;
                        type = Interface;
                        return true;
                    }

                    // int
                    end = i + 2;
                    type = Int;
                    return true;
                }
                break;
            case 'f': // if
                if (src[i] is 'f')
                {
                    end = i + 1;
                    type = If;
                    return true;
                }
                break;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsLong(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if(src[i] is 'o' && src[i + 1] is 'n' && src[i + 2] is 'g')
        {
            end = i + 3;
            type = Long;
            return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsNull(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if (src[i] is 'u' && src[i + 1] is 'l' && src[i + 2] is 'l') // null
        {
            end = i + 3;
            type = Null;
            return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsPublicOrPrivate(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'u': // public
                if (src[i + 1] is 'b' && src[i + 2] is 'l' && src[i + 3] is 'i' && src[i + 4] is 'c')
                {
                    end = i + 5;
                    type = Public;
                    return true;
                }
                break;
            case 'r': // private
                if (src[i + 1] is 'i' && src[i + 2] is 'v' && src[i + 3] is 'a' && src[i + 4] is 't' && src[i + 5] is 'e')
                {
                    end = i + 6;
                    type = Private;
                    return true;
                }
                break;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsReturn(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if (src[i] is 'e' && src[i + 1] is 't' && src[i + 2] is 'u' && src[i + 3] is 'r' && src[i + 4] is 'n') // return
        {
            end = i + 5;
            type = Return;
            return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsSetOrStructOrStackallocOrStaticOrSByte(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'e': // set
                if (src[i + 1] is 't')
                {
                    end = i + 2;
                    type = Set;
                    return true;
                }
                break;
            case 't': // struct, stackalloc, static
                switch (src[i + 1])
                {
                    case 'a': //  stackalloc, static
                        switch (src[i + 2])
                        {
                            case 'c': // stackalloc
                                if (src[i + 3] is 'k' && src[i + 4] is 'a' && src[i + 5] is 'l' && src[i + 6] is 'l' && src[i + 7] is 'o' && src[i + 8] is 'c')
                                {
                                    end = i + 9;
                                    type = Stackalloc;
                                    return true;
                                }
                                break;
                            case 't': // static
                                if (src[i + 3] is 'i' && src[i + 4] is 'c')
                                {
                                    end = i + 5;
                                    type = Static;
                                    return true;
                                }
                                break;
                        }
                        break;
                    case 'r': // struct
                        if (src[i + 1] is 'u' && src[i + 2] is 'c' && src[i + 3] is 't') // Struct
                        {
                            end = i + 4;
                            type = Struct;
                            return true;
                        }
                        break;
                }
                break;
            case 'b': // Sbyte
                if (src[i + 1] is 'y' && src[i + 2] is 't' && src[i + 3] is 'e') // Sbyte
                {
                    end = i + 4;
                    type = SByte;
                    return true;
                }
                break;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsTrueOrThis(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'r': // true
                if (src[i + 1] is 'u' && src[i + 2] is 'e') 
                {
                    end = i + 3;
                    type = True;
                    return true;
                }
                break;
            case 'h': // this
                if(src[i + 1] is 'i' && src[i + 2] is 's')
                {
                    end = i + 3;
                    type = This;
                    return true;
                }
                break;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsUsingOrUShortOrUIntOrULong(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 's': // using, ushort
                switch (src[i + 1])
                {
                    case 'i': // using
                        if (src[i + 2] is 'n' && src[i + 3] is 'g') // using
                        {
                            end = i + 4;
                            type = Using;
                            return true;
                        }
                        break;
                    case 'h': // ushort
                        if (src[i + 2] is 'o' && src[i + 3] is 'r' && src[i + 4] is 't') // ushort
                        {
                            end = i + 5;
                            type = UShort;
                            return true;
                        }
                        break;
                }
                break;
            case 'i': // uint
                if (src[i + 1] is 'n' && src[i + 2] is 't') // uint
                {
                    end = i + 3;
                    type = UInt;
                    return true;
                }
                break;
            case 'l': // ulong
                if (src[i + 1] is 'o' && src[i + 2] is 'n' && src[i + 3] is 'g') // ulong
                {
                    end = i + 4;
                    type = Ulong;
                    return true;
                }
                break;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsVarOrVoid(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'a':// var
                if (src[i + 1] is 'r')
                {
                    end = i + 2;
                    type = Var;
                    return true;
                }
                break;
            case 'o': // void
                if (src[i + 1] is 'i' && src[i + 2] is 't')
                {
                    end = i + 3;
                    type = Void;
                    return true;
                }
                break;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsWhile(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if (src[i] is 'h' && src[i + 1] is 'i' && src[i + 2] is 'l' && src[i + 3] is 'e') // while
        {
            end = i + 4;
            type = While;
            return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }

    private static bool IsPragma(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        // #define, #if, #endif
        i++;
        switch (src[i])
        {
            case 'd': // #define
                if(src[i + 1] is 'e' && src[i + 2] is 'f' && src[i + 3] is 'i' && src[i + 4] is 'n' && src[i + 5] is 'e')
                {
                    end = i + 6;
                    type = PragmaDefine;
                    return true;
                }
                break;
            case 'e': // #endif
                if (src[i + 1] is 'n' && src[i + 2] is 'd' && src[i + 3] is 'i' && src[i + 4] is 'f')
                {
                    end = i + 6;
                    type = PragmaEndIf;
                    return true;
                }
                break;
            case 'i': // #if
                if(src[i + 1] is 'f')
                {
                    end = i + 6;
                    type = PragmaDefine;
                    return true;
                }
                break;
        }

        end = 0;
        type = Unknown;
        return false;
    }

    private static bool IsSpace(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        switch (src[i])
        {
            case ' ': // space
                end = i + 1;
                type = Space;
                return true;
            case '\t': // tab
                end = i + 1;
                type = Tab;
                return true;
            case '\r': // new line
                if (src[i + 1] is '\n')
                {
                    end = i + 2;
                    type = Newline;
                    return true;
                }
                break;
            case '\n': // new line
                end = i + 1;
                type = Newline;
                return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }



    private static bool IsSymbol(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        switch (src[i])
        {
            case '.':
                end = i + 1;
                type = Dot;
                return true;
            case ',':
                end = i + 1;
                type = Comma;
                return true;
            case '\'':
                end = i + 1;
                type = SingleQuotationMark;
                return true;
            case '"':
                end = i + 1;
                type = DoubleQuotationMark;
                return true;
            case ':':
                end = i + 1;
                type = Colon;
                return true;
            case ';':
                end = i + 1;
                type = Semicolon;
                return true;
            case '!':
                end = i + 1;
                type = ExclamationMark;
                return true;
            case '(':
                end = i + 1;
                type = OpenParenthesis;
                return true;
            case ')':
                end = i + 1;
                type = CloseParenthesis;
                return true;
            case '[':
                end = i + 1;
                type = OpenBrackets;
                return true;
            case ']':
                end = i + 1;
                type = CloseBrackets;
                return true;
            case '{':
                end = i + 1;
                type = OpenBraces;
                return true;
            case '}':
                end = i + 1;
                type = CloseBraces;
                return true;
        }

        // if all false then end became 0 and type unknown
        return IsAssignOperator(src, i, out end, out type)
            || IsOperator(src, i, out end, out type);
    }

    private static bool IsAssignOperator(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        switch (src[i + 1])
        {
            case '=':
                switch (src[i])
                {
                    /// binary
                    case '&':
                        end = i + 2;
                        type = AndEquals;
                        return true;
                    case '|':
                        end = i + 2;
                        type = OrEquals;
                        return true;
                    case '^':
                        end = i + 2;
                        type = XorEquals;
                        return true;
                    case '~':
                        end = i + 2;
                        type = InvertEquals;
                        return true;
                    /// mathmatical
                    case '+':
                        end = i + 2;
                        type = PlusEquals;
                        return true;
                    case '-':
                        end = i + 2;
                        type = MinusEquals;
                        return true;
                    case '*':
                        end = i + 2;
                        type = MultiplicationEquals;
                        return true;
                    case '/':
                        end = i + 2;
                        type = DivisionEquals;
                        return true;
                    case '%':
                        end = i + 2;
                        type = ModuloEquals;
                        return true;
                    /// compare
                    case '<':
                        end = i + 2;
                        type = LessThen;
                        return true;
                    case '>':
                        end = i + 2;
                        type = GreaterThen;
                        return true;
                    case '=':
                        end = i + 2;
                        type = EqualsThen;
                        return true;
                    case '!':
                        end = i + 2;
                        type = NotEqualsThen;
                        return true;
                }
                break;
            case '*':
                if (src[i] is '*' && src[i + 2] is '=')
                {
                    end = i + 3;
                    type = PowerEquals;
                    return true;
                }
                break;
        }

        if (src[i] is '=')
        {
            end = i + 1;
            type = Assign;
            return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }

    private static bool IsOperator(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        return IsBinaryOperator(src, i, out end, out type)
            || IsMathmaticalOperatorOrComment(src, i, out end, out type);

    }
    private static bool IsBinaryOperator(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        switch (src[i])
        {
            case '~':
                end = i + 1;
                type = Invert;
                return true;
            case '^':
                end = i + 1;
                type = Xor;
                return true;
            case '|':
                end = i + 1;
                type = Or;
                return true;
            case '&':
                end = i + 1;
                type = And;
                return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }
    private static bool IsMathmaticalOperatorOrComment(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        switch (src[i])
        {
            case '+':
                end = i + 1;
                type = Plus;
                return true;
            case '-':
                end = i + 1;
                type = Minus;
                return true;
            case '*':
                switch (src[i + 1])
                {
                    case '*': // power 
                        end = i + 2;
                        type = Power;
                        return true;
                    case '/': // comment
                        end = i + 2;
                        type = InlineCommentEnd;
                        return true;
                }
                end = i + 1;
                type = Multiplication;
                return true;
            case '/':
                switch (src[i + 1])
                {
                    case '*': // inline comment
                        end = i + 2;
                        type = InlineCommentStart;
                        return true;
                    case '/': // comment
                        end = i + 2;
                        type = SingleLineComment;
                        return true;
                }
                end = i + 1;
                type = Division;
                return true;
            case '%':
                end = i + 1;
                type = Modulo;
                return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }

    // todo remove into the parser
    private static bool IsIdentifier(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        // starts with [A-Za-z]
        if (src[i] is '_' or (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'))
        {
            i++;
            // followed by [A-Za-z0-9_]*
            while (i < src.Length && src[i] is '_' or (>= 'A' and <= 'Z') or (>= 'a' and <= 'z') or (>= '0' and <= '9'))
            {
                i++;
            }
            end = i;
            type = Unknown;
            return true;
        }

        end = 0;
        type = Unknown;
        return false;
    }
}
