using System.Collections.Generic;
using System.Linq;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Errors;
// TODO: Make the error messages less heehoo
public class ErrorList
{
    private readonly List<Error> _errors = new();
        
    public void Concat(ErrorList errors)
    {
        _errors.AddRange(errors._errors);
    }

    public bool Any()
    {
        return _errors.ToArray().Any();
    }

    public IEnumerable<Error> ToArray()
    {
        return _errors.ToArray();
    }
    public void ReportInvalidNumberConversion(TextSpan span, string num, TypeSymbol type)
    {
        string message = $"Error - invalid number: Cannot convert {num} to {type}";
        _errors.Add(new Error(span, message));
    }

    public void ReportBadCharacter(char character, int position)
    {
        TextSpan span = new(position, 1);
        string message = $"Error - bad character: {character} is not a valid character";
        _errors.Add(new Error(span, message));
    }

    public void ReportUnExpectedToken(TextSpan span, string? token, SyntaxTokenType result, SyntaxTokenType expected)
    {
        string message = $"Error - unexpected token <{token}>: got {result} not {expected}";
        _errors.Add(new Error(span, message));
    }

    public void ReportUndefinedBinaryOperator(TextSpan span, TypeSymbol left, string? op, TypeSymbol right)
    {
        string message = $"Error - bad binary operator {op} cant be applied to {left} and {right}";
        _errors.Add(new Error(span, message));
    }
    
    public void ReportUndefinedUnaryOperator(TextSpan span, string? op, TypeSymbol operand)
    {
        string message = $"Error - bad unary operator {op} cant be applied to {operand}";
        _errors.Add(new Error(span, message));
    }

    public void ReportInvalidToken(TextSpan span, string token)
    {
        var message = $"Error - invalid token {token}";
        _errors.Add(new Error(span, message));
    }

    public void ReportVariableNonExistent(TextSpan span, string token)
    {
        var message = $"Error - the variable '{token}' does not exist";
        _errors.Add(new Error(span, message));
    }
}