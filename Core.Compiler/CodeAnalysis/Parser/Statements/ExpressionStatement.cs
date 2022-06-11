using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Expressions;

namespace Core.Compiler.CodeAnalysis.Parser.Statements;

public class ExpressionStatement : StatementSyntax
{
    public readonly ExpressionSyntax Expression;
    private readonly SyntaxToken _semicolon;

    public ExpressionStatement(ExpressionSyntax expression, SyntaxToken semicolon)
    {
        Expression = expression;
        _semicolon = semicolon;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.ExpressionStatement;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Expression;
        yield return _semicolon;
    }
}