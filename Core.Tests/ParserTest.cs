using System;
using System.Collections;
using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser;
using Core.Compiler.CodeAnalysis.Parser.Statements;
using Xunit;

namespace Core.Tests;
public static class ParserTest
{
    private static IEnumerable<SyntaxTokenType> BinaryOpTypes()
    {
        var types = (SyntaxTokenType[]) Enum.GetValues(typeof(SyntaxTokenType));
        foreach (SyntaxTokenType type in types)
        {
            if (SyntaxInfo.GetBinaryPrecedence(type) > 0)
            {
                yield return type;
            }
        }
    }

    private static IEnumerable BinaryOpData()
    {
        foreach (SyntaxTokenType op1 in BinaryOpTypes())
        {
            foreach (SyntaxTokenType op2 in BinaryOpTypes())
            {
                yield return new object[] { op1, op2 };
            }
        }
    }

    private static StatementSyntax ParseExpression(string text)
    {
        SyntaxTree tree = SyntaxTree.Parse(text); 
        StatementSyntax root = tree.Root;
        return root;
    }

    [Theory]
    [MemberData(nameof(BinaryOpData))]
    public static void Parser_Honors_Binary_Precedence(SyntaxTokenType type1, SyntaxTokenType type2)
    {
        int typeprecedence1 = SyntaxInfo.GetBinaryPrecedence(type1);
        int typeprecedence2 = SyntaxInfo.GetBinaryPrecedence(type2);
        string? typetext1 = SyntaxInfo.GetText(type1);
        string? typetext2 = SyntaxInfo.GetText(type2);

        var text = $"1 {typetext1} 2 {typetext2} 3;";
        StatementSyntax expression = ParseExpression(text);
        
        // What the tree should look like depending on precedence
        using var e = new AssertingNumerator(expression);
        if (typeprecedence2 > typeprecedence1)
        {
            e.AssertNode(SyntaxTokenType.ExpressionStatement);
                e.AssertNode(SyntaxTokenType.BinaryExpression);
                    e.AssertNode(SyntaxTokenType.LiteralExpression);
                        e.AssertToken(SyntaxTokenType.NumberToken, "1");
                e.AssertToken(type1, typetext1);
                e.AssertNode(SyntaxTokenType.BinaryExpression);
                    e.AssertNode(SyntaxTokenType.LiteralExpression);
                        e.AssertToken(SyntaxTokenType.NumberToken, "2");
                    e.AssertToken(type2, typetext2);
                    e.AssertNode(SyntaxTokenType.LiteralExpression);
                        e.AssertToken(SyntaxTokenType.NumberToken, "3");
            e.AssertToken(SyntaxTokenType.SemicolonToken, ";");
        }
        else
        {
            e.AssertNode(SyntaxTokenType.ExpressionStatement);
                e.AssertNode(SyntaxTokenType.BinaryExpression);
                    e.AssertNode(SyntaxTokenType.BinaryExpression);
                        e.AssertNode(SyntaxTokenType.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.NumberToken, "1");
                        e.AssertToken(type1, typetext1);
                        e.AssertNode(SyntaxTokenType.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.NumberToken, "2");
                        e.AssertToken(type2, typetext2);
                        e.AssertNode(SyntaxTokenType.LiteralExpression);
                            e.AssertToken(SyntaxTokenType.NumberToken, "3");
            e.AssertToken(SyntaxTokenType.SemicolonToken, ";");
        }
    }
    
    private static IEnumerable<SyntaxTokenType> UnaryOpTypes()
    {
        var types = (SyntaxTokenType[]) Enum.GetValues(typeof(SyntaxTokenType));
        foreach (SyntaxTokenType type in types)
        {
            if (SyntaxInfo.GetUnaryPrecedence(type) > 0)
            {
                yield return type;
            }
        }
    }
    
    private static IEnumerable UnaryOpData()
    {
        foreach (SyntaxTokenType op in UnaryOpTypes())
        {
            yield return new object[] { op };
        }
    }

    [Theory]
    [MemberData(nameof(UnaryOpData))]
    public static void Parser_Honors_Unary_Precedence(SyntaxTokenType type)
    {
        string? typetext = SyntaxInfo.GetText(type);
        var numbertext = $"{typetext}1;";
        StatementSyntax numberedexpression = ParseExpression(numbertext);
        using var e = new AssertingNumerator(numberedexpression);
        e.AssertNode(SyntaxTokenType.ExpressionStatement);
            e.AssertNode(SyntaxTokenType.UnaryExpression);
                e.AssertToken(type, typetext);
                e.AssertNode(SyntaxTokenType.LiteralExpression);
                    e.AssertToken(SyntaxTokenType.NumberToken, "1");
        e.AssertToken(SyntaxTokenType.SemicolonToken, ";");
    }

    [Fact]
    public static void Parser_Parses_Assignments()
    {
        StatementSyntax expression = ParseExpression("x = 5;");
        using var e = new AssertingNumerator(expression);
        
        e.AssertNode(SyntaxTokenType.ExpressionStatement);
            e.AssertNode(SyntaxTokenType.AssignmentExpression);
                e.AssertToken(SyntaxTokenType.VariableToken, "x");
                e.AssertToken(SyntaxTokenType.EqualsToken, "=");
                e.AssertNode(SyntaxTokenType.LiteralExpression);
                    e.AssertToken(SyntaxTokenType.NumberToken, "5");
        e.AssertToken(SyntaxTokenType.SemicolonToken, ";");        
    }

    [Fact]
    public static void Parser_Parses_Variables()
    {
        StatementSyntax expression = ParseExpression("x;");
        using var e = new AssertingNumerator(expression);
        e.AssertNode(SyntaxTokenType.ExpressionStatement);
            e.AssertNode(SyntaxTokenType.VariableExpression);
                e.AssertToken(SyntaxTokenType.VariableToken, "x");
        e.AssertToken(SyntaxTokenType.SemicolonToken, ";");    
    }
    
    [Fact]
    public static void Parser_Parses_Functions()
    {
        StatementSyntax expression = ParseExpression("println(\"stuff\");");
        using var e = new AssertingNumerator(expression);
        e.AssertNode(SyntaxTokenType.ExpressionStatement);
            e.AssertNode(SyntaxTokenType.FunctionCallExpression);
                e.AssertToken(SyntaxTokenType.VariableToken, "println");
                e.AssertToken(SyntaxTokenType.OpenParenToken, "(");
                e.AssertNode(SyntaxTokenType.LiteralExpression);
                    e.AssertToken(SyntaxTokenType.StringToken, "\"stuff\"");
                e.AssertToken(SyntaxTokenType.ClosedParenToken, ")");
            e.AssertToken(SyntaxTokenType.SemicolonToken, ";");
    }

    [Fact]
    public static void Parser_Parses_IfStatement()
    {
        StatementSyntax expression = ParseExpression("if (2 < 5) {} else {}");
        using var e = new AssertingNumerator(expression);
        e.AssertNode(SyntaxTokenType.IfStatement);
            e.AssertToken(SyntaxTokenType.IfKeyword, "if");
            e.AssertToken(SyntaxTokenType.OpenParenToken, "(");
            e.AssertNode(SyntaxTokenType.BinaryExpression);
                e.AssertNode(SyntaxTokenType.LiteralExpression);
                    e.AssertToken(SyntaxTokenType.NumberToken, "2");
                e.AssertToken(SyntaxTokenType.LessThanToken, "<");
                e.AssertNode(SyntaxTokenType.LiteralExpression);
                    e.AssertToken(SyntaxTokenType.NumberToken, "5");
            e.AssertToken(SyntaxTokenType.ClosedParenToken, ")");
            e.AssertNode(SyntaxTokenType.BlockStatement);
                e.AssertToken(SyntaxTokenType.OpenBracketToken, "{");
                e.AssertToken(SyntaxTokenType.ClosedBracketToken, "}");
            e.AssertNode(SyntaxTokenType.ElseStatement);
                e.AssertToken(SyntaxTokenType.ElseKeyword, "else");
                e.AssertNode(SyntaxTokenType.BlockStatement);
                    e.AssertToken(SyntaxTokenType.OpenBracketToken, "{");
                    e.AssertToken(SyntaxTokenType.ClosedBracketToken, "}");
    }
}