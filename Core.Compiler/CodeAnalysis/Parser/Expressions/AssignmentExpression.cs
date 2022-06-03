using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;
public class AssignmentExpression : ExpressionSyntax
{
    public readonly SyntaxToken VariableToken;
    private readonly SyntaxToken _equalsToken;
    public readonly ExpressionSyntax Expression;
    public AssignmentExpression(SyntaxToken variableToken, SyntaxToken equalsToken, ExpressionSyntax expression)
    {
        VariableToken = variableToken;
        Expression = expression;
        _equalsToken = equalsToken;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.AssignmentExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return VariableToken;
        yield return _equalsToken;
        yield return Expression;
    }
}