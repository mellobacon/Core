using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;
public class BinaryBoundExpression : IBoundExpression
{
    public IBoundExpression Left { get; }
    public BoundBinaryOperator Op { get; }
    public IBoundExpression Right { get; }
    public BinaryBoundExpression(IBoundExpression left, BoundBinaryOperator op, IBoundExpression right)
    {
        Left = left;
        Op = op;
        Right = right;
    }

    public BoundType BoundType => BoundType.BinaryExpression;
    public TypeSymbol Type => Op.ResultType;
}