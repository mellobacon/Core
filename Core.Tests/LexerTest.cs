using System.Collections.Generic;
using Core.Compiler.CodeAnalysis;
using Core.Compiler.CodeAnalysis.Lexer;
using Xunit;

namespace Core.Tests;
public static class LexerTest
{
    [Theory]
    [MemberData(nameof(GetTokens))]
    public static void Lexer_Can_Lex(string text, SyntaxTokenType type)
    {
        var tokens = new List<SyntaxToken>();
        Lexer lexer = new (SourceText.From(text));
        while (true)
        {
            SyntaxToken token = lexer.Lex();
            if (token.Type == SyntaxTokenType.EofToken)
            {
                break;
            }
            tokens.Add(token);
        }
        SyntaxToken t = Assert.Single(tokens);
        Assert.Equal(text, t.Text);
        Assert.Equal(type, t.Type);
    }

    private static IEnumerable<object[]> GetTokens()
    {
        var tokens = new (string text, SyntaxTokenType type)[]
        {
            ("1", SyntaxTokenType.NumberToken),
            ("123", SyntaxTokenType.NumberToken),
            ("1.23", SyntaxTokenType.NumberToken),
            ("1_000", SyntaxTokenType.NumberToken),
            ("1.2.3", SyntaxTokenType.BadToken),
            ("1000_", SyntaxTokenType.BadToken),
            ("100__000_______________000", SyntaxTokenType.NumberToken),
            
            ("+", SyntaxTokenType.PlusToken),
            ("-", SyntaxTokenType.MinusToken),
            ("/", SyntaxTokenType.SlashToken),
            ("*", SyntaxTokenType.StarToken),
            ("%", SyntaxTokenType.ModuloToken),
            ("^", SyntaxTokenType.HatToken),
            ("!", SyntaxTokenType.BangToken),
            
            ("<", SyntaxTokenType.LessThanToken),
            (">", SyntaxTokenType.MoreThanToken),
            ("<=", SyntaxTokenType.LessEqualsToken),
            (">=", SyntaxTokenType.MoreEqualsToken),
            ("=", SyntaxTokenType.EqualsToken),
            
            ("(", SyntaxTokenType.OpenParenToken),
            (")", SyntaxTokenType.ClosedParenToken),
            ("{", SyntaxTokenType.OpenBracketToken),
            ("}", SyntaxTokenType.ClosedBracketToken),

            ("||", SyntaxTokenType.DoublePipeToken),
            ("&&", SyntaxTokenType.DoubleAmpersandToken),
            ("==", SyntaxTokenType.EqualsEqualsToken),
            ("!=", SyntaxTokenType.NotEqualsToken),
            ("+=", SyntaxTokenType.PlusEqualsToken),
            ("-=", SyntaxTokenType.MinusEqualsToken),
            ("/=", SyntaxTokenType.SlashEqualsToken),
            ("*=", SyntaxTokenType.StarEqualsToken),
            ("%=", SyntaxTokenType.ModuloEqualsToken),
            
            ("False", SyntaxTokenType.FalseKeyword),
            ("True", SyntaxTokenType.TrueKeyword),
            
            ("let", SyntaxTokenType.VariableKeyword),
            
            ("if", SyntaxTokenType.IfKeyword),
            ("else", SyntaxTokenType.ElseKeyword),
            ("while", SyntaxTokenType.WhileKeyword),

            ("_Test", SyntaxTokenType.VariableToken),
            ("_1000", SyntaxTokenType.VariableToken),
            ("myVar", SyntaxTokenType.VariableToken),
            
            ("\"string\"", SyntaxTokenType.StringToken),
            ("\"str ing\"", SyntaxTokenType.StringToken),
            ("\"THE FITNESS GRAM PACER TEST IS A-\"", SyntaxTokenType.StringToken),
            ("\")(**^%^&(*uyGYU5789324U3J\"", SyntaxTokenType.StringToken),
            
            (";", SyntaxTokenType.SemicolonToken),
            (",", SyntaxTokenType.CommaToken),
        };
        foreach ((string text, SyntaxTokenType type) in tokens)
        {
            yield return new object[] {text, type};
        }
    }
}