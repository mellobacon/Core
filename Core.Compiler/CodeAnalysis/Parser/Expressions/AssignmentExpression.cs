using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;
public class AssignmentExpression : ExpressionSyntax
{
    public readonly SyntaxToken VariableToken;
    private readonly SyntaxToken _equalsToken;
    public readonly ExpressionSyntax Expression;
    public readonly SyntaxToken Operator;
    public readonly bool IsCompoundOp;
    public AssignmentExpression(SyntaxToken variableToken, SyntaxToken equalsToken, ExpressionSyntax expression)
    {
        VariableToken = variableToken;
        Expression = expression;
        _equalsToken = equalsToken;
    }

    public AssignmentExpression(SyntaxToken variableToken, SyntaxToken equalsToken, SyntaxToken compoundOp, ExpressionSyntax expression,
        bool isCompoundOp) : this(variableToken, equalsToken, expression)
    {
        IsCompoundOp = isCompoundOp;
        Operator = compoundOp;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.AssignmentExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return VariableToken;
        yield return _equalsToken;
        yield return Expression;
    }
}