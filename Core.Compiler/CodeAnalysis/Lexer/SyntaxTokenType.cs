namespace Core.Compiler.CodeAnalysis.Lexer;
public enum SyntaxTokenType
{
    NumberToken,
    StringToken,
    VariableToken,
        
    PlusToken,
    MinusToken,
    SlashToken,
    StarToken,
    ModuloToken,
    HatToken,
    LessThanToken,
    MoreThanToken,
    
    LessEqualsToken,
    MoreEqualsToken,
    EqualsEqualsToken,
    NotEqualsToken,
    
    PlusEqualsToken,
    MinusEqualsToken,
    SlashEqualsToken,
    StarEqualsToken,
    ModuloEqualsToken,
    
    OpenParenToken,
    ClosedParenToken,
    OpenBracketToken,
    ClosedBracketToken,

    DoublePipeToken,
    DoubleAmpersandToken,
    BangToken,
        
    EqualsToken,
        
    TrueKeyword,
    FalseKeyword,
    VariableKeyword,
    IfKeyword,
    ElseKeyword,
    WhileKeyword,

    BlockStatement,
    ExpressionStatement,
    VariableStatement,
    IfStatement,
    ElseStatement,
    WhileStatement,
    
    BinaryExpression,
    UnaryExpression,
    LiteralExpression,
    GroupedExpression,
    AssignmentExpression,
    VariableExpression,

    WhiteSpaceToken,
    SemicolonToken,
    BadToken,
    EofToken
}