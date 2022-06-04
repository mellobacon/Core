using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;

public class VariableExpression: ExpressionSyntax
{
    public SyntaxToken VariableToken { get; }

    public VariableExpression(SyntaxToken variableToken)
    {
        VariableToken = variableToken;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.VariableExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return VariableToken;
    }
}