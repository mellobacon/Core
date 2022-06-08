using System;
using Core.Compiler.CodeAnalysis.Binding.Expressions;
using Core.Compiler.CodeAnalysis.Errors;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Expressions;

namespace Core.Compiler.CodeAnalysis.Binding;
public class Binder
{
    public ErrorList Errors { get; } = new();

    public IBoundExpression BindExpression(ExpressionSyntax syntax)
    {
        return syntax.Type switch
        {
            SyntaxTokenType.LiteralExpression => BindLiteralExpression((LiteralExpression)syntax),
            SyntaxTokenType.UnaryExpression => BindUnaryExpression((UnaryExpression)syntax),
            SyntaxTokenType.BinaryExpression => BindBinaryExpression((BinaryExpression)syntax),
            SyntaxTokenType.GroupedExpression => BindGroupedExpression((GroupedExpression)syntax),
            SyntaxTokenType.AssignmentExpression => BindAssignmentExpression((AssignmentExpression)syntax),
            SyntaxTokenType.VariableExpression => BindVariableExpression((VariableExpression)syntax),
            _ => throw new Exception($"Unexpected syntax [{syntax.Type}] (Binder)")
        };
    }

    private static IBoundExpression BindLiteralExpression(LiteralExpression syntax)
    {
        object value = syntax.Value ?? 0;
        return new LiteralBoundExpression(value);
    }

    private IBoundExpression BindBinaryExpression(BinaryExpression syntax)
    {
        IBoundExpression left = BindExpression(syntax.Left);
        IBoundExpression right = BindExpression(syntax.Right);
        BoundBinaryOperator? op = BoundBinaryOperator.GetOp(left.Type, syntax.Op.Type, right.Type);
        if (op is null)
        {
            Errors.ReportUndefinedBinaryOperator(syntax.Op.TextSpan ,left.Type, syntax.Op.Text, right.Type);
            return left;
        }
        return new BinaryBoundExpression(left, op, right);
    }

    private IBoundExpression BindUnaryExpression(UnaryExpression syntax)
    {
        IBoundExpression operand = BindExpression(syntax.Expression);
        UnaryBoundOperator? op = UnaryBoundOperator.GetOp(syntax.Op.Type, operand.Type);
        if (op is null)
        {
            Errors.ReportUndefinedUnaryOperator(syntax.Op.TextSpan, syntax.Op.Text, operand.Type);
            return operand;
        }

        return new UnaryBoundExpression(op, operand);
    }

    private IBoundExpression BindAssignmentExpression(AssignmentExpression syntax)
    {
        string name = syntax.VariableToken.Text!;
        IBoundExpression expression = BindExpression(syntax.Expression);
        var variable = new Variable(name, expression.Type);
        if (Variables.GetVariable(name) is null)
        {
            Variables.AddVariable(variable);
        }
        
        

        return new AssignmentBoundExpression(variable, expression, syntax.Operator, syntax.IsCompoundOp);
    }

    private IBoundExpression BindVariableExpression(VariableExpression syntax)
    {
        string? name = syntax.VariableToken.Text;
        Variable? variable = Variables.GetVariable(name);

        if (variable is null)
        {
            Errors.ReportVariableNoneExistent(syntax.VariableToken.TextSpan, name);
            return new LiteralBoundExpression(0);
        }

        return new VariableBoundExpression(variable);
    }

    private IBoundExpression BindGroupedExpression(GroupedExpression syntax)
    {
        return BindExpression(syntax.Expression);
    }
}