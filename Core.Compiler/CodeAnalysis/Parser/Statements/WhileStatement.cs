using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Expressions;

namespace Core.Compiler.CodeAnalysis.Parser.Statements;

public class WhileStatement : StatementSyntax
{
    public SyntaxToken Whilekeyword { get; }
    public SyntaxToken Openparen { get; }
    public ExpressionSyntax Condition { get; }
    public SyntaxToken Closeparen { get; }
    public StatementSyntax Dostatement { get; }

    public WhileStatement(SyntaxToken whilekeyword, SyntaxToken openparen, ExpressionSyntax condition, 
        SyntaxToken closeparen, StatementSyntax dostatement)
    {
        Whilekeyword = whilekeyword;
        Openparen = openparen;
        Condition = condition;
        Closeparen = closeparen;
        Dostatement = dostatement;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.WhileStatement;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Whilekeyword;
        yield return Openparen;
        yield return Condition;
        yield return Closeparen;
        yield return Dostatement;
    }
}