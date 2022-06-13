using System.Collections.Generic;
using System.Collections.Immutable;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Statements;

public class BlockStatement : StatementSyntax
{
    private SyntaxToken OpenBracket { get; }
    public ImmutableArray<StatementSyntax> Statements { get; }
    private SyntaxToken ClosedBracket { get; }

    public BlockStatement(SyntaxToken openBracket, ImmutableArray<StatementSyntax> statements, SyntaxToken closedBracket)
    {
        OpenBracket = openBracket;
        Statements = statements;
        ClosedBracket = closedBracket;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.BlockStatement;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenBracket;
        foreach (StatementSyntax statement in Statements)
        {
            yield return statement;
        }
        yield return ClosedBracket;
    }
}