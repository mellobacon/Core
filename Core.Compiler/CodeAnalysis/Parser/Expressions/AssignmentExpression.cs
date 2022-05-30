using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;
public class AssignmentExpression : ExpressionSyntax
{
    private readonly SyntaxToken _variableToken;
    private readonly SyntaxToken _equalsToken;
    private readonly ExpressionSyntax _expression;
    public AssignmentExpression(SyntaxToken variableToken, SyntaxToken equalsToken, ExpressionSyntax expression)
    {
        _variableToken = variableToken;
        _expression = expression;
        _equalsToken = equalsToken;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.AssignmentExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return _variableToken;
        yield return _equalsToken;
        yield return _expression;
    }
}