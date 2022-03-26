using CNative;

Console.WriteLine("Hello, World!");

string example = File.ReadAllText("./../.../../../../LexerLevel2.cs");

Lexer lexer = new Lexer(example);
await lexer.AnalyseAsync();

; // break me 