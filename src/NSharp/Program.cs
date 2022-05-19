using NSharp;
using NSharp.Lex;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NSharp.Benchmark")]

Console.WriteLine("Hello, World!");

string example = File.ReadAllText("./../.../../../../LexerLevel2.cs");

Stopwatch sw = new Stopwatch();
sw.Start();

Lexer.Analyse(example);


sw.Stop();
Console.WriteLine(sw.Elapsed.TotalMilliseconds);

; // break me 