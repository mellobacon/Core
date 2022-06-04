﻿namespace Core.Compiler.CodeAnalysis.Lexer;
public static class SyntaxInfo
{
    public static int GetUnaryPrecedence(SyntaxTokenType type)
    {
        return type switch
        {
            SyntaxTokenType.MinusToken => 8,
            SyntaxTokenType.BangToken => 7,
            _ => 0
        };
    }
    public static int GetBinaryPrecedence(SyntaxTokenType type)
    {
        // follows pemdas
        return type switch
        {
            SyntaxTokenType.HatToken => 6,
            SyntaxTokenType.SlashToken => 5,
            SyntaxTokenType.StarToken => 5,
            SyntaxTokenType.PlusToken => 4,
            SyntaxTokenType.MinusToken => 4,
            SyntaxTokenType.ModuloToken => 4,
            SyntaxTokenType.LessThanToken => 3,
            SyntaxTokenType.MoreThanToken => 3,
            SyntaxTokenType.LessEqualsToken => 3,
            SyntaxTokenType.MoreEqualsToken => 3,
            SyntaxTokenType.EqualsEqualsToken => 3,
            SyntaxTokenType.DoubleAmpersandToken => 2,
            SyntaxTokenType.DoublePipeToken => 1,
            _ => 0
        };
    }

    public static SyntaxTokenType GetKeywordType(string text)
    {
        return text switch
        {
            "True" => SyntaxTokenType.TrueKeyword,
            "False" => SyntaxTokenType.FalseKeyword,
            "let" => SyntaxTokenType.VariableKeyword,
            _ => SyntaxTokenType.VariableToken
        };
    }
    
    public static string? GetText(SyntaxTokenType type)
    {
        return type switch
        {
            SyntaxTokenType.PlusToken => "+",
            SyntaxTokenType.MinusToken => "-",
            SyntaxTokenType.StarToken => "*",
            SyntaxTokenType.SlashToken => "/",
            SyntaxTokenType.OpenParenToken => "(",
            SyntaxTokenType.ClosedParenToken => ")",
            SyntaxTokenType.DoublePipeToken => "||",
            SyntaxTokenType.DoubleAmpersandToken => "&&",
            SyntaxTokenType.ModuloToken => "%",
            SyntaxTokenType.LessThanToken => "<",
            SyntaxTokenType.MoreThanToken => ">",
            SyntaxTokenType.LessEqualsToken => "<=",
            SyntaxTokenType.MoreEqualsToken => ">=",
            SyntaxTokenType.EqualsEqualsToken => "==",
            SyntaxTokenType.EqualsToken => "=",
            SyntaxTokenType.HatToken => "^",
            SyntaxTokenType.BangToken => "!",
            /*
            SyntaxTokenType.OpenBracketToken => "{",
            SyntaxTokenType.ClosedBracketToken => "}",
            SyntaxTokenType.SemicolonToken => ";",
            SyntaxTokenType.NotEqualsToken => "!=",
            SyntaxTokenType.StarStarToken => "**",
            SyntaxTokenType.PlusEqualsToken => "+=",
            SyntaxTokenType.MinusEqualsToken => "-=",
            SyntaxTokenType.SlashEqualsToken => "/=",
            SyntaxTokenType.StarEqualsToken => "*=",
            SyntaxTokenType.ModuloEqualsToken => "%=",
            */
            _ => null
        };
    }
}