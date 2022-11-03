using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Statements;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;

public class FunctionDeclarationExpression : StatementSyntax
{
    public readonly SyntaxToken FuncType;
    public readonly SyntaxToken Name;
    private readonly SyntaxToken _openParen;
    public readonly Parameters<ExpressionSyntax> Args;
    private readonly SyntaxToken _closedParen;
    private readonly StatementSyntax _statement;

    public FunctionDeclarationExpression(SyntaxToken funcType, SyntaxToken name, SyntaxToken openParen, 
        Parameters<ExpressionSyntax> args, SyntaxToken closedParen, StatementSyntax statement)
    {
        FuncType = funcType;
        Name = name;
        Args = args;
        _openParen = openParen;
        _closedParen = closedParen;
        _statement = statement;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.FunctionDeclarationExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return FuncType;
        yield return Name;
        yield return _openParen;
        foreach (ExpressionSyntax arg in Args)
        {
            yield return arg;
        }
        yield return _closedParen;
        yield return _statement;
    }
}