using System;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis.Binding;

public class UnaryBoundOperator
{
    private SyntaxTokenType SyntaxTokenType { get; }
    public UnaryOperatorType BoundType { get; }
    public Type Result { get; }

    public UnaryBoundOperator(SyntaxTokenType type, UnaryOperatorType boundtype, Type result)
    {
        SyntaxTokenType = type;
        BoundType = boundtype;
        Result = result;
    }
    
    private static readonly UnaryBoundOperator[] _operations =
    {
        new(SyntaxTokenType.MinusToken, UnaryOperatorType.Negation, typeof(int)),
        new(SyntaxTokenType.MinusToken, UnaryOperatorType.Negation, typeof(float)),
    };
    
    public static UnaryBoundOperator? GetOp(SyntaxTokenType kind, Type operandtype)
    {
        foreach (UnaryBoundOperator op in _operations)
        {
            if (op.SyntaxTokenType == kind && op.Result == operandtype)
            {
                return op;
            }
        }
        return null;
    }
}