using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;

public class FunctionCallExpression : ExpressionSyntax
{
    public SyntaxToken Name { get; }
    private SyntaxToken OpenParen { get; }
    public Parameters<ExpressionSyntax> Args { get; }
    private SyntaxToken ClosedParen { get; }

    public FunctionCallExpression(SyntaxToken name, SyntaxToken openParen, Parameters<ExpressionSyntax> args, SyntaxToken closedParen)
    {
        Name = name;
        OpenParen = openParen;
        Args = args;
        ClosedParen = closedParen;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.FunctionCallExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Name;
        yield return OpenParen;
        foreach (ExpressionSyntax arg in Args)
        {
            yield return arg;
        }
        yield return ClosedParen;
    }
}