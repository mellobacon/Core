using System.Collections.Immutable;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;

public class MethodBoundExpression : IBoundExpression
{
    public FunctionSymbol Function { get; }
    public ImmutableArray<IBoundExpression> Args { get; }

    public MethodBoundExpression(FunctionSymbol function, ImmutableArray<IBoundExpression> args)
    {
        Function = function;
        Args = args;
    }

    public BoundType BoundType => BoundType.MethodExpression;
    public TypeSymbol Type => Function.FunctionType;
}