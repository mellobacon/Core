using System;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding;

public class UnaryBoundOperator
{
    private SyntaxTokenType SyntaxTokenType { get; }
    public UnaryOperatorType BoundType { get; }
    public TypeSymbol Result { get; }

    public UnaryBoundOperator(SyntaxTokenType type, UnaryOperatorType boundtype, TypeSymbol result)
    {
        SyntaxTokenType = type;
        BoundType = boundtype;
        Result = result;
    }
    
    private static readonly UnaryBoundOperator[] _operations =
    {
        new(SyntaxTokenType.MinusToken, UnaryOperatorType.Negation, TypeSymbol.Int),
        new(SyntaxTokenType.MinusToken, UnaryOperatorType.Negation, TypeSymbol.Float),
        new(SyntaxTokenType.BangToken, UnaryOperatorType.BooleanNegation, TypeSymbol.Bool),
    };
    
    public static UnaryBoundOperator? GetOp(SyntaxTokenType kind, TypeSymbol operandtype)
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