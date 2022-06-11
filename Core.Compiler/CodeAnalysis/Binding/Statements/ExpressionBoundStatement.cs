using Core.Compiler.CodeAnalysis.Binding.Expressions;

namespace Core.Compiler.CodeAnalysis.Binding.Statements;

public class ExpressionBoundStatement : IBoundStatement
{
    public readonly IBoundExpression Expression;

    public ExpressionBoundStatement(IBoundExpression expression)
    {
        Expression = expression;
    }

    public BoundType BoundType => BoundType.ExpressionStatement;
}