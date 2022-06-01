using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;

public class UnaryExpression : ExpressionSyntax
{
    private readonly SyntaxToken _op;
    private readonly ExpressionSyntax _expression;

    public UnaryExpression(SyntaxToken op, ExpressionSyntax expression)
    {
        _op = op;
        _expression = expression;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.UnaryExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return _op;
        yield return _expression;
    }
}