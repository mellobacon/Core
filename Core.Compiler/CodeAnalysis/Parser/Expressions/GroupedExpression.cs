using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;
public class GroupedExpression : ExpressionSyntax
{
    private readonly SyntaxToken _leftOp;
    public readonly ExpressionSyntax Expression;
    private readonly SyntaxToken _rightOp;
    public GroupedExpression(SyntaxToken leftOp, ExpressionSyntax expression, SyntaxToken rightOp)
    {
        _leftOp = leftOp;
        Expression = expression;
        _rightOp = rightOp;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.GroupedExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return _leftOp;
        yield return Expression;
        yield return _rightOp;
    }
}