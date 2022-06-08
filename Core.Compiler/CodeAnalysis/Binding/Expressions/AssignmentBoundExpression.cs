using System;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;

public class AssignmentBoundExpression: IBoundExpression
{
    public Variable Variable { get; }
    public IBoundExpression Expression { get; }
    
    public SyntaxToken Operator { get; }
    
    public bool HasCompoundOp { get; }

    public AssignmentBoundExpression(Variable variable, IBoundExpression expression,  SyntaxToken compoundOp, bool hasCompoundOp)
    {
        Variable = variable;
        Expression = expression;
        Operator = compoundOp;
        HasCompoundOp = hasCompoundOp;
    }

    public BoundType BoundType => BoundType.AssignmentExpression;
    public Type Type => Expression.Type;
}