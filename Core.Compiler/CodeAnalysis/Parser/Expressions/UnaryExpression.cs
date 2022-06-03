using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;

public class UnaryExpression : ExpressionSyntax
{
    public readonly SyntaxToken Op;
    public readonly ExpressionSyntax Expression;

    public UnaryExpression(SyntaxToken op, ExpressionSyntax expression)
    {
        Op = op;
        Expression = expression;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.UnaryExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Op;
        yield return Expression;
    }
}