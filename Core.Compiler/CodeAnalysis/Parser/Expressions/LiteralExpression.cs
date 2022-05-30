using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;
public class LiteralExpression : ExpressionSyntax
{
    private readonly SyntaxToken _token;
    public readonly object? Value;

    public LiteralExpression(SyntaxToken token, object? value)
    {
        _token = token;
        Value = value;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.LiteralExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return _token;
    }
}