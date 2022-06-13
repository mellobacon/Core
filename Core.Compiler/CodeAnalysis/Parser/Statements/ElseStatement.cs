using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Statements;

public class ElseStatement : StatementSyntax
{
    private SyntaxToken Elsekeyword { get; }
    public StatementSyntax Statement { get; }

    public ElseStatement(SyntaxToken elsekeyword, StatementSyntax statement)
    {
        Elsekeyword = elsekeyword;
        Statement = statement;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.ElseStatement;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Elsekeyword;
        yield return Statement;
    }
}