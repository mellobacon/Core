using System;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;
public class LiteralBoundExpression : IBoundExpression
{
    public object Value { get; }

    public LiteralBoundExpression(object value)
    {
        Value = value;
        Type = value switch
        {
            bool => TypeSymbol.Bool,
            int => TypeSymbol.Int,
            float => TypeSymbol.Float,
            double => TypeSymbol.Double,
            string => TypeSymbol.String,
            _ => throw new Exception($"Unexpected literal '{Value}' of type {Value.GetType()}")
        };
    }

    public BoundType BoundType => BoundType.LiteralExpression;
    public TypeSymbol Type { get; }
}