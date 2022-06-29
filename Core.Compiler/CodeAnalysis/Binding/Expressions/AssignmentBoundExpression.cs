using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding.Expressions;

public class AssignmentBoundExpression: IBoundExpression
{
    public VariableSymbol Variable { get; }
    public IBoundExpression Expression { get; }
    
    public SyntaxToken Operator { get; }
    
    public bool HasCompoundOp { get; }

    public AssignmentBoundExpression(VariableSymbol variable, IBoundExpression expression,  SyntaxToken compoundOp, bool hasCompoundOp)
    {
        Variable = variable;
        Expression = expression;
        Operator = compoundOp;
        HasCompoundOp = hasCompoundOp;
    }

    public BoundType BoundType => BoundType.AssignmentExpression;
    public TypeSymbol Type => Expression.Type;
}