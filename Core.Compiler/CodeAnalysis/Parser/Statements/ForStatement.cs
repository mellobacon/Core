using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Expressions;

namespace Core.Compiler.CodeAnalysis.Parser.Statements;

public class ForStatement : StatementSyntax
{
    private SyntaxToken Forkeyword { get; }
    private SyntaxToken Openparen { get; }
    public StatementSyntax Initializer { get; }
    public ExpressionSyntax Condition { get; }
    private SyntaxToken Semicolon { get; }
    public ExpressionSyntax Iterator { get; }
    private SyntaxToken Closedparen { get; }
    public StatementSyntax Dostatement { get; }

    public ForStatement(SyntaxToken forkeyword, SyntaxToken openparen, StatementSyntax initializer, 
        ExpressionSyntax condition, SyntaxToken semicolon, ExpressionSyntax iterator, SyntaxToken closedparen, 
        StatementSyntax dostatement)
    {
        Forkeyword = forkeyword;
        Openparen = openparen;
        Initializer = initializer;
        Condition = condition;
        Semicolon = semicolon;
        Iterator = iterator;
        Closedparen = closedparen;
        Dostatement = dostatement;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.ForStatement;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Forkeyword;
        yield return Openparen;
        yield return Initializer;
        yield return Condition;
        yield return Semicolon;
        yield return Iterator;
        yield return Closedparen;
        yield return Dostatement;
    }
}