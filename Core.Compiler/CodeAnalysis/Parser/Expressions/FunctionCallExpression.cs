using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;

public class FunctionCallExpression : ExpressionSyntax
{
    private SyntaxToken Name { get; }
    private SyntaxToken OpenParen { get; }
    public ExpressionSyntax Arg { get; }
    private SyntaxToken ClosedParen { get; }

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