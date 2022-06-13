using Core.Compiler.CodeAnalysis.Binding.Expressions;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding.Statements;

public class VariableBoundStatement : IBoundStatement
{
    public readonly Variable Variable;
    public readonly IBoundExpression Expression;

    public VariableBoundStatement(Variable variable, IBoundExpression expression)
    {
        Variable = variable;
        Expression = expression;
    }

    public BoundType BoundType => BoundType.VariableStatement;
}