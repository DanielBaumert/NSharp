//namespace CNative1;

//using System;
//using System.Diagnostics;
//using static LexerTokenType;

//public enum LexerTokenType
//{
//    // keyword
//    /// <summary> asm </summary>
//    ASM,
//    /// <summary> byte </summary>
//    Byte,
//    /// <summary> bool </summary>
//    Bool,
//    /// <summary> class </summary>
//    Class,
//    /// <summary> char </summary>
//    Char,
//    /// <summary> const </summary>
//    Const,
//    /// <summary> do </summary>
//    Do,
//    /// <summary> double </summary>
//    Double,
//    /// <summary> else </summary>
//    Else,
//    /// <summary> enum </summary>
//    Enum,
//    /// <summary> false </summary>
//    False,
//    /// <summary> for </summary>
//    For,
//    /// <summary> foreach </summary>
//    Foreach,
//    /// <summary> float </summary>
//    Float,
//    /// <summary> get </summary>
//    Get,
//    /// <summary> interface </summary>
//    Interface,
//    /// <summary> int </summary>
//    Int,
//    /// <summary> if </summary>
//    If,
//    /// <summary> long </summary>
//    Long,
//    /// <summary> namespace </summary>
//    Namespace,
//    /// <summary> null </summary>
//    Null,
//    /// <summary> public </summary>
//    Public,
//    /// <summary> private </summary>
//    Private,
//    /// <summary> return </summary>
//    Return,
//    /// <summary> set </summary>
//    Set,
//    /// <summary> struct </summary>
//    Struct,
//    /// <summary> sbyte </summary>
//    SByte,
//    /// <summary> short </summary>
//    Short,
//    /// <summary> stackalloc </summary>
//    Stackalloc,
//    /// <summary> static </summary>
//    Static,
//    /// <summary> true </summary>
//    True,
//    /// <summary> this </summary>
//    This,
//    /// <summary> ushort </summary>
//    UShort,
//    /// <summary> uint </summary>
//    UInt,
//    /// <summary> ulong </summary>
//    Ulong,
//    /// <summary> using </summary>
//    Using,
//    /// <summary> var </summary>
//    Var,
//    /// <summary> void </summary>
//    Void,
//    /// <summary> while </summary>
//    While,
//    /// <summary> #define </summary>
//    PragmaDefine,
//    /// <summary> #if </summary>
//    PragmaIf,
//    /// <summary> #endif </summary>
//    PragmaEndIf,
//    /// <summary> [_A-Za-z][_A-Za-z0-9]* </summary>
//    Identifier,
//    /// <summary> \s </summary>
//    Space,
//    /// <summary> \t </summary>
//    Tab,
//    /// <summary> \n or \r\n </summary>
//    Newline,
//    // IsCompareOperator
//    /// <summary> &lt;= </summary>
//    LessThen,
//    /// <summary> >= </summary>
//    GreaterThen,
//    /// <summary> == </summary>
//    EqualsThen,
//    /// <summary> != </summary>
//    NotEqualsThen,
//    // Assign
//    /// <summary> = </summary>
//    Assign,
//    // BinaryAssign
//    /// <summary> &= </summary>
//    AndEquals,
//    /// <summary> |= </summary>
//    OrEquals,
//    /// <summary> ^= </summary>
//    XorEquals,
//    /// <summary> ~= </summary>
//    InvertEquals,
//    // MathmaticalAssign
//    /// <summary> += </summary>
//    PlusEquals,
//    /// <summary> -= </summary>
//    MinusEquals,
//    /// <summary> *= </summary>
//    MultiplicationEquals,
//    /// <summary> **= </summary>
//    PowerEquals,
//    /// <summary> /= </summary>
//    DivisionEquals,
//    /// <summary> &= </summary>
//    ModuloEquals,
//    // BinaryOperator
//    /// <summary> ~ </summary>
//    Invert,
//    /// <summary> ^ </summary>
//    Xor,
//    /// <summary> | </summary>
//    Or,
//    /// <summary> &#38; </summary>
//    And,
//    // MathmaticalOperator
//    /// <summary> + </summary>
//    Plus,
//    /// <summary> - </summary>
//    Minus,
//    /// <summary> * </summary>
//    Multiplication,
//    /// <summary> ** </summary>
//    Power,
//    /// <summary> / </summary>
//    Division,
//    /// <summary> % </summary>
//    Modulo,
//    // Symbols
//    /// <summary> . </summary>
//    Dot,
//    /// <summary> , </summary>
//    Comma,
//    /// <summary> ' </summary>
//    SingleQuotationMark,
//    /// <summary> " </summary>
//    DoubleQuotationMark,
//    /// <summary> : </summary>
//    Colon,
//    /// <summary> ; </summary>
//    Semicolon,
//    /// <summary> ! </summary>
//    ExclamationMark,
//    /// <summary> ( </summary>
//    OpenParenthesis,
//    /// <summary> ) </summary>
//    CloseParenthesis,
//    /// <summary> [ </summary>
//    OpenBrackets,
//    /// <summary> ] </summary>
//    CloseBrackets,
//    /// <summary> { </summary>
//    OpenBraces,
//    /// <summary> } </summary>
//    CloseBraces,
//    /// <summary> [0-9]+ </summary>
//    Numbers,
//    /// <summary> //[^$]* </summary>
//    SingleLineComment,
//    /// <summary> /* </summary>
//    InlineCommentStart,
//    /// <summary> */ </summary>
//    InlineCommentEnd,

//    Unknown = -1
//}

//[DebuggerDisplay("Type = {Type}")]
//internal readonly struct LexerToken
//{
//    public readonly LexerTokenType Type;
//    public readonly int Start;
//    public readonly int End;

//    public LexerToken(LexerTokenType type, int start, int end)
//    {
//        Type = type;
//        Start = start;
//        End = end;
//    }
//}


//internal class LexerLevel
//{
//    private string _src;

//    private Queue<LexerToken> _level2Queue;

//    public LexerLevel(
//        string src,
//        ref Queue<LexerToken> level2LexerQueue)
//    {
//        _src = src;
//        _level2Queue = level2LexerQueue;
//    }


//    public void Analyse()
//    {
//        int end = 0;
//        int i = 0;
//        LexerTokenType type;

//        ReadOnlySpan<char> src = _src.ToCharArray();

//        while (i < src.Length)
//        {
//            // IDEE: a = 0, b = 1 => array of func
//            switch (src[i])
//            {
//                case 'a': // asm
//                    if (src[i + 1] is 's' && src[i + 2] is 'm')
//                    {
//                        end = i + 3;
//                        type = ASM;
//                        return true;
//                    }
//                    break;
//                case 'b': // byte, bool
//                    switch (src[i + 1])
//	                {
//                        case 'y': // byte
//                            if (src[i + 2] is 't' && src[i + 3] is 'e')
//                            {
//                                end = i + 4;
//                                type = Byte;
//                                return true;
//                            }
//                            break;
//                        case 'o':  // bool
//                            if (src[i + 2] is 'o' && src[i + 3] is 'l')
//                            {
//                                end = i + 4;
//                                type = Bool;
//                                return true;
//                            }
//                            break;
//                    }
//                    break;
//                case 'c': // class, char, const
//                    switch (src[i + 1])
//                    {
//                        case 'h': // Char
//                            if (src[i + 2] is 'a' && src[i + 3] is 'r')
//                            {
//                                end = i + 4;
//                                type = Char;
//                                return true;
//                            }
//                            break;
//                        case 'l': // Class
//                            if (src[i + 2] is 'a' && src[i + 3] is 's' && src[i + 5] is 's')
//                            {
//                                end = i + 5;
//                                type = Class;
//                                return true;
//                            }
//                            break;
//                        case 'o': // Const
//                            if (src[i + 2] is 'n' && src[i + 3] is 's' && src[i + 4] is 't')
//                            {
//                                end = i + 5;
//                                type = Const;
//                                return true;
//                            }
//                            break;
//                    }
//                    break;
//                case 'd': //  do...while, double
//                    if (src[i] is 'o') // do
//                    {
//                        if (src[i + 1] is 'u' && src[i + 2] is 'b' && src[i + 3] is 'l' && src[i + 4] is 'e')
//                        {
//                            end = i + 5;
//                            type = Double;
//                            return true;
//                        }

//                        end = i + 1;
//                        type = Do;
//                        return true;
//                    }
//                    break;
//                case 'e': // else, enum
//                    switch (src[i + 1])
//                    {
//                        case 'l':
//                            if (src[i + 2] is 's' && src[i + 3] is 'e') // else
//                            {
//                                end = i + 4;
//                                type = Else;
//                                return true;
//                            }
//                            break;
//                        case 'n':
//                            if (src[i + 2] is 'u' && src[i + 3] is 'm') // enum
//                            {
//                                end = i + 4;
//                                type = Enum;
//                                return true;
//                            }
//                            break;
//                    }
//                    break;
//                case 'f': // false, for, foreach, float
//                    switch (src[i + 1])
//                    {
//                        case 'a': // false
//                            if (src[i + 2] is 'l' && src[i + 3] is 's' && src[i + 4] is 'e')
//                            {
//                                end = i + 5;
//                                type = False;
//                                return true;
//                            }
//                            break;
//                        case 'o': // for or foreach
//                            if (src[i + 2] is 'r')
//                            {
//                                if (src[i + 3] is 'e' && src[i + 4] is 'a' && src[i + 5] is 'c' && src[i + 6] is 'h') // foreach
//                                {
//                                    end = i + 7;
//                                    type = Foreach;
//                                    return true;
//                                }

//                                // for
//                                end = i + 3;
//                                type = For;
//                                return true;
//                            }
//                            break;
//                        case 'l': // float
//                            if (src[i + 2] is 'o' && src[i + 3] is 'a' && src[i + 4] is 't')
//                            {
//                                end = i + 5;
//                                type = Float;
//                                return true;
//                            }
//                            break;
//                    }
//                    break;
//                case 'g': // get
//                    if (src[i + 1] is 'e' && src[i + 2] is 't') // get
//                    {
//                        end = i + 3;
//                        type = Get;
//                        return true;
//                    }
//                    break;
//                case 'i': // interface, int, if
//                    switch (src[i + 1])
//                    {
//                        case 'n': // interface
//                            if (src[i + 2] is 't')
//                            {
//                                if (src[i + 3] is 'e' && src[i + 4] is 'r' && src[i + 5] is 'f' && src[i + 6] is 'a' && src[i + 7] is 'c' && src[i + 8] is 'e')
//                                {
//                                    end = i + 9;
//                                    type = Interface;
//                                    return true;
//                                }

//                                // int
//                                end = i + 3;
//                                type = Int;
//                                return true;
//                            }
//                            break;
//                        case 'f': // if
//                            end = i + 2;
//                            type = If;
//                            return true;
//                    }
//                    break;
//                case 'l': // long
//                    if (src[i + 1] is 'o' && src[i + 2] is 'n' && src[i + 3] is 'g')
//                    {
//                        end = i + 4;
//                        type = Long;
//                        return true;
//                    }
//                    break;
//                case 'n': // namespace null, 
//                    switch (src[i + 1])
//                    {
//                        case 'a': // namespace
//                            if (src[i + 2] is 'm' && src[i + 3] is 'e' && src[i + 4] is 's' && src[i + 5] is 'p' && src[i + 6] is 'a' && src[i + 7] is 'c' && src[i + 8] is 'e')
//                            {
//                                end = i + 9;
//                                type = Namespace;
//                                return true;
//                            }
//                            break;
//                        case 'u': // null
//                            if (src[i + 2] is 'l' && src[i + 3] is 'l') // null
//                            {
//                                end = i + 4;
//                                type = Null;
//                                return true;
//                            }
//                            break;
//                    }
//                    break;
//                case 'p': // public, private 
//                    switch (src[i + 1])
//                    {
//                        case 'u': // public
//                            if (src[i + 2] is 'b' && src[i + 3] is 'l' && src[i + 4] is 'i' && src[i + 5] is 'c')
//                            {
//                                end = i + 6;
//                                type = Public;
//                                return true;
//                            }
//                            break;
//                        case 'r': // private
//                            if (src[i + 2] is 'i' && src[i + 3] is 'v' && src[i + 4] is 'a' && src[i + 5] is 't' && src[i + 6] is 'e')
//                            {
//                                end = i + 7;
//                                type = Private;
//                                return true;
//                            }
//                            break;
//                    }
//                    break;
//                case 'r': // return
//                    if (src[i + 1] is 'e' && src[i + 2] is 't' && src[i + 3] is 'u' && src[i + 4] is 'r' && src[i + 5] is 'n') // return
//                    {
//                        end = i + 6;
//                        type = Return;
//                        return true;
//                    }
//                    break;
//                case 's': // struct, sbyte, short, stackalloc, static
//                    switch (src[i + 1])
//                    {
//                        case 'b': // Sbyte
//                            if (src[i + 2] is 'y' && src[i + 3] is 't' && src[i + 4] is 'e') // Sbyte
//                            {
//                                end = i + 5;
//                                type = SByte;
//                                return true;
//                            }
//                            break;
//                        case 'e': // set
//                            if (src[i + 2] is 't')
//                            {
//                                end = i + 3;
//                                type = Set;
//                                return true;
//                            }
//                            break;
//                        case 'h': // short
//                            if (src[i + 2] is 'o' && src[i + 3] is 'r' && src[i + 4] is 't')
//                            {
//                                end = i + 5;
//                                type = Short;
//                                return true;
//                            }
//                            break;
//                        case 't': // struct, stackalloc, static
//                            switch (src[i + 2])
//                            {
//                                case 'a': //  stackalloc, static
//                                    switch (src[i + 3])
//                                    {
//                                        case 'c': // stackalloc
//                                            if (src[i + 4] is 'k' && src[i + 5] is 'a' && src[i + 6] is 'l' && src[i + 7] is 'l' && src[i + 8] is 'o' && src[i + 9] is 'c')
//                                            {
//                                                end = i + 10;
//                                                type = Stackalloc;
//                                                return true;
//                                            }
//                                            break;
//                                        case 't': // static
//                                            if (src[i + 4] is 'i' && src[i + 5] is 'c')
//                                            {
//                                                end = i + 6;
//                                                type = Static;
//                                                return true;
//                                            }
//                                            break;
//                                    }
//                                    break;
//                                case 'r': // struct
//                                    if (src[i + 2] is 'u' && src[i + 3] is 'c' && src[i + 4] is 't') // Struct
//                                    {
//                                        end = i + 5;
//                                        type = Struct;
//                                        return true;
//                                    }
//                                    break;
//                            }
//                            break;
//                    }
//                    break;
//                case 't': // true, this
//                    switch (src[i + 1])
//                    {
//                        case 'h': // this
//                            if (src[i + 2] is 'i' && src[i + 3] is 's')
//                            {
//                                end = i + 4;
//                                type = This;
//                                return true;
//                            }
//                            break;
//                        case 'r': // true
//                            if (src[i + 2] is 'u' && src[i + 3] is 'e')
//                            {
//                                end = i + 4;
//                                type = True;
//                                return true;
//                            }
//                            break;
//                    }
//                    break;
//                case 'u': // ushort, uint, ulong, using [union]
//                    switch (src[i + 1])
//                    {
//                        case 's': // using, ushort
//                            switch (src[i + 2])
//                            {
//                                case 'i': // using
//                                    if (src[i + 3] is 'n' && src[i + 4] is 'g') // using
//                                    {
//                                        end = i + 5;
//                                        type = Using;
//                                        return true;
//                                    }
//                                    break;
//                                case 'h': // ushort
//                                    if (src[i + 3] is 'o' && src[i + 4] is 'r' && src[i + 5] is 't') // ushort
//                                    {
//                                        end = i + 6;
//                                        type = UShort;
//                                        return true;
//                                    }
//                                    break;
//                            }
//                            break;
//                        case 'i': // uint
//                            if (src[i + 2] is 'n' && src[i + 3] is 't') // uint
//                            {
//                                end = i + 4;
//                                type = UInt;
//                                return true;
//                            }
//                            break;
//                        case 'l': // ulong
//                            if (src[i + 2] is 'o' && src[i + 3] is 'n' && src[i + 4] is 'g') // ulong
//                            {
//                                end = i + 5;
//                                type = Ulong;
//                                return true;
//                            }
//                            break;
//                    }
//                    break;
//                case 'v': // var, void
//                    switch (src[i + 1])
//                    {
//                        case 'a':// var
//                            if (src[i + 2] is 'r')
//                            {
//                                end = i + 3;
//                                type = Var;
//                                return true;
//                            }
//                            break;
//                        case 'o': // void
//                            if (src[i + 2] is 'i' && src[i + 3] is 't')
//                            {
//                                end = i + 4;
//                                type = Void;
//                                return true;
//                            }
//                            break;
//                    }
//                    break;
//                case 'w': // while
//                    if (src[i + 1] is 'h' && src[i + 2] is 'i' && src[i + 3] is 'l' && src[i + 4] is 'e') // while
//                    {
//                        end = i + 5;
//                        type = While;
//                        return true;
//                    }
//                case '#': // #define, #if, #IfEnd
//                    switch (src[i + 1])
//                    {
//                        case 'd': // #define
//                            if (src[i + 2] is 'e' && src[i + 3] is 'f' && src[i + 4] is 'i' && src[i + 5] is 'n' && src[i + 6] is 'e')
//                            {
//                                end = i + 7;
//                                type = PragmaDefine;
//                                return true;
//                            }
//                            break;
//                        case 'e': // #endif
//                            if (src[i + 2] is 'n' && src[i + 3] is 'd' && src[i + 4] is 'i' && src[i + 5] is 'f')
//                            {
//                                end = i + 6;
//                                type = PragmaEndIf;
//                                return true;
//                            }
//                            break;
//                        case 'i': // #if
//                            if (src[i + 2] is 'f')
//                            {
//                                end = i + 3;
//                                type = PragmaIf;
//                                return true;
//                            }
//                            break;
//                    }
//                case ' ': // space
//                    end = i + 1;
//                    type = Space;
//                    return true;
//                case '\t': // tab
//                    end = i + 1;
//                    type = Tab;
//                    return true;
//                case '\r': // new line
//                    if (src[i + 1] is '\n')
//                    {
//                        end = i + 2;
//                        type = Newline;
//                        return true;
//                    }
//                    break;
//                case '\n': // new line
//                    end = i + 1;
//                    type = Newline;
//                    return true;
//                case '.':
//                    end = i + 1;
//                    type = Dot;
//                    return true;
//                case ',':
//                    end = i + 1;
//                    type = Comma;
//                    return true;
//                case '\'':
//                    end = i + 1;
//                    type = SingleQuotationMark;
//                    return true;
//                case '"':
//                    end = i + 1;
//                    type = DoubleQuotationMark;
//                    return true;
//                case ':':
//                    end = i + 1;
//                    type = Colon;
//                    return true;
//                case ';':
//                    end = i + 1;
//                    type = Semicolon;
//                    return true;
//                case '!':
//                    end = i + 1;
//                    type = ExclamationMark;
//                    return true;
//                case '(':
//                    end = i + 1;
//                    type = OpenParenthesis;
//                    return true;
//                case ')':
//                    end = i + 1;
//                    type = CloseParenthesis;
//                    return true;
//                case '[':
//                    end = i + 1;
//                    type = OpenBrackets;
//                    return true;
//                case ']':
//                    end = i + 1;
//                    type = CloseBrackets;
//                    return true;
//                case '{':
//                    end = i + 1;
//                    type = OpenBraces;
//                    return true;
//                case '}':
//                    end = i + 1;
//                    type = CloseBraces;
//                    return true;
//                /// binary
//                case '&':
//                    if (src[i + 1] is '=')
//                    {
//                        end = i + 2;
//                        type = AndEquals;
//                        return true;
//                    }
//                    end = i + 1;
//                    type = And;
//                    return true;
//                case '|':
//                    if (src[i + 1] is '=')
//                    {
//                        end = i + 2;
//                        type = OrEquals;
//                        return true;
//                    }
//                    end = i + 1;
//                    type = Or;
//                    return true;
//                case '^':
//                    if(src[i + 1] is '=')
//                    {
//                        end = i + 2;
//                        type = XorEquals;
//                        return true;
//                    }
//                    end = i + 1;
//                    type = Xor;
//                    return true;
//                case '~':
//                    if (src[i + 1] is '=')
//                    {
//                        end = i + 2;
//                        type = InvertEquals;
//                        return true;
//                    }
//                    end = i + 1;
//                    type = Invert;
//                    return true;
//                /// mathmatical
//                case '+':
//                    switch (src[i + 1])
//                    {
//                        case '=':
//                            end = i + 2;
//                            type = PlusEquals;
//                            return true;
//                        case ' ':
//                            end = i + 1;
//                            type = Plus;
//                            return true;
//                    }
//                    break;
//                case '-':
//                    switch(src[i + 1])
//                    {
//                        case '=':
//                            end = i + 2;
//                            type = MinusEquals;
//                            return true;
//                        case ' ':
//                            end = i + 1;
//                            type = Minus;
//                            return true;
//                    }
//                    break;
//                case '*':
//                    switch (src[i + 1])
//                    {
//                        case '*':
//                            if(src[i + 2] is '=')
//                            {
//                                end = i + 3;
//                                type = PowerEquals;
//                                return true;
//                            }
//                            break;
//                        case '=':
//                            end = i + 2;
//                            type = MultiplicationEquals;
//                            return true;
//                        case ' ':
//                            end = i + 1;
//                            type = Multiplication;
//                            return true;
//                    }
//                case '/':
//                    if (src[i + 1] is '=')
//                    {
//                        end = i + 1;
//                        type = DivisionEquals;
//                        return true;
//                    }
//                    end = i + 1;
//                    type = Division;
//                    return true;
//                case '%':
//                    if (src[i + 1] is '=')
//                    {
//                        end = i + 1;
//                        type = ModuloEquals;
//                        return true;
//                    }
//                    end = i + 1;
//                    type = ModuloEquals;
//                    return true;
//                /// compare
//                case '<':
//                    end = i + 1;
//                    type = LessThen;
//                    return true;
//                case '>':
//                    end = i + 1;
//                    type = GreaterThen;
//                    return true;
//                case '=':
//                    end = i + 1;
//                    type = EqualsThen;
//                    return true;
//                case '!':
//                    end = i + 1;
//                    type = NotEqualsThen;
//                    return true;
//                case '=': 
//                    if(src[i + 1] is '=' && ' ')
//                    {

//                    }
                    

//            }

//            end = 0;
//            type = Unknown;
//            return false;
//        }
//    }
//    private void AddLevel2Token(int start, int end, LexerTokenType type)
//    {
//        _level2Queue.Enqueue(new LexerToken(type, start, end));
//    }


  
   
//    private static bool IsWhile(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
//    {
//        i++;
       

//        end = 0;
//        type = Unknown;
//        return false;
//    }

//    private static bool IsPragma(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
//    {
//        // #define, #if, #endif
//        i++;
        

//        end = 0;
//        type = Unknown;
//        return false;
//    }

//    private static bool IsSpace(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
//    {
//        switch (src[i])
//        {
//            case ' ': // space
//                end = i + 1;
//                type = Space;
//                return true;
//            case '\t': // tab
//                end = i + 1;
//                type = Tab;
//                return true;
//            case '\r': // new line
//                if (src[i + 1] is '\n')
//                {
//                    end = i + 2;
//                    type = Newline;
//                    return true;
//                }
//                break;
//            case '\n': // new line
//                end = i + 1;
//                type = Newline;
//                return true;
//        }

//        end = 0;
//        type = Unknown;
//        return false;
//    }



//    private static bool IsSymbol(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
//    {
//        switch (src[i])
//        {
           
//        }

//        // if all false then end became 0 and type unknown
//        return IsAssignOperator(src, i, out end, out type)
//            || IsOperator(src, i, out end, out type);
//    }
//    private static bool IsAssignOperator(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
//    {
        

//        end = 0;
//        type = Unknown;
//        return false;
//    }
//    private static bool IsOperator(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
//    {
//        switch (src[i])
//        {
//            // binary
//            case '~':
//                end = i + 1;
//                type = Invert;
//                return true;
//            case '^':
//                end = i + 1;
//                type = Xor;
//                return true;
//            case '|':
//                end = i + 1;
//                type = Or;
//                return true;
//            case '&':
//                end = i + 1;
//                type = And;
//                return true;
//            // math
//            case '+':
//                end = i + 1;
//                type = Plus;
//                return true;
//            case '-':
//                end = i + 1;
//                type = Minus;
//                return true;
//            case '*':
//                switch (src[i + 1])
//                {
//                    case '*': // power 
//                        end = i + 2;
//                        type = Power;
//                        return true;
//                    case '/': // comment
//                        end = i + 2;
//                        type = InlineCommentEnd;
//                        return true;
//                }
//                end = i + 1;
//                type = Multiplication;
//                return true;
//            case '/':
//                switch (src[i + 1])
//                {
//                    case '*': // inline comment
//                        end = i + 2;
//                        type = InlineCommentStart;
//                        return true;
//                    case '/': // comment
//                        end = i + 2;
//                        type = SingleLineComment;
//                        return true;
//                }
//                end = i + 1;
//                type = Division;
//                return true;
//            case '%':
//                end = i + 1;
//                type = Modulo;
//                return true;
//        }

//        end = 0;
//        type = Unknown;
//        return false;
//    }

//    // todo remove into the parser
//    private static bool IsIdentifier(ReadOnlySpan<char> src, int i, out int end, out LexerTokenType type)
//    {
//        // starts with [A-Za-z]
//        if (src[i] is '_' or (>= 'A' and <= 'Z') or (>= 'a' and <= 'z'))
//        {
//            i++;
//            // followed by [A-Za-z0-9_]*
//            while (i < src.Length && src[i] is '_' or (>= 'A' and <= 'Z') or (>= 'a' and <= 'z') or (>= '0' and <= '9'))
//            {
//                i++;
//            }
//            end = i;
//            type = Unknown;
//            return true;
//        }

//        end = 0;
//        type = Unknown;
//        return false;
//    }
//}
