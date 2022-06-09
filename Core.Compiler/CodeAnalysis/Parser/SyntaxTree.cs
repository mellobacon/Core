using Core.Compiler.CodeAnalysis.Errors;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Expressions;

namespace Core.Compiler.CodeAnalysis.Parser;
public class SyntaxTree
{
    public readonly ExpressionSyntax Root;
    private SyntaxToken _eofToken;
    public readonly ErrorList Errors;
    public readonly SourceText Text;

    public SyntaxTree(ExpressionSyntax expression, SyntaxToken eofToken, ErrorList errors, SourceText text)
    {
        Root = expression;
        _eofToken = eofToken;
        Errors = errors;
        Text = text;
    }

    public static SyntaxTree Parse(string text)
    {
        SourceText sourcetext = SourceText.From(text);
        var parser = new Parser(sourcetext);
        return parser.Parse();
    }
}
