using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;

public class FunctionCallExpression : ExpressionSyntax
{
    public SyntaxToken Name { get; }
    public SyntaxToken OpenParen { get; }
    public ExpressionSyntax Arg { get; }
    public SyntaxToken ClosedParen { get; }

    public FunctionCallExpression(SyntaxToken name, SyntaxToken openParen, ExpressionSyntax arg, SyntaxToken closedParen)
    {
        Name = name;
        OpenParen = openParen;
        Arg = arg;
        ClosedParen = closedParen;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.FunctionCallExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Name;
        yield return OpenParen;
        yield return Arg;
        yield return ClosedParen;
    }
}