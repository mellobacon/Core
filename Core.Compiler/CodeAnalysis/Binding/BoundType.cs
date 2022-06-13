namespace Core.Compiler.CodeAnalysis.Binding;
public enum BoundType
{
    BinaryExpression,
    UnaryExpression,
    LiteralExpression,
    AssignmentExpression,
    VariableExpression,
    MethodExpression,
    
    BlockStatement,
    ExpressionStatement,
    VariableStatement,
    IfStatement,
    WhileStatement,
    ForStatement,
}