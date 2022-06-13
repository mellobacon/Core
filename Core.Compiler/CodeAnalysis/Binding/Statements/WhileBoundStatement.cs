using Core.Compiler.CodeAnalysis.Binding.Expressions;

namespace Core.Compiler.CodeAnalysis.Binding.Statements;

public class WhileBoundStatement : IBoundStatement
{
    public IBoundExpression Condition { get; }
    public IBoundStatement Statement { get; }

    public WhileBoundStatement(IBoundExpression condition, IBoundStatement statement)
    {
        Condition = condition;
        Statement = statement;
    }

    public BoundType BoundType => BoundType.WhileStatement;
}