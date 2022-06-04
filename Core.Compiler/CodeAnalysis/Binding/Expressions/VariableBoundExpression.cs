using System;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;

public class VariableBoundExpression: IBoundExpression
{
    public Variable Variable { get; }
    public VariableBoundExpression(Variable variable)
    {
        Variable = variable;
    }

    public BoundType BoundType => BoundType.VariableExpression;
    public Type Type => Variable.Type;
}