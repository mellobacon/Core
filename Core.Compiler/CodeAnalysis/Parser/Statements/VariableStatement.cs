using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Expressions;

namespace Core.Compiler.CodeAnalysis.Parser.Statements;

public class VariableStatement : StatementSyntax
{
    private readonly SyntaxToken Declarationkeyword;
    public readonly SyntaxToken Variable;
    private new readonly SyntaxToken Equals;
    public readonly ExpressionSyntax Expression;
    private readonly SyntaxToken Semicolon;
    
    public VariableStatement(SyntaxToken declarationkeyword, SyntaxToken variable, SyntaxToken equals,
        ExpressionSyntax expression, SyntaxToken semicolon)
    {
        Declarationkeyword = declarationkeyword;
        Variable = variable;
        Equals = equals;
        Expression = expression;
        Semicolon = semicolon;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.VariableStatement;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Declarationkeyword;
        yield return Variable;
        yield return Equals;
        yield return Expression;
        yield return Semicolon;
    }
}