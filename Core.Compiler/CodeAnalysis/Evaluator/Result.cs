using Core.Compiler.CodeAnalysis.Errors;

namespace Core.Compiler.CodeAnalysis.Evaluator;
public class Result
{
    public readonly ErrorList Errors;
    public readonly object? Value;
    public Result(ErrorList errors, object? value)
    {
        Errors = errors;
        Value = value;
    }
}