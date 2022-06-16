using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;

public class ErrorBoundExpression : IBoundExpression
{
    public BoundType BoundType => BoundType.ErrorExpression;
    public TypeSymbol Type => TypeSymbol.Error;
}