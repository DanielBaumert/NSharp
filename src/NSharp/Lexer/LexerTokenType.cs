namespace NSharp.Lex;

public enum LexerTokenType
{
    // keyword
    /// <summary> asm </summary>
    ASM,
    /// <summary> alias </summary>
    Alias,
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
    /// <summary> else </summary>
    Else,
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
    /// <summary> import </summary>
    Import,
    /// <summary> interface </summary>
    Interface,
    /// <summary> internal </summary>
    Internal,
    /// <summary> int </summary>
    Int,
    /// <summary> if </summary>
    If,
    /// <summary> in </summary>
    In,
    /// <summary> long </summary>
    Long,
    /// <summary> namespace </summary>
    Namespace,
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
    /// <summary> short </summary>
    Short,
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
    /// <summary> # </summary>
    Hash,
    /// <summary> #define </summary>
    PragmaDefine,
    /// <summary> #if </summary>
    PragmaIf,
    /// <summary> #endif </summary>
    PragmaEndIf,
    /// <summary> [_A-Za-z][_A-Za-z0-9]* </summary>
    Identifier,
    /// <summary> 0x[0-9A-F]+ </summary>
    HexNumber,
    /// <summary> 0b[0-1]+ </summary>
    BinaryNumber,
    /// <summary> [0-9]+ </summary>
    Digits,
    /// <summary> \s </summary>
    Whitespace,
    /// <summary> \t </summary>
    Tab,
    /// <summary> \n or \r\n </summary>
    Newline,
    // IsCompareOperator
    /// <summary> &lt; </summary>
    Less,
    /// <summary> &lt;= </summary>
    LessThen,
    /// <summary> > </summary>
    Greater,
    /// <summary> >= </summary>
    GreaterThen,
    /// <summary> == </summary>
    EqualsThen,
    /// <summary> != </summary>
    NotEqualsThen,
    /// <summary> &#38;&#38; </summary>
    AndAlso,
    /// <summary> || </summary>
    OrElse,
    // Assign
    /// <summary> = </summary>
    Assign,
    // BinaryAssign
    /// <summary> &#38;= </summary>
    AndEquals,
    /// <summary> |= </summary>
    OrEquals,
    /// <summary> ^= </summary>
    XorEquals,
    /// <summary> ~= </summary>
    InvertEquals,
    /// <summary> &lt;&lt;= </summary>
    LeftShiftEquals,
    /// <summary> >>= </summary>
    RightShiftEquals,
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
    /// <summary> %= </summary>
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
    /// <summary> &lt;&lt; </summary>
    LeftShift,
    /// <summary> >> </summary>
    RightShift,
    // MathmaticalOperator
    /// <summary> + </summary>
    Plus,
    /// <summary> ++ </summary>
    AtomicIncreas,
    /// <summary> - </summary>
    Minus,
    /// <summary> -- </summary>
    AtomicDecrease,
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
    Not,
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
    /// <summary> /* </summary>
    InlineCommentStart,
    /// <summary> /* ... */ </summary>
    InlineComment,
    /// <summary> */ </summary>
    InlineCommentEnd,
    /// <summary> //[^$]* </summary>
    SingleLineComment,
    /// <summary> \ </summary>
    Slash,

    Unknown = -1
}
