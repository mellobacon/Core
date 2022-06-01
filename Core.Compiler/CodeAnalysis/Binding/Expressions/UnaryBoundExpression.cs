using System;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;

public class UnaryBoundExpression : IBoundExpression
{
    public readonly UnaryBoundOperator Op;
    public readonly IBoundExpression Operand;

    public UnaryBoundExpression(UnaryBoundOperator op, IBoundExpression operand)
    {
        Op = op;
        Operand = operand;
    }
    
    public BoundType BoundType => BoundType.UnaryExpression;
    public Type Type => Op.Result;
}