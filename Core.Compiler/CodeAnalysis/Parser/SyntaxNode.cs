using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser;

public abstract class SyntaxNode
{
    public abstract SyntaxTokenType Type { get; }

    public abstract IEnumerable<SyntaxNode> GetChildren();
}