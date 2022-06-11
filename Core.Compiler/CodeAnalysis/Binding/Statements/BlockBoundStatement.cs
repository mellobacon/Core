using System.Collections.Immutable;

namespace Core.Compiler.CodeAnalysis.Binding.Statements;

public class BlockBoundStatement : IBoundStatement
{
    public ImmutableArray<IBoundStatement> Statements { get; }

    public BlockBoundStatement(ImmutableArray<IBoundStatement> statements)
    {
        Statements = statements;
    }

    public BoundType BoundType => BoundType.BlockStatement;
}