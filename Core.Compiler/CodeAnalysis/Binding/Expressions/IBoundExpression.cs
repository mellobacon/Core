using System;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;
public interface IBoundExpression : IBindNode
{
    public Type Type { get; }
}