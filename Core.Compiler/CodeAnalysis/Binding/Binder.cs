using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
    private Scope? _scope;
    private ErrorList Errors { get; } = new();

    private Binder(Scope? parent)
    {
        _scope = new Scope(parent);
    }
    public static GlobalScope BindScope(GlobalScope? scope, StatementSyntax syntax)
    {
        var scopeStack = new Stack<GlobalScope>();
        
        while (scope is not null)
        {
            scopeStack.Push(scope);
            scope = scope._GlobalScope;
        }

        Scope? currentScope = null;
        while (scopeStack.Count > 0)
        {
            scope = scopeStack.Pop();
            var localscope = new Scope(currentScope);
            foreach (VariableSymbol variable in scope.Variables.Keys)
            {
                localscope.AddVariable(variable);
            }
            currentScope = localscope;
        }

        var binder = new Binder(currentScope);
        IBoundStatement statement = binder.BindStatement(syntax);
        var variables = binder._scope!.GetVariables();
        return new GlobalScope(scope, statement, variables, binder.Errors);
    }

    private IBoundStatement BindStatement(StatementSyntax syntax)
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
        _scope = new Scope(_scope!);
        foreach (StatementSyntax statement in syntax.Statements)
        {
            statements.Add(BindStatement(statement));
        }
        _scope = _scope.ParentScope;

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
        string name = syntax.Variable.Text;
        TypeSymbol type = BindVarType(syntax.VarType);
        IBoundExpression expression = BindExpression(syntax.Expression);
        expression = TryConversion(syntax, expression, type);
        var variable = new VariableSymbol(name, type);
        if (!_scope!.AddVariable(variable))
        {
            Errors.ReportVariableAlreadyExists(syntax.Variable.TextSpan, name);
        }
        return new VariableBoundStatement(variable, expression);
    }

    private TypeSymbol BindVarType(SyntaxToken token)
    {
        return token.Text switch
        {
            "int" => TypeSymbol.Int,
            "float" => TypeSymbol.Float,
            "double" => TypeSymbol.Double,
            "string" => TypeSymbol.String,
            "bool" => TypeSymbol.Bool,
            _ => TypeSymbol.Error
        };
    }
    
    

    private IBoundStatement BindExpressionStatement(ExpressionStatement syntax)
    {
        IBoundExpression expression = BindExpression(syntax.Expression);
        return new ExpressionBoundStatement(expression);
    }

    private IBoundExpression TryConversion(VariableStatement syntax, IBoundExpression expression, TypeSymbol type)
    {
        if (expression.Type != type)
        {
            if (type == TypeSymbol.Error)
            {
                Errors.ReportInvalidType(syntax.VarType.TextSpan, syntax.VarType.Text);
                return new ErrorBoundExpression();
            }
            Errors.ReportTypeConversionError(syntax.VarType.TextSpan, expression.Type.Name, type);
            return new ErrorBoundExpression();
        }

        return expression;
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
        string name = syntax.VariableToken.Text;
        IBoundExpression expression = BindExpression(syntax.Expression);
        if (_scope!.GetVariable(name) is null)
        {
            Errors.ReportVariableNonExistent(syntax.VariableToken.TextSpan, name);
            return expression;
        }

        var variable = _scope.GetVariable(name);

        return new AssignmentBoundExpression(variable, expression, syntax.Operator, syntax.IsCompoundOp);
    }

    private IBoundExpression BindVariableExpression(VariableExpression syntax)
    {
        string name = syntax.VariableToken.Text;
        VariableSymbol? variable = _scope!.GetVariable(name);

        if (variable is null)
        {
            Errors.ReportVariableNonExistent(syntax.VariableToken.TextSpan, name);
            return new ErrorBoundExpression();
        }

        return new VariableBoundExpression(variable);
    }

    private IBoundExpression BindGroupedExpression(GroupedExpression syntax)
    {
        return BindExpression(syntax.Expression);
    }

    private IBoundExpression BindMethodExpression(FunctionCallExpression syntax)
    {
        ImmutableArray<IBoundExpression>.Builder args = ImmutableArray.CreateBuilder<IBoundExpression>();
        foreach (ExpressionSyntax arg in syntax.Args)
        {
            args.Add(BindExpression(arg));
        }

        FunctionSymbol? function = Functions.GetAll().First(f => f?.Name == syntax.Name.Text);
        if (function is null)
        {
            // return function doesnt exist error
            return new ErrorBoundExpression();
        }

        return new MethodBoundExpression(function, args.ToImmutable());
    }
}