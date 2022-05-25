using NSharp.Lex;
using NSharp.Lexer.Level;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NSharp.Benchmark")]

Console.WriteLine("Hello, World!");

string example = File.ReadAllText(@"Example.ns");

Queue<LexerToken> queue = Lexer.Analyse(example);

Console.WriteLine("Sleep");

; // break me 