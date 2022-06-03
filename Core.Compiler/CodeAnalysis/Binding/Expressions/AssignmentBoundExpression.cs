using System;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;

public class AssignmentBoundExpression: IBoundExpression
{
    public Variable Variable { get; }
    public IBoundExpression Expression { get; }

    public AssignmentBoundExpression(Variable variable, IBoundExpression expression)
    {
        Variable = variable;
        Expression = expression;
    }

    public BoundType BoundType => BoundType.AssignmentExpression;
    public Type Type => Expression.Type;
}