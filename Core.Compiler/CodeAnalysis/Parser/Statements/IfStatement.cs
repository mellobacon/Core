using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Expressions;

namespace Core.Compiler.CodeAnalysis.Parser.Statements;

public class IfStatement: StatementSyntax
{
    private SyntaxToken Ifkeyword { get; }
    private SyntaxToken Openparen { get; }
    public ExpressionSyntax Condition { get; }
    private SyntaxToken Closedparen { get; }
    public StatementSyntax Thenstatement { get; }
    public ElseStatement? Elsestatement { get; }

    public IfStatement(SyntaxToken ifkeyword, SyntaxToken openparen, ExpressionSyntax condition, 
        SyntaxToken closedparen, StatementSyntax thenstatement, ElseStatement? elsestatement)
    {
        Ifkeyword = ifkeyword;
        Openparen = openparen;
        Condition = condition;
        Closedparen = closedparen;
        Thenstatement = thenstatement;
        Elsestatement = elsestatement;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.IfStatement;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Ifkeyword;
        yield return Openparen;
        yield return Condition;
        yield return Closedparen;
        yield return Thenstatement;
        if (Elsestatement != null) yield return Elsestatement;
    }
}