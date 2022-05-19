using NSharp.Lex;
using System.Runtime.InteropServices;

namespace NSharp.Lexer.Level;

using static LexerTokenType;

[Obsolete("To Slow")]
internal unsafe static class Lexer
{


    private static delegate*<in ReadOnlySpan<char>, int, out int, out LexerTokenType, bool>[] _shortHand
        = new delegate*<in ReadOnlySpan<char>, int, out int, out LexerTokenType, bool>[128]
    {
        /*0x00 | nul */ &NotSupportedException,
        /*0x01 | soh */ &NotSupportedException,
        /*0x02 | stx */ &NotSupportedException,
        /*0x03 | etx */ &NotSupportedException,
        /*0x04 | eot */ &NotSupportedException,
        /*0x05 | enq */ &NotSupportedException,
        /*0x06 | ack */ &NotSupportedException,
        /*0x07 | bel */ &NotSupportedException,
        /*0x08 | bs  */ &NotSupportedException,
        /*0x09 | ht  */ &NotSupportedException,
        /*0x0A | lf  */ &IsNewline,
        /*0x0B | vt  */ &NotSupportedException,
        /*0x0C | ff  */ &NotSupportedException,
        /*0x0D | cr  */ &IsCarriageReturn,
        /*0x0E | so  */ &NotSupportedException,
        /*0x0F | si  */ &NotSupportedException,
        /*0x10 | dle */ &NotSupportedException,
        /*0x11 | dc1 */ &NotSupportedException,
        /*0x12 | dc2 */ &NotSupportedException,
        /*0x13 | dc3 */ &NotSupportedException,
        /*0x14 | dc4 */ &NotSupportedException,
        /*0x15 | nak */ &NotSupportedException,
        /*0x16 | syn */ &NotSupportedException,
        /*0x17 | etb */ &NotSupportedException,
        /*0x18 | can */ &NotSupportedException,
        /*0x19 | em  */ &NotSupportedException,
        /*0x1A | sub */ &NotSupportedException,
        /*0x1B | esc */ &NotSupportedException,
        /*0x1C | fs  */ &NotSupportedException,
        /*0x1D | gr  */ &NotSupportedException,
        /*0x1E | rs  */ &NotSupportedException,
        /*0x1F | us  */ &NotSupportedException,
        /*0x20 | [_] */ &IsSpace,
        /*0x21 |  !  */ &IsExclamationMarkOrNot,
        /*0x22 |  "  */ &IsDoubleQuotationMark,
        /*0x23 |  #  */ &IsPragma,
        /*0x24 |  $  */ &NotSupportedException,
        /*0x25 |  %  */ &IsModuloOrModuloEquals,
        /*0x26 |  &  */ &IsAndOrAndEqualsOrAndAlso,
        /*0x27 |  '  */ &IsSingleQuotationMark,
        /*0x28 |  (  */ &IsOpenParenthesis,
        /*0x29 |  )  */ &IsCloseParenthesis,
        /*0x2A |  *  */ &IsMultiplicationOrMultiplicationEqualsOrPowerOrPowerEquals,
        /*0x2B |  +  */ &IsPlusOrPlusEqualsOrAtomicIncreas,
        /*0x2C |  ,  */ &IsComma,
        /*0x2D |  -  */ &IsMinusOrMinusEqualsOrAtomicDecrease,
        /*0x2E |  .  */ &IsDot,
        /*0x2F |  /  */ &IsDivisionOrDivisionEquals,
        /*0x30 |  0  */ &IsHexOrBinaryOrDigits,
        /*0x31 |  1  */ &IsDigits,
        /*0x32 |  2  */ &IsDigits,
        /*0x33 |  3  */ &IsDigits,
        /*0x34 |  4  */ &IsDigits,
        /*0x35 |  5  */ &IsDigits,
        /*0x36 |  6  */ &IsDigits,
        /*0x37 |  7  */ &IsDigits,
        /*0x38 |  8  */ &IsDigits,
        /*0x39 |  9  */ &IsDigits,
        /*0x3A |  :  */ &IsColon,
        /*0x3B |  ;  */ &IsSemicolon,
        /*0x3C |  <  */ &IsLessOrLessThenOrLeftShiftOrLeftShiftEquals,
        /*0x3D |  =  */ &IsEqualOrEqualsThen,
        /*0x3E |  >  */ &IsGreaterOrGreaterThenOrRightShiftOrRightShiftEquals,
        /*0x3F |  ?  */ &NotSupportedException,
        /*0x40 |  @  */ &NotSupportedException,
        /*0x41 |  A  */ &IsIdentifier,
        /*0x42 |  B  */ &IsIdentifier,
        /*0x43 |  C  */ &IsIdentifier,
        /*0x44 |  D  */ &IsIdentifier,
        /*0x45 |  E  */ &IsIdentifier,
        /*0x46 |  F  */ &IsIdentifier,
        /*0x47 |  G  */ &IsIdentifier,
        /*0x48 |  H  */ &IsIdentifier,
        /*0x49 |  I  */ &IsIdentifier,
        /*0x4A |  J  */ &IsIdentifier,
        /*0x4B |  K  */ &IsIdentifier,
        /*0x4C |  L  */ &IsIdentifier,
        /*0x4D |  M  */ &IsIdentifier,
        /*0x4E |  N  */ &IsIdentifier,
        /*0x4F |  O  */ &IsIdentifier,
        /*0x50 |  P  */ &IsIdentifier,
        /*0x51 |  Q  */ &IsIdentifier,
        /*0x52 |  R  */ &IsIdentifier,
        /*0x53 |  S  */ &IsIdentifier,
        /*0x54 |  T  */ &IsIdentifier,
        /*0x55 |  U  */ &IsIdentifier,
        /*0x56 |  V  */ &IsIdentifier,
        /*0x57 |  W  */ &IsIdentifier,
        /*0x58 |  X  */ &IsIdentifier,
        /*0x59 |  Y  */ &IsIdentifier,
        /*0x5A |  Z  */ &IsIdentifier,
        /*0x5B |  [  */ &IsOpenBrackets,
        /*0x5C |  \  */ &IsSlash,
        /*0x5D |  ]  */ &IsCloseBrackets,
        /*0x5E |  ^  */ &IsXorOrXorEquals,
        /*0x5F |  _  */ &IsIdentifier,
        /*0x60 |  `  */ &NotSupportedException,
        /*0x61 |  a  */ &IsAsm,
        /*0x62 |  b  */ &IsByteOrBool,
        /*0x63 |  c  */ &IsClassOrCharOrConst,
        /*0x64 |  d  */ &IsDoOrDouble,
        /*0x65 |  e  */ &IsElseOrEnum,
        /*0x66 |  f  */ &IsFalseOrForOrForeachOrFloat,
        /*0x67 |  g  */ &IsGet,
        /*0x68 |  h  */ &IsIdentifier,
        /*0x69 |  i  */ &IsInterfaceOrInternalOrIntOrIfOrIn,
        /*0x6A |  j  */ &IsIdentifier,
        /*0x6B |  k  */ &IsIdentifier,
        /*0x6C |  l  */ &IsLong,
        /*0x6D |  m  */ &IsIdentifier,
        /*0x6E |  n  */ &IsNamespaceOrNull,
        /*0x6F |  o  */ &IsIdentifier,
        /*0x70 |  p  */ &IsPublicOrPrivate,
        /*0x71 |  q  */ &IsIdentifier,
        /*0x72 |  r  */ &IsReturn,
        /*0x73 |  s  */ &IsSetOrStructOrShortOrStackallocOrStaticOrSByte,
        /*0x74 |  t  */ &IsTrueOrThis,
        /*0x75 |  u  */ &IsUsingOrUShortOrUIntOrULong,
        /*0x76 |  v  */ &IsVarOrVoid,
        /*0x77 |  w  */ &IsWhile,
        /*0x78 |  x  */ &IsIdentifier,
        /*0x79 |  y  */ &IsIdentifier,
        /*0x7A |  z  */ &IsIdentifier,
        /*0x7B |  {  */ &IsOpenBraces,
        /*0x7C |  |  */ &IsOrOrOrEqualsOrOrElse,
        /*0x7D |  }  */ &IsCloseBraces,
        /*0x7E |  ~  */ &IsInvertOrInvertEquals,
        /*0x7F | Del */ &NotSupportedException,
    };

    private static ReadOnlySpan<T> GetSpan<T>(T[] src)
    {
        return new ReadOnlySpan<T>(src);
    }
    
    [Obsolete("To Slow")]
    public unsafe static void Analyse(in ReadOnlySpan<char> src, Queue<LexerToken> level2Queue)
    {
        int i = 0;
        int end;
        int tmpEnd;

        LexerTokenType tokenType;
        LexerTokenType tmpTokenType;

        delegate*<in ReadOnlySpan<char>, int, out int, out LexerTokenType, bool>* call = null;

        var callref = __makeref(call);
        IntPtr* callPtr = *(IntPtr**)&callref;

        ;

        fixed (delegate*<in ReadOnlySpan<char>, int, out int, out LexerTokenType, bool>* ptr = _shortHand)
        {
            var valueref = __makeref(ptr[(int)src[i]]);
            IntPtr* valuePtr = *(IntPtr**)&valueref;


            while (i < src.Length)
            {
                if ((*call)(in src, i, out end, out tokenType))
                {
                    if (tokenType is >= ASM and <= Identifier)
                    {
                        if (CheckForLeadingSpaces(src, end, out tmpEnd, out tmpTokenType))
                        {
                            level2Queue.Enqueue(new LexerToken(tokenType, i, end));
                            level2Queue.Enqueue(new LexerToken(tmpTokenType, end, tmpEnd));

                            i = tmpEnd;
                        }
                        else
                        {
                            IsIdentifier(src, end, out tmpEnd, out tokenType);

                            level2Queue.Enqueue(new LexerToken(tokenType, i, tmpEnd));
                            i = tmpEnd;
                        }
                    }
                    else
                    {
                        level2Queue.Enqueue(new LexerToken(tokenType, i, end));
                        i = end;
                    }
                }
                else if (tokenType is Unknown)
                {
                    IsIdentifier(src, end, out tmpEnd, out tmpTokenType);

                    level2Queue.Enqueue(new LexerToken(tmpTokenType, i, tmpEnd));
                    i = tmpEnd;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

    /// <exception cref="NotSupportedException"></exception>
    public static bool NotSupportedException(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        throw new NotSupportedException();
    }

    private static bool CheckForLeadingSpaces(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        return IsSpace(src, i, out end, out type)
            || IsTab(src, i, out end, out type)
            || IsNewline(src, i, out end, out type)
            || IsCarriageReturn(src, i, out end, out type);
    }

    /// <summary> 0x[0-9a-f]+ or 0b[01]+ or [0-9]+ </summary>
    private static bool IsHexOrBinaryOrDigits(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        switch (src[end])
        {
            case 'x':
                end++;
                if (src[end] is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F')
                {
                    end++;
                    while (src[end] is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F')
                    {
                        end++;
                    }

                    type = HexNumber;
                    return true;
                }
                throw new NotSupportedException("Behind the prefix 0x ");
            case 'b':
                end++;
                if (src[end] is '0' or '1')
                {
                    end++;
                    while (src[end] is '0' or '1')
                    {
                        end++;
                    }

                    type = BinaryNumber;
                    return true;
                }
                throw new NotSupportedException("Behind the prefix 0b ");
        }

        while (src[end] is >= '0' and <= '9')
        {
            end++;
        }

        type = Digits;
        return true;
    }
    /// <summary> [0-9]+ </summary>
    private static bool IsDigits(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        while (src[end] is >= '0' and <= '9')
        {
            end++;
        }

        type = Digits;
        return true;
    }
    /// <summary> [A-Za-z_][A-Za-z0-9_]* </summary>
    private static bool IsIdentifier(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        while (src[end] is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_' or >= '0' and <= '9')
        {
            end++;
        }

        type = Identifier;
        return true;
    }
    /// <summary> [_] </summary>
    private static bool IsSpace(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        while (src[end] is ' ')
        {
            end++;
        }

        type = Whitespace;
        return true;
    }
    /// <summary> \t </summary>
    private static bool IsTab(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        while (src[end] is '\t')
        {
            end++;
        }

        type = Tab;
        return true;
    }
    /// <summary> \r\n </summary>
    private static bool IsCarriageReturn(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        if (src[i + 1] is '\n')
        {
            end = i + 2;
            type = Newline;
            return true;
        }

        throw new NotSupportedException("Carriage return without line feed");
    }
    /// <summary> \n </summary>
    private static bool IsNewline(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        while (src[end] is '\n')
        {
            end++;
        }

        type = Newline;
        return true;
    }
    /// <summary> asm </summary>
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
    /// <summary> byte or bool </summary>
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
    /// <summary> class or char or const </summary>
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
    /// <summary> do or double </summary>
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
    /// <summary> else or enum </summary>
    private static bool IsElseOrEnum(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'l':
                if (src[i + 1] is 's' && src[i + 2] is 'e') // else
                {
                    end = i + 3;
                    type = Else;
                    return true;
                }
                break;
            case 'n':
                if (src[i + 1] is 'u' && src[i + 2] is 'm') // enum
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
    /// <summary> false or for or foreach or float </summary>
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
    /// <summary> get </summary>
    private static bool IsGet(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if (src[i] is 'e' && src[i + 1] is 't') // get
        {
            end = i + 2;
            type = Get;
            return true;
        }

        end = i;
        type = Unknown;
        return false;
    }
    /// <summary> interface or int or if </summary>
    private static bool IsInterfaceOrInternalOrIntOrIfOrIn(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'n': // interface or interface
                if (src[i + 1] is 't')
                {
                    if (src[i + 2] is 'e' && src[i + 3] is 'r')
                    {

                        if (src[i + 4] is 'f' && src[i + 5] is 'a' && src[i + 6] is 'c' && src[i + 7] is 'e')
                        {
                            end = i + 8;
                            type = Interface;
                            return true;
                        }

                        if (src[i + 4] is 'n' && src[i + 5] is 'a' && src[i + 6] is 'l')
                        {
                            end = i + 7;
                            type = Internal;
                            return true;
                        }
                    }

                    // int
                    end = i + 2;
                    type = Int;
                    return true;
                }

                end = i + 1;
                type = In;
                return true;

            case 'f': // if
                end = i + 1;
                type = If;
                return true;
        }

        end = i;
        type = Unknown;
        return true;
    }
    /// <summary> long </summary>
    private static bool IsLong(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
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
    /// <summary> namespace or null </summary>
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
                if (src[i + 1] is 'l' && src[i + 2] is 'l') // null
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
    /// <summary> public or private </summary>
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
    /// <summary> return </summary>
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
    /// <summary> set or struct or short or stackalloc or static or sbyte </summary>
    private static bool IsSetOrStructOrShortOrStackallocOrStaticOrSByte(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        switch (src[i])
        {
            case 'b': // Sbyte
                if (src[i + 1] is 'y' && src[i + 2] is 't' && src[i + 3] is 'e') // Sbyte
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
    /// <summary> true or this </summary>
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
    /// <summary> using or ushort or uint or ulong </summary>
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
    /// <summary> var or void </summary>
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

        end = i;
        type = Unknown;
        return false;
    }
    /// <summary> while </summary>
    private static bool IsWhile(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        i++;
        if (src[i] is 'h' && src[i + 1] is 'i' && src[i + 2] is 'l' && src[i + 3] is 'e') // while
        {
            end = i + 4;
            type = While;
            return true;
        }

        end = i;
        type = Unknown;
        return false;
    }
    /// <summary> #... </summary>
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
    /// <summary> . </summary>
    private static bool IsDot(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = Dot;
        return true;
    }
    /// <summary> , </summary>
    private static bool IsComma(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = Comma;
        return true;
    }
    /// <summary> ' </summary>
    private static bool IsSingleQuotationMark(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = SingleQuotationMark;
        return true;
    }
    /// <summary> " </summary>
    private static bool IsDoubleQuotationMark(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = DoubleQuotationMark;
        return true;
    }
    /// <summary> : </summary>
    private static bool IsColon(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = Colon;
        return true;
    }
    /// <summary> ; </summary>
    private static bool IsSemicolon(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = Semicolon;
        return true;
    }
    /// <summary> ! or != </summary>
    private static bool IsExclamationMarkOrNot(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        if (src[i + 1] is '=')
        {
            end = i + 1;
            type = NotEqualsThen;
            return true;
        }

        end = i + 1;
        type = Not;
        return true;
    }
    /// <summary> ( </summary>
    private static bool IsOpenParenthesis(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = OpenParenthesis;
        return true;
    }
    /// <summary> ) </summary>
    private static bool IsCloseParenthesis(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = CloseParenthesis;
        return true;
    }
    /// <summary> [ </summary>
    private static bool IsOpenBrackets(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = OpenBrackets;
        return true;
    }
    /// <summary> ] </summary>
    private static bool IsCloseBrackets(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = CloseBrackets;
        return true;
    }
    /// <summary> { </summary>
    private static bool IsOpenBraces(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = OpenBraces;
        return true;
    }
    /// <summary> \ </summary>
    private static bool IsSlash(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        type = Slash;
        end = i + 1;
        return true;
    }
    /// <summary> } </summary>
    private static bool IsCloseBraces(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        end = i + 1;
        type = CloseBraces;
        return true;
    }
    /// <summary> = or == </summary>
    private static bool IsEqualOrEqualsThen(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        if (src[i + 1] is '=')
        {
            end = i + 1;
            type = EqualsThen;
            return true;
        }

        end = i + 1;
        type = Assign;
        return true;
    }
    /// <summary>  &#38; or &#38;= or &#38;&#38; </summary>
    private static bool IsAndOrAndEqualsOrAndAlso(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
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
    }
    /// <summary> | or |= or || </summary>
    private static bool IsOrOrOrEqualsOrOrElse(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        switch (src[i + 1])
        {
            case '=':
                end = i + 2;
                type = OrEquals;
                return true;
            case '|':
                end = i + 2;
                type = OrElse;
                return true;
        }

        end = i + 1;
        type = Or;

        return true;
    }
    /// <summary> ^ or ^= </summary>
    private static bool IsXorOrXorEquals(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        if (src[i + 1] is '=')
        {
            end = i + 2;
            type = XorEquals;
            return true;
        }

        end = i + 1;
        type = Xor;
        return true;
    }
    /// <summary> ~ or ~= </summary>
    private static bool IsInvertOrInvertEquals(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        if (src[i + 1] is '=')
        {
            end = i + 2;
            type = InvertEquals;
            return true;
        }

        end = i + 1;
        type = Invert;
        return true;
    }
    /// <summary> + or += or ++ </summary>
    private static bool IsPlusOrPlusEqualsOrAtomicIncreas(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
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
    }
    /// <summary> - or -= or -- </summary>
    private static bool IsMinusOrMinusEqualsOrAtomicDecrease(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
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
    }
    
    private static bool IsMultiplicationOrMultiplicationEqualsOrPowerOrPowerEquals(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        switch (src[i + 1])
        {
            case '=':
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
    }
    /// <summary> / or /= </summary>
    private static bool IsDivisionOrDivisionEquals(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        if (src[i + 1] is '=')
        {
            end = i + 1;
            type = DivisionEquals;
            return true;
        }

        end = i + 1;
        type = Division;
        return true;
    }
    /// <summary> % or %= </summary>
    private static bool IsModuloOrModuloEquals(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        if (src[i + 1] is '=')
        {
            end = i + 1;
            type = ModuloEquals;
            return true;
        }

        end = i + 1;
        type = Modulo;
        return true;
    }
    /// <summary> < or <= or << or <<= </summary>
    private static bool IsLessOrLessThenOrLeftShiftOrLeftShiftEquals(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        switch (src[i + 1])
        {
            case '=':
                end = i + 1;
                type = LessThen;
                return true;
            case '<':
                if (src[i + 2] is '=')
                {
                    end = i + 3;
                    type = LeftShiftEquals;
                    return true;
                }

                end = i + 2;
                type = LeftShift;
                return true;
        }

        end = i + 1;
        type = Less;
        return true;
    }
    /// <summary> > or >= or >> or >>= </summary>
    private static bool IsGreaterOrGreaterThenOrRightShiftOrRightShiftEquals(in ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
    {
        switch (src[i + 1])
        {
            case '=':
                end = i + 1;
                type = GreaterThen;
                return true;
            case '>':
                if (src[i + 2] is '=')
                {
                    end = i + 3;
                    type = RightShiftEquals;
                    return true;
                }

                end = i + 2;
                type = RightShift;
                return true;
        }

        end = i + 1;
        type = Greater;
        return true;
    }
}