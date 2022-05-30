using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Parser.Expressions;
public class BinaryExpression : ExpressionSyntax
{
    public readonly ExpressionSyntax Left;
    public readonly SyntaxToken Op;
    public readonly ExpressionSyntax Right;

    public BinaryExpression(ExpressionSyntax left, SyntaxToken op, ExpressionSyntax right)
    {
        Left = left;
        Op = op;
        Right = right;
    }

    public override SyntaxTokenType Type => SyntaxTokenType.BinaryExpression;
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Left;
        yield return Op;
        yield return Right;
    }
}