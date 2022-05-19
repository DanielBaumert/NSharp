using System.Runtime.CompilerServices;

namespace NSharp.Lex.Level;
using static LexerTokenType;

internal static class LexerLevel2
{
    public static void Analyse(in ReadOnlySpan<char> src, Queue<LexerNode> level1Queue, Queue<LexerToken> level2Queue)
    {
        int end;
        LexerTokenType type;

        while (level1Queue.Count > 0)
        {
            // TODO: out params and if's
            LexerNode node = GetNextLvl1Node(level1Queue);
            switch (node.Type)
            {
                case LexerNodeType.Empty:
                    if (IsSpace(in src, node.Start, out end, out type))
                    {
                        if (end != node.End) // \r\n
                        {
                            // get \n of \r\n
                            node = GetNextLvl1Node(level1Queue);
                        }

                        AddLevel2Token(level2Queue, node.Start, end, type);
                    }
                    else
                    {
                        AddLevel2Token(level2Queue, node.Start, node.End, Unknown);
                    }
                    break;
                case LexerNodeType.Word:
                    if (IsKeyword(in src, node.Start, out end, out type) && node.End == end)
                    {
                        AddLevel2Token(level2Queue, node.Start, node.End, type);
                    }
                    else
                    {
                        AddLevel2Token(level2Queue, node.Start, node.End, Unknown);
                    }
                    break;
                case LexerNodeType.Number:
                    AddLevel2Token(level2Queue, node.Start, node.End, Digits);
                    break;
                case LexerNodeType.Symbol:
                    if (IsSymbol(in src, node.Start, out end, out type))
                    {
                        if (end > node.End)
                        {
                            // skip next symbol
                            int delta = end - node.Start;
                            while (delta > 0)
                            {
                                GetNextLvl1Node(level1Queue);
                                delta--;
                            }
                            // end skip
                        }

                        AddLevel2Token(level2Queue, node.Start, end, type);
                    }
                    else
                    {
                        AddLevel2Token(level2Queue, node.Start, node.End, Unknown);
                    }
                    break;
                default:
                    throw new NotSupportedException(node.Type.ToString());
            }
        }
    }

    private static LexerNode GetNextLvl1Node(Queue<LexerNode> level1Queue)
    {
        return level1Queue.Dequeue();
    }

    private static void AddLevel2Token(Queue<LexerToken> level2Queue, int start, int end, LexerTokenType type)
    {
        level2Queue.Enqueue(new LexerToken(type, start, end));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKeyword(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        // IDEE: a = 0, b = 1 => array of func
        switch (src[i])
        {
            case 'a': // asm
                return IsAsm(in src, i, out end, out type);
            case 'b': // byte, bool
                return IsByteOrBool(in src, i, out end, out type);
            case 'c': // class, char
                return IsClassOrCharOrConst(in src, i, out end, out type);
            case 'd': //  do...while, double
                return IsDoOrDouble(in src, i, out end, out type);
            case 'e': // else, enum
                return IsElseOrEnum(in src, i, out end, out type);
            case 'f': // false, for, foreach, float
                return IsFalseOrForOrForeachOrFloat(in src, i, out end, out type);
            case 'g': // get
                return IsGet(in src, i, out end, out type);
            case 'i': // interface, internal, int, if, in
                return IsInterfaceOrIntOrIfOrIn(in src, i, out end, out type);
            case 'l': // long
                return IsLong(in src, i, out end, out type);
            case 'n': // namespace null, 
                return IsNamespaceOrNull(in src, i, out end, out type);
            case 'p': // public, private 
                return IsPublicOrPrivate(in src, i, out end, out type);
            case 'r': // return
                return IsReturn(in src, i, out end, out type);
            case 's': // struct, sbyte, short, stackalloc, static
                return IsSetOrStructOrShortOrStackallocOrStaticOrSByte(in src, i, out end, out type);
            case 't': // true, this
                return IsTrueOrThis(in src, i, out end, out type);
            case 'u': // ushort, uint, ulong, using [union]
                return IsUsingOrUShortOrUIntOrULong(in src, i, out end, out type);
            case 'v': // var, void
                return IsVarOrVoid(in src, i, out end, out type);
            case 'w': // while
                return IsWhile(in src, i, out end, out type);
            case '#': // #define, #if, #IfEnd
                return IsPragma(in src, i, out end, out type);
            default:
                end = i;
                type = Unknown;
                return false;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsAsm(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if (src[i] is 's' && src[i + 1] is 'm')
        {
            end = i + 2;
            type = ASM;
            return true;
        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsByteOrBool(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
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

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsClassOrCharOrConst(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'h': // Char
                if (src[i + 1] is 'a' && src[i + 2] is 'r')
                {
                    end = i + 3;
                    type = Char;
                    return true;
                }
                break;
            case 'l': // Class
                if (src[i + 1] is 'a' && src[i + 2] is 's' && src[i + 3] is 's')
                {
                    end = i + 4;
                    type = Class;
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

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsDoOrDouble(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if (src[i] is 'o') // do
        {
            if (src[i + 1] is 'u' && src[i + 2] is 'b' && src[i + 3] is 'l' && src[i + 4] is 'e')
            {
                end = i + 5;
                type = Double;
                return true;
            }
            end = i + 1;
            type = Do;
            return true;
        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsElseOrEnum(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'l': // else
                if (src[i + 1] is 's' && src[i + 2] is 'e') 
                {
                    end = i + 3;
                    type = Else;
                    return true;
                }
                break;
            case 'n': // enum
                if (src[i + 1] is 'u' && src[i + 2] is 'm') 
                {
                    end = i + 3;
                    type = Enum;
                    return true;
                }
                break;
        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsFalseOrForOrForeachOrFloat(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
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
                    // foreach
                    if (src[i + 2] is 'e' && src[i + 3] is 'a' && src[i + 4] is 'c' && src[i + 5] is 'h') 
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
                if (src[i + 1] is 'o' && src[i + 2] is 'a' && src[i + 3] is 't')
                {
                    end = i + 4;
                    type = Float;
                    return true;
                }
                break;
        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsGet(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        // get
        if (src[i] is 'e' && src[i + 1] is 't') 
        {
            end = i + 2;
            type = Get;
            return true;
        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsInterfaceOrIntOrIfOrIn(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'n': // interface or interface
                if (src[i + 1] is 't')
                {
                    if (src[i + 2] is 'e' && src[i + 3] is 'r')
                    {
                        // Internal
                        if (src[i + 4] is 'n' && src[i + 5] is 'a' && src[i + 6] is 'l')
                        {
                            end = i + 7;
                            type = Internal;
                            return true;
                        }

                        // Interface
                        if (src[i + 4] is 'f' && src[i + 5] is 'a' && src[i + 6] is 'c' && src[i + 7] is 'e')
                        {
                            end = i + 8;
                            type = Interface;
                            return true;
                        }
                    }

                    // int
                    end = i + 2;
                    type = Int;
                    return true;
                }

                // in
                end = i + 1;
                type = In;
                return true;
            case 'f': // if
                if (src[i] is 'f')
                {
                    end = i + 1;
                    type = If;
                    return true;
                }
                break;
        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsLong(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        // long
        if (src[i] is 'o' && src[i + 1] is 'n' && src[i + 2] is 'g')
        {
            end = i + 3;
            type = Long;
            return true;
        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsNamespaceOrNull(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'a': // namespace
                if (src[i + 1] is 'm' && src[i + 2] is 'e'
                    && src[i + 3] is 's' && src[i + 4] is 'p' && src[i + 5] is 'a' && src[i + 6] is 'c' && src[i + 7] is 'e')
                {
                    end = i + 8;
                    type = Namespace;
                    return true;
                }
                break;
            case 'u': // null
                if (src[i + 1] is 'l' && src[i + 2] is 'l')
                {
                    end = i + 3;
                    type = Null;
                    return true;
                }
                break;
        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsPublicOrPrivate(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
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

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsReturn(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if (src[i] is 'e' && src[i + 1] is 't' && src[i + 2] is 'u' && src[i + 3] is 'r' && src[i + 4] is 'n') // return
        {
            end = i + 5;
            type = Return;
            return true;
        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSetOrStructOrShortOrStackallocOrStaticOrSByte(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'b': // sbyte
                if (src[i + 1] is 'y' && src[i + 2] is 't' && src[i + 3] is 'e') // sbyte
                {
                    end = i + 4;
                    type = SByte;
                    return true;
                }
                break;
            case 'e': // set
                if (src[i + 1] is 't')
                {
                    end = i + 2;
                    type = Set;
                    return true;
                }
                break;
            case 'h': // short
                if (src[i + 1] is 'o' && src[i + 2] is 'r' && src[i + 3] is 't')
                {
                    end = i + 4;
                    type = Short;
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

        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsTrueOrThis(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'h': // this
                if (src[i + 1] is 'i' && src[i + 2] is 's')
                {
                    end = i + 3;
                    type = This;
                    return true;
                }
                break;
            case 'r': // true
                if (src[i + 1] is 'u' && src[i + 2] is 'e')
                {
                    end = i + 3;
                    type = True;
                    return true;
                }
                break;
        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsUsingOrUShortOrUIntOrULong(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
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

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsVarOrVoid(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
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
                if (src[i + 1] is 'i' && src[i + 2] is 'd')
                {
                    end = i + 3;
                    type = Void;
                    return true;
                }
                break;
        }

        end = i;type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsWhile(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        // while
        if (src[i] is 'h' && src[i + 1] is 'i' && src[i + 2] is 'l' && src[i + 3] is 'e') 
        {
            end = i + 4;
            type = While;
            return true;
        }

        end = i;
        type = Unknown;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsPragma(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        // #define, #if, #endif
        i++;
        switch (src[i])
        {
            case 'd': // #define
                if (src[i + 1] is 'e' && src[i + 2] is 'f' && src[i + 3] is 'i' && src[i + 4] is 'n' && src[i + 5] is 'e')
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
                if (src[i + 1] is 'f')
                {
                    end = i + 6;
                    type = PragmaDefine;
                    return true;
                }
                break;
        }

        end = i;
        type = Unknown;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSpace(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        switch (src[i])
        {
            case ' ': // whitespace
                end = i + 1;
                type = Whitespace;
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
                throw new FormatException("Unknow char sequenz");
            case '\n': // new line
                end = i + 1;
                type = Newline;
                return true;
        }

        end = i;
        type = Unknown;
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSymbol(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
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
            case '\\':
                end = i + 1;
                type = Slash;
                return true;
            /* AssignOperator */
            case '&':
                switch (src[i + 1])
                {
                    case '=':
                        end = i + 2;
                        type = AndEquals;
                        return true;
                    case '&':
                        end = i + 2;
                        type = AndAlso;
                        return true;
                }
                end = i + 1;
                type = And;
                return true;
            case '|':
                switch (src[i + 1])
                {
                    case '=':
                        end = i + 2;
                        type = OrEquals;
                        return true;
                    case '&':
                        end = i + 2;
                        type = OrElse;
                        return true;
                }
                end = i + 1;
                type = Or;
                return true;
            case '^':
                if (src[i + 1] is '=')
                {
                    end = i + 2;
                    type = OrEquals;
                    return true;
                }
                end = i + 1;
                type = Xor;
                return true;
            case '~':
                if (src[i + 1] is '=')
                {
                    end = i + 2;
                    type = InvertEquals;
                    return true;
                }
                end = i + 1;
                type = Invert;
                return true;
            /// mathmatical
            case '+':
                switch (src[i + 1])
                {
                    case '=':
                        end = i + 2;
                        type = PlusEquals;
                        return true;
                    case '+':
                        end = i + 2;
                        type = AtomicIncreas;
                        return true;
                }

                end = i + 1;
                type = Plus;
                return true;
            case '-':
                switch (src[i + 1])
                {
                    case '=':
                        end = i + 2;
                        type = MinusEquals;
                        return true;
                    case '+':
                        end = i + 2;
                        type = AtomicDecrease;
                        return true;
                }

                end = i + 1;
                type = Minus;
                return true;
            case '*':
                // * or *= or ** or **= 
                switch (src[i + 1])
                {
                    case '/': // comment
                        end = i + 2;
                        type = InlineCommentEnd;
                        return true;
                    case '=': // *)
                        end = i + 2;
                        type = MultiplicationEquals;
                        return true;
                    case '*':
                        if (src[i + 2] is '=')
                        {
                            end = i + 3;
                            type = PowerEquals;
                            return true;
                        }

                        end = i + 2;
                        type = Power;
                        return true;
                }

                end = i + 1;
                type = Multiplication;
                return true;
            case '/':
                // /= or /* or / or //
                switch (src[i + 1])
                {
                    case '=': // /=
                        end = i + 1;
                        type = DivisionEquals;
                        return true;
                    case '*': // /*
                        end = i + 1;
                        type = InlineCommentStart;
                        return true;
                    case '/': // //
                        end = i + 1;
                        type = SingleLineComment;
                        return true;
                }
                end = i + 1; // /
                type = Division;
                return true;
            case '%':
                if (src[i + 1] is '=')
                {
                    end = i + 1;
                    type = ModuloEquals;
                    return true;
                }

                end = i + 1;
                type = Modulo;
                return true;
            /// compare
            case '<':
                // < or <= or  << or <<= 
                switch (src[i + 1])
                {
                    case '=': //  <=
                        end = i + 1;
                        type = LessThen;
                        return true;
                    case '<': //  <<
                        if (src[i + 2] is '=') // <<=
                        {

                            end = i + 3;
                            type = LeftShiftEquals;
                            return true;
                        }

                        end = i + 2;
                        type = LeftShift;
                        return true;
                }
                // <
                end = i + 1;
                type = Less;
                return true;
            case '>':
                switch (src[i + 1])
                {
                    case '=': // >=
                        end = i + 1;
                        type = GreaterThen;
                        return true;
                    case '>': // >> or >>=
                        // >>=
                        if (src[i + 2] is '=')
                        {
                            end = i + 3;
                            type = RightShiftEquals;
                            return true;
                        }
                        
                        // >> 
                        end = i + 2;
                        type = RightShift;
                        return true;
                }

                end = i + 1;
                type = Greater;
                return true;
            case '=':
                // ==
                if (src[i + 1] is '=')
                {
                    end = i + 1;
                    type = EqualsThen;
                    return true;
                }

                end = i + 1;
                type = Assign;
                return true;
            case '!':
                // !=
                if (src[i + 1] is '=')
                {
                    end = i + 2;
                    type = NotEqualsThen;
                    return true;
                }

                end = i + 1;
                type = Not;
                return true;
        }

        end = i;
        type = Unknown;
        return true;
    }
}
