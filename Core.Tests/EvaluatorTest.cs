﻿using System.Collections.Generic;
using System.Linq;
using Core.Compiler.CodeAnalysis.Evaluator;
using Core.Compiler.CodeAnalysis.Parser;
using Xunit;

namespace Core.Tests;
public static class EvaluatorTest
{
    private static IEnumerable<object[]> GetEvaluations()
    {
        var evals = new (string text, object value)[]
        {
            ("1;", 1),
            ("-2;", -2),
            ("1.5;", 1.5f),
            ("0.6 + 0.1;", 0.70000005f), // smh float point
            ("1 + 0.5;", 1.5f),
            ("5 - 2;", 3),
            ("5.9 - 2.3;", 3.6000001f),
            ("5.0 * 5.0;", 25f),
            ("10.0 / 5.0;", 2f),
            ("6 % 2;", 0),
            ("6.2 % 3.2;", 2.9999998f),
            ("2^2;", 4d),
            ("0.5^2;", 0.25),
            ("1_000;", 1000),
            ("1_000 * 5;", 5000),
            ("1 + 2 + 3;", 6),
            ("1 + 2 * 3;", 7),
            ("(1 + 2) * 3;", 9),
            ("1 / 2;", 0.5f),
            ("1 % 2;", 1),
            ("1 < 2;", true),
            ("9 > 1;", true),
            ("1.5 > 3;", false),
            ("0.6 < 0.7;", true),
            ("5 >= 5;", true),
            ("5 <= 4;", false),
            ("5.5 >= 5.2;", true),
            ("5.0 <= 4.9;", false),
            ("5 <= 5;", true),
            ("25 >= 63;", false),
            ("(1 + 2) < 5;", true),
            ("(1 + 3) * 5 > 2;", true),
            ("1 == 2;", false),
            ("(5 * 10) / 16 == 3.125;", true),
            ("1 == 1;", true),
            ("False == False;", true),
            ("5 != 6;", true),
            ("False != False;", false),
            ("False;", false),
            ("True;", true),
            ("!True;", false),
            ("False || True;", true),
            ("True && True;", true),
            ("True && False;", false),
            ("\"string\";", "string"),
            ("let int x = 5;", 5),
        };
        foreach ((string text, object value) in evals)
        {
            yield return new[] { text, value };
        }
    }
    [Theory]
    [MemberData(nameof(GetEvaluations))]
    public static void Evaluator_Outputs_Correct_Value(string text, object value)
    {
        Result result = GetResult(text);
        Assert.Equal(value, result.Value);
    }

    private static IEnumerable<object[]> GetWrongEvaluations()
    {
        var evals = new (string text, string error)[]
        {
            ("2.2.2;", "Error - invalid number: Cannot convert 2.2.2 to float"),
            ("$;", "Error - bad character: $ is not a valid character"),
            ("2||2;", "Error - bad binary operator || cant be applied to int and int"),
            ("100_", "Error - invalid token 100_"),
            ("-False;", "Error - bad unary operator - cant be applied to bool"),
            ("!100;", "Error - bad unary operator ! cant be applied to int"),
            ("let dynamic x = 10;", "Error - dynamic is an invalid type"),
            ("let string x = 10;", "Error - Cannot convert int to string")
        };
        foreach ((string text, string error) in evals)
        {
            yield return new object[] { text, error };
        }
    }

    [Theory]
    [MemberData(nameof(GetWrongEvaluations))]
    public static void Evaluator_Outputs_Correct_Error_Message(string text, string error)
    {
        Result result = GetResult(text);
        string errormessage = result.Errors.ToArray().First().Message;
        Assert.Equal(errormessage, error);
    }

    [Fact]
    public static void Evaluator_Evaluates_IfElseStatement()
    {
        const string input = @"{if (2 > 5) { ""2""; } else { ""3""; }}";
        Result result = GetResult(input);
        Assert.Equal("3", result.Value);
    }
    
    [Fact]
    public static void Evaluator_Evaluates_WhileStatement()
    {
        const string input = @"{let int x = 0; while (x < 3) { x += 1; }}";
        Result result = GetResult(input);
        Assert.Equal(3, result.Value);
    }
    
    [Fact]
    public static void Evaluator_Evaluates_ForStatement()
    {
        const string input = @"{for (let int x = 0; x < 3; x += 1) { print(x); }}";
        Result result = GetResult(input);
        Assert.Null(result.Value);
    }

    private static Result GetResult(string text)
    {
        SyntaxTree tree = SyntaxTree.Parse(text);
        var compilation = new Compilation(null, tree);
        Result result = compilation.Evaluate();
        return result;
    }
}