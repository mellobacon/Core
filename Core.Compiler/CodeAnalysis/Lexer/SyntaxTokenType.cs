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
    ForKeyword,

    BlockStatement,
    ExpressionStatement,
    VariableStatement,
    IfStatement,
    ElseStatement,
    WhileStatement,
    ForStatement,

    BinaryExpression,
    UnaryExpression,
    LiteralExpression,
    GroupedExpression,
    AssignmentExpression,
    VariableExpression,
    FunctionCallExpression,
    FunctionDeclarationExpression,

    CommaToken,
    WhiteSpaceToken,
    SemicolonToken,
    BadToken,
    EofToken
}