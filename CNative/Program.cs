using CNative;

Console.WriteLine("Hello, World!");

string example = @"public static double[,] Transpose(this double[,] self)
{
    double[,] trans = new double[self.Cols(), self.Rows()];

    for (int row = 0; row < self.Rows(); row++)
    {
        for (int col = 0; col < self.Cols(); col++)
        {
            trans[col, row] = self[row, col];
        }
    }

    return trans;
}";

Lexer lexer = new Lexer(example);
await lexer.AnalyseAsync();

; // break me 