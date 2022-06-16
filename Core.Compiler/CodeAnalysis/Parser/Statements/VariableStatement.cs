using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Expressions;

namespace Core.Compiler.CodeAnalysis.Parser.Statements;

public class VariableStatement : StatementSyntax
{
    public readonly SyntaxToken VarType;
    private readonly SyntaxToken _declarationkeyword;
    public readonly SyntaxToken Variable;
    private readonly SyntaxToken _equals;
    public readonly ExpressionSyntax Expression;
    private readonly SyntaxToken _semicolon;

    public VariableStatement(SyntaxToken declarationkeyword, SyntaxToken varType, SyntaxToken variable, SyntaxToken equals,
        ExpressionSyntax expression, SyntaxToken semicolon)
    {
        _declarationkeyword = declarationkeyword;
        VarType = varType;
        Variable = variable;
        _equals = equals;
        Expression = expression;
        _semicolon = semicolon;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.VariableStatement;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return _declarationkeyword;
        yield return VarType;
        yield return Variable;
        yield return _equals;
        yield return Expression;
        yield return _semicolon;
    }
}