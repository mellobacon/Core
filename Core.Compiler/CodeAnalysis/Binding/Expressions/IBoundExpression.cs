using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;
public interface IBoundExpression : IBindNode
{
    public TypeSymbol Type { get; }
}