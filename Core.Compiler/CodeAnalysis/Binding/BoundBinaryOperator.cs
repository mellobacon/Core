using System;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding;
public class BoundBinaryOperator
{
    public BinaryOperatorType BoundType { get; }
    private SyntaxTokenType SyntaxTokenType { get; }
    private TypeSymbol LeftType { get; }
    private TypeSymbol RightType { get; }
    public TypeSymbol ResultType { get; }

    private BoundBinaryOperator(BinaryOperatorType boundType, SyntaxTokenType syntaxTokenType, TypeSymbol leftType, TypeSymbol rightType, TypeSymbol resultType)
    {
        BoundType = boundType;
        SyntaxTokenType = syntaxTokenType;
        LeftType = leftType;
        RightType = rightType;
        ResultType = resultType;
    }
    
    private BoundBinaryOperator(BinaryOperatorType boundType, SyntaxTokenType syntaxTokenType, TypeSymbol type, TypeSymbol resultType) 
        : this(boundType, syntaxTokenType, type, type, resultType) {}

    private BoundBinaryOperator(BinaryOperatorType boundType, SyntaxTokenType syntaxTokenType, TypeSymbol type) : this(
        boundType, syntaxTokenType, type, type, type) {}

    // defines what counts as a valid binary expression...i think
    private static readonly BoundBinaryOperator[] _operations =
    {
        new(BinaryOperatorType.Addition, SyntaxTokenType.PlusToken, TypeSymbol.Int),
        new(BinaryOperatorType.Addition, SyntaxTokenType.PlusToken, TypeSymbol.Float),
        new(BinaryOperatorType.Addition, SyntaxTokenType.PlusToken, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Float),
        new(BinaryOperatorType.Addition, SyntaxTokenType.PlusToken, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Float),
        
        new(BinaryOperatorType.Subtraction, SyntaxTokenType.MinusToken, TypeSymbol.Int),
        new(BinaryOperatorType.Subtraction, SyntaxTokenType.MinusToken, TypeSymbol.Float),
        new(BinaryOperatorType.Subtraction, SyntaxTokenType.MinusToken, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Float),
        new(BinaryOperatorType.Subtraction, SyntaxTokenType.MinusToken, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Float),
        
        new(BinaryOperatorType.Multiplication, SyntaxTokenType.StarToken, TypeSymbol.Int),
        new(BinaryOperatorType.Multiplication, SyntaxTokenType.StarToken, TypeSymbol.Float),
        new(BinaryOperatorType.Multiplication, SyntaxTokenType.StarToken, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Float),
        new(BinaryOperatorType.Multiplication, SyntaxTokenType.StarToken, TypeSymbol.Float, TypeSymbol.Int,TypeSymbol.Float),
        
        new(BinaryOperatorType.Division, SyntaxTokenType.SlashToken, TypeSymbol.Int),
        new(BinaryOperatorType.Division, SyntaxTokenType.SlashToken, TypeSymbol.Float),
        new(BinaryOperatorType.Division, SyntaxTokenType.SlashToken, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Float),
        new(BinaryOperatorType.Division, SyntaxTokenType.SlashToken, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Float),
        
        new(BinaryOperatorType.Modulo, SyntaxTokenType.ModuloToken, TypeSymbol.Int),
        new(BinaryOperatorType.Modulo, SyntaxTokenType.ModuloToken, TypeSymbol.Float),
        
        new(BinaryOperatorType.Exponent, SyntaxTokenType.HatToken, TypeSymbol.Int, TypeSymbol.Double),
        new(BinaryOperatorType.Exponent, SyntaxTokenType.HatToken, TypeSymbol.Float, TypeSymbol.Double),
        new(BinaryOperatorType.Exponent, SyntaxTokenType.HatToken, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Double),
        new(BinaryOperatorType.Exponent, SyntaxTokenType.HatToken, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Double),
        
        new(BinaryOperatorType.LessThan, SyntaxTokenType.LessThanToken, TypeSymbol.Int, TypeSymbol.Bool),
        new(BinaryOperatorType.LessThan, SyntaxTokenType.LessThanToken, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.LessThan, SyntaxTokenType.LessThanToken, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.LessThan, SyntaxTokenType.LessThanToken, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Bool),
        
        new(BinaryOperatorType.MoreThan, SyntaxTokenType.MoreThanToken, TypeSymbol.Int, TypeSymbol.Bool),
        new(BinaryOperatorType.MoreThan, SyntaxTokenType.MoreThanToken, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.MoreThan, SyntaxTokenType.MoreThanToken, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.MoreThan, SyntaxTokenType.MoreThanToken, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Bool),

        new(BinaryOperatorType.LessEqual, SyntaxTokenType.LessEqualsToken, TypeSymbol.Int, TypeSymbol.Bool),
        new(BinaryOperatorType.LessEqual, SyntaxTokenType.LessEqualsToken, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.LessEqual, SyntaxTokenType.LessEqualsToken, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.LessEqual, SyntaxTokenType.LessEqualsToken, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Bool),
        
        new(BinaryOperatorType.MoreEqual, SyntaxTokenType.MoreEqualsToken, TypeSymbol.Int, TypeSymbol.Bool),
        new(BinaryOperatorType.MoreEqual, SyntaxTokenType.MoreEqualsToken, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.MoreEqual, SyntaxTokenType.MoreEqualsToken, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.MoreEqual, SyntaxTokenType.MoreEqualsToken, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Bool),
        
        new(BinaryOperatorType.Equal, SyntaxTokenType.EqualsEqualsToken, TypeSymbol.Int, TypeSymbol.Bool),
        new(BinaryOperatorType.Equal, SyntaxTokenType.EqualsEqualsToken, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.Equal, SyntaxTokenType.EqualsEqualsToken, TypeSymbol.Bool),
        new(BinaryOperatorType.Equal, SyntaxTokenType.EqualsEqualsToken, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.Equal, SyntaxTokenType.EqualsEqualsToken, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Bool),
        
        new(BinaryOperatorType.NotEqual, SyntaxTokenType.NotEqualsToken, TypeSymbol.Int, TypeSymbol.Bool),
        new(BinaryOperatorType.NotEqual, SyntaxTokenType.NotEqualsToken, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.NotEqual, SyntaxTokenType.NotEqualsToken, TypeSymbol.Bool),
        new(BinaryOperatorType.NotEqual, SyntaxTokenType.NotEqualsToken, TypeSymbol.Int, TypeSymbol.Float, TypeSymbol.Bool),
        new(BinaryOperatorType.NotEqual, SyntaxTokenType.NotEqualsToken, TypeSymbol.Float, TypeSymbol.Int, TypeSymbol.Bool),
        
        new(BinaryOperatorType.LogicalAnd, SyntaxTokenType.DoubleAmpersandToken, TypeSymbol.Bool),
        new(BinaryOperatorType.LogicalOr, SyntaxTokenType.DoublePipeToken, TypeSymbol.Bool)
    };

    // Get operator based on types supplied
    public static BoundBinaryOperator? GetOp(TypeSymbol left, SyntaxTokenType type, TypeSymbol right)
    {
        foreach (BoundBinaryOperator op in _operations)
        {
            if (op.LeftType == left && op.SyntaxTokenType == type && op.RightType == right)
            {
                return op;
            }
        }

        return null;
    }
}