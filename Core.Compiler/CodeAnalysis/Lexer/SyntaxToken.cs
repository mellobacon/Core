using System.Collections.Generic;
using System.Linq;
using Core.Compiler.CodeAnalysis.Parser;

namespace Core.Compiler.CodeAnalysis.Lexer;
public class SyntaxToken : SyntaxNode
{
    public readonly string? Text;
    public readonly object? Value;
    public readonly int Position;
    public TextSpan TextSpan
    {
        get
        {
            if (Text != null)
            {
                return new TextSpan(Position - Text.Length, Text.Length);
            }

            return default;
        }
    }

    public override SyntaxTokenType Type { get; }

    public SyntaxToken(string? text, object? value, SyntaxTokenType type, int position)
    {
        Text = text;
        Value = value;
        Type = type;
        Position = position;
    }
        
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        return Enumerable.Empty<SyntaxNode>(); // Theres no node to return so fuck off
    }
}