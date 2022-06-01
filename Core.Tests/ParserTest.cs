﻿using System;
using System.Collections;
using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser;
using Core.Compiler.CodeAnalysis.Parser.Expressions;
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

    private static ExpressionSyntax ParseExpression(string text)
    {
        SyntaxTree tree = SyntaxTree.Parse(text);
        ExpressionSyntax root = tree.Root;
        return root;
    }

    [Theory]
    [MemberData(nameof(BinaryOpData))]
    public static void Parser_Honors_Precedence(SyntaxTokenType type1, SyntaxTokenType type2)
    {
        int typeprecedence1 = SyntaxInfo.GetBinaryPrecedence(type1);
        int typeprecedence2 = SyntaxInfo.GetBinaryPrecedence(type2);
        string? typetext1 = SyntaxInfo.GetText(type1);
        string? typetext2 = SyntaxInfo.GetText(type2);

        var text = $"1 {typetext1} 2 {typetext2} 3";
        ExpressionSyntax expression = ParseExpression(text);
        
        // What the tree should look like depending on precedence
        using var e = new AssertingNumerator(expression);
        if (typeprecedence2 > typeprecedence1)
        {
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
        }
        else
        {
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
        }
    }
}