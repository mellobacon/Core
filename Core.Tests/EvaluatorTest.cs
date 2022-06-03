using System.Collections.Generic;
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
            ("1", 1),
            ("-2", -2),
            ("1.5", 1.5f),
            ("0.6 + 0.1", 0.70000005f), // smh float point
            ("1 + 0.5", 1.5f),
            ("5 - 2", 3),
            ("5.9 - 2.3", 3.6000001f),
            ("5.0 * 5.0", 25f),
            ("10.0 / 5.0", 2f),
            ("6 % 2", 0),
            ("6.2 % 3.2", 2.9999998f),
            ("2^2", 4d),
            ("0.5^2", 0.25),
            ("1_000", 1000),
            ("1_000 * 5", 5000),
            ("1 + 2 + 3", 6),
            ("1 + 2 * 3", 7),
            ("(1 + 2) * 3", 9),
            ("1 / 2", 0.5f),
            ("1 % 2", 1),
            ("1 < 2", true),
            ("9 > 1", true),
            ("1.5 > 3", false),
            ("0.6 < 0.7", true),
            ("5 >= 5", true),
            ("5 <= 4", false),
            ("5.5 >= 5.2", true),
            ("5.0 <= 4.9", false),
            ("5 <= 5", true),
            ("25 >= 63", false),
            ("(1 + 2) < 5", true),
            ("(1 + 3) * 5 > 2", true),
            ("1 == 2", false),
            ("(5 * 10) / 16 == 3.125", true),
            ("1 == 1", true),
            ("False == False", true),
            ("False", false),
            ("True", true),
            ("False || True", true),
            ("True && True", true),
            ("True && False", false),
            ("\"string\"", "string"),
            ("x = 5", 5)
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
            ("2.2.2", "Heehoo invalid number: Cannot convert 2.2.2 to System.Single"),
            ("$", "Heehoo bad character: $ is not a valid character"),
            ("2||2", "Heehoo bad binary operator || cant be applied to System.Int32 and System.Int32"),
            ("100_", "Heehoo invalid token 100_"),
            ("-False", "Heehoo bad unary operator - cant be applied to System.Boolean")
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

    private static Result GetResult(string text)
    {
        SyntaxTree tree = SyntaxTree.Parse(text);
        var compilation = new Compilation(tree);
        Result result = compilation.Evaluate();
        return result;
    }
}