using System;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;

public class MethodBoundExpression : IBoundExpression
{
    public FunctionSymbol Function { get; }
    public IBoundExpression Arg { get; }

    public MethodBoundExpression(FunctionSymbol function, IBoundExpression arg)
    {
        Function = function;
        Arg = arg;
    }

    public BoundType BoundType => BoundType.MethodExpression;
    public Type Type => Function._type;
}