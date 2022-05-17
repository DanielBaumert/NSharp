namespace NSharp.Parser.Models;

internal abstract class SyntaxNodeBase
{

}

internal abstract class SyntaxKeyNodeBase : SyntaxNodeBase
{
    public List<SyntaxNodeBase> Children { get; } = new List<SyntaxNodeBase>();
    public List<SyntaxTriviaNode> Lead { get; } = new List<SyntaxTriviaNode>();
    public List<SyntaxTriviaNode> Trail { get; } = new List<SyntaxTriviaNode>();
}

internal abstract class SyntaxTokenBase : SyntaxNodeBase
{
    
}

internal class Identifier : SyntaxTokenBase
{
    public string Value { get; set; }
}
