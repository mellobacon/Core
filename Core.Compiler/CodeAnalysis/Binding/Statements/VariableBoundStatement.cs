using Core.Compiler.CodeAnalysis.Binding.Expressions;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding.Statements;

public class VariableBoundStatement : IBoundStatement
{
    public readonly VariableSymbol Variable;
    public readonly IBoundExpression Expression;

    public VariableBoundStatement(VariableSymbol variable, IBoundExpression expression)
    {
        Variable = variable;
        Expression = expression;
    }

    public BoundType BoundType => BoundType.VariableStatement;
}