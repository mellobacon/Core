using Core.Compiler.CodeAnalysis.Binding.Expressions;

namespace Core.Compiler.CodeAnalysis.Binding.Statements;

public class ForBoundStatement : IBoundStatement
{
    public IBoundStatement Init { get; }
    public IBoundExpression Condition { get; }
    public IBoundExpression Iter { get; }
    public IBoundStatement Statement { get; }

    public ForBoundStatement(IBoundStatement init, IBoundExpression condition, IBoundExpression iter, 
        IBoundStatement statement)
    {
        Init = init;
        Condition = condition;
        Iter = iter;
        Statement = statement;
    }
    public BoundType BoundType => BoundType.ForStatement;
}