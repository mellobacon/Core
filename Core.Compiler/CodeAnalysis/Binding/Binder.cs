using System;
using System.Collections.Immutable;
using Core.Compiler.CodeAnalysis.Binding.Expressions;
using Core.Compiler.CodeAnalysis.Binding.Statements;
using Core.Compiler.CodeAnalysis.Errors;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Expressions;
using Core.Compiler.CodeAnalysis.Parser.Statements;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding;
public class Binder
{
    public ErrorList Errors { get; } = new();

    public IBoundStatement BindStatement(StatementSyntax syntax)
    {
        return syntax.Type switch
        {
            SyntaxTokenType.BlockStatement => BindBlockStatement((BlockStatement)syntax),
            SyntaxTokenType.IfStatement => BindIfStatement((IfStatement)syntax),
            SyntaxTokenType.WhileStatement => BindWhileStatement((WhileStatement)syntax),
            SyntaxTokenType.ForStatement => BindForStatement((ForStatement)syntax),
            SyntaxTokenType.VariableStatement => BindVariableStatement((VariableStatement)syntax),
            SyntaxTokenType.ExpressionStatement => BindExpressionStatement((ExpressionStatement)syntax),
            _ => throw new Exception($"Unexpected statement syntax [{syntax.Type}] (Binder)")
        };
    }

    private IBoundStatement BindBlockStatement(BlockStatement syntax)
    {
        ImmutableArray<IBoundStatement>.Builder statements = ImmutableArray.CreateBuilder<IBoundStatement>();
        foreach (StatementSyntax statement in syntax.Statements)
        {
            IBoundStatement s = BindStatement(statement);
            statements.Add(s);
        }

        return new BlockBoundStatement(statements.ToImmutable());
    }

    private IBoundStatement BindIfStatement(IfStatement syntax)
    {
        IBoundExpression condition = BindExpression(syntax.Condition);
        IBoundStatement statement = BindStatement(syntax.Thenstatement);
        IBoundStatement? elsestatement = syntax.Elsestatement == null ? null : BindStatement(syntax.Elsestatement.Statement);
        return new IfBoundStatement(condition, statement, elsestatement);
    }

    private IBoundStatement BindWhileStatement(WhileStatement syntax)
    {
        IBoundExpression condition = BindExpression(syntax.Condition);
        IBoundStatement statement = BindStatement(syntax.Dostatement);
        return new WhileBoundStatement(condition, statement);
    }

    private IBoundStatement BindForStatement(ForStatement syntax)
    {
        IBoundStatement init = BindStatement(syntax.Initializer);
        IBoundExpression condition = BindExpression(syntax.Condition);
        IBoundExpression iterator = BindExpression(syntax.Iterator);
        IBoundStatement statement = BindStatement(syntax.Dostatement);
        return new ForBoundStatement(init, condition, iterator, statement);
    }
    
    private IBoundStatement BindVariableStatement(VariableStatement syntax)
    {
        string name = syntax.Variable.Text ?? "no";
        IBoundExpression expression = BindExpression(syntax.Expression);
        var variable = new VariableSymbol(name, expression.Type);
        Variables.AddVariable(variable);
        return new VariableBoundStatement(variable, expression);
    }
    
    private IBoundStatement BindExpressionStatement(ExpressionStatement syntax)
    {
        IBoundExpression expression = BindExpression(syntax.Expression);
        return new ExpressionBoundStatement(expression);
    }
    
    private IBoundExpression BindExpression(ExpressionSyntax syntax)
    {
        return syntax.Type switch
        {
            SyntaxTokenType.LiteralExpression => BindLiteralExpression((LiteralExpression)syntax),
            SyntaxTokenType.UnaryExpression => BindUnaryExpression((UnaryExpression)syntax),
            SyntaxTokenType.BinaryExpression => BindBinaryExpression((BinaryExpression)syntax),
            SyntaxTokenType.GroupedExpression => BindGroupedExpression((GroupedExpression)syntax),
            SyntaxTokenType.AssignmentExpression => BindAssignmentExpression((AssignmentExpression)syntax),
            SyntaxTokenType.VariableExpression => BindVariableExpression((VariableExpression)syntax),
            SyntaxTokenType.FunctionCallExpression => BindMethodExpression((FunctionCallExpression)syntax),
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
        var variable = new VariableSymbol(name, expression.Type);
        if (Variables.GetVariable(name) is null)
        {
            Errors.ReportVariableNoneExistent(syntax.VariableToken.TextSpan, name);
            return new LiteralBoundExpression(0);
        }

        return new AssignmentBoundExpression(variable, expression, syntax.Operator, syntax.IsCompoundOp);
    }

    private IBoundExpression BindVariableExpression(VariableExpression syntax)
    {
        string? name = syntax.VariableToken.Text ?? "no";
        VariableSymbol? variable = Variables.GetVariable(name);

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

    private IBoundExpression BindMethodExpression(FunctionCallExpression syntax)
    {
        IBoundExpression arg = BindExpression(syntax.Arg);
        FunctionSymbol function = Functions.Print;
        return new MethodBoundExpression(function, arg);
    }
}