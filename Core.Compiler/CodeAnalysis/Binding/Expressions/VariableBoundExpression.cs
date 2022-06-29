using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;

public class VariableBoundExpression: IBoundExpression
{
    public VariableSymbol Variable { get; }
    public VariableBoundExpression(VariableSymbol variable)
    {
        Variable = variable;
    }

    public BoundType BoundType => BoundType.VariableExpression;
    public TypeSymbol Type => Variable.VarType;
}