using Core.Compiler.CodeAnalysis.Binding.Expressions;

namespace Core.Compiler.CodeAnalysis.Binding.Statements;

public class IfBoundStatement : IBoundStatement
{
    public IBoundExpression Condition { get; }
    public IBoundStatement Statement { get; }
    public IBoundStatement? ElseStatement { get; }

    public IfBoundStatement(IBoundExpression condition, IBoundStatement statement, IBoundStatement? elseStatement)
    {
        Condition = condition;
        Statement = statement;
        ElseStatement = elseStatement;
    }

    public BoundType BoundType => BoundType.IfStatement;
}