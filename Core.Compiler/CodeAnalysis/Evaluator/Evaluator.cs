using System;
using Core.Compiler.CodeAnalysis.Binding;
using Core.Compiler.CodeAnalysis.Binding.Expressions;
using Core.Compiler.CodeAnalysis.Binding.Statements;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Evaluator;
public class Evaluator
{
    private readonly IBoundStatement _root;
    private object? _value;
    public Evaluator(IBoundStatement root)
    {
        _root = root;
    }

    public object? Evaluate()
    {
        EvaluateStatement(_root);
        return _value;
    }

    private void EvaluateStatement(IBoundStatement root)
    {
        switch (root)
        {
            case BlockBoundStatement b:
                foreach (IBoundStatement statement in b.Statements)
                {
                    EvaluateStatement(statement);
                }
                break;
            case IfBoundStatement f:
                if ((bool)(EvaluateExpression(f.Condition) ?? false))
                {
                    EvaluateStatement(f.Statement);
                }
                else if (f.ElseStatement != null)
                {
                    EvaluateStatement(f.ElseStatement);
                }
                break;
            case WhileBoundStatement w:
                while ((bool)(EvaluateExpression(w.Condition) ?? false))
                {
                    EvaluateStatement(w.Statement);
                }
                break;
            case ForBoundStatement i:
                EvaluateStatement(i.Init);
                while ((bool)(EvaluateExpression(i.Condition) ?? false))
                {
                    EvaluateStatement(i.Statement);
                    EvaluateExpression(i.Iter);
                }
                break;
            case ExpressionBoundStatement e:
                _value = EvaluateExpression(e.Expression);
                break;
            case VariableBoundStatement v:
                object? value = EvaluateExpression(v.Expression);
                Variables.SetVariable(v.Variable, value);
                _value = value;
                break;
            default:
                throw new Exception($"Unexpected node {root.BoundType}");
        }
    }

    private object? EvaluateExpression(IBoundExpression root)
    {
        return root.BoundType switch
        {
            BoundType.UnaryExpression => EvaluateUnaryExpression(root),
            BoundType.BinaryExpression => EvaluateBinaryExpression(root),
            BoundType.LiteralExpression => EvaluateLiteralExpression(root),
            BoundType.AssignmentExpression => EvaluateAssignmentExpression(root),
            BoundType.VariableExpression => EvaluateVariableExpression(root),
            BoundType.MethodExpression => EvaluateMethod(root),
            _ => null
        };
    }

    private object? EvaluateMethod(IBoundExpression root)
    {
        if (root is not MethodBoundExpression m) return null;

        if (m.Function == Functions.Print)
        {
            foreach (IBoundExpression arg in m.Args)
            {
                object message = EvaluateExpression(arg)!;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(message);
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        else if (m.Function == Functions.PrintLn)
        {
            foreach (IBoundExpression arg in m.Args)
            {
                object message = EvaluateExpression(arg)!;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
        return null;
    }

    private object? EvaluateAssignmentExpression(IBoundExpression root)
    {
        if (root is not AssignmentBoundExpression a) return null;
        object? value = EvaluateExpression(a.Expression);

        if (a.HasCompoundOp)
        {
            // Ok...*listen*...i can explain.....
            // dynamic works ok xd. this is jank. but it works
            dynamic variableValue = 0;
            dynamic temp = 0;
            if (value is float)
            {
                temp = Convert.ToSingle(value);
                variableValue = Convert.ToSingle(Variables.GetVariableValue(a.Variable.Name) ?? 0);
            }
            else if (value is int)
            {
                temp = (int)value;
                variableValue = (int)(Variables.GetVariableValue(a.Variable.Name) ?? 0);
            }
            
            switch (a.Operator.Type)
            {
                case SyntaxTokenType.PlusToken:
                    value = variableValue + temp;
                    Variables.SetVariable(a.Variable, value);
                    return value;
                case SyntaxTokenType.MinusToken:
                    value = variableValue - temp;
                    Variables.SetVariable(a.Variable, value);
                    return value;
                case SyntaxTokenType.SlashToken:
                    value = variableValue / temp;
                    Variables.SetVariable(a.Variable, value);
                    return value;
                case SyntaxTokenType.StarToken:
                    value = variableValue * temp;
                    Variables.SetVariable(a.Variable, value);
                    return value;
                case SyntaxTokenType.ModuloToken:
                    value = variableValue % temp;
                    Variables.SetVariable(a.Variable, value);
                    return value;
            }
        }
        
        Variables.SetVariable(a.Variable, value);

        return value;
    }

    private object? EvaluateBinaryExpression(IBoundExpression root)
    {
        if (root is not BinaryBoundExpression b) return null;
        
        object? left = EvaluateExpression(b.Left);
        BoundBinaryOperator op = b.Op;
        object? right = EvaluateExpression(b.Right);

        if (left == null || right == null) return null;

        // If one of the numbers is a float convert both numbers to floats else make sure both are ints
        // (except division)
        if (left is float || right is float)
        {
            return op.BoundType switch
            {
                BinaryOperatorType.Addition => Convert.ToSingle(left) + Convert.ToSingle(right),
                BinaryOperatorType.Subtraction => Convert.ToSingle(left) - Convert.ToSingle(right),
                BinaryOperatorType.Multiplication => Convert.ToSingle(left) * Convert.ToSingle(right),
                BinaryOperatorType.Division => Convert.ToSingle(left) / Convert.ToSingle(right),
                BinaryOperatorType.Modulo => Convert.ToSingle(left) % Convert.ToSingle(right),
                BinaryOperatorType.LessThan => Convert.ToSingle(left) < Convert.ToSingle(right),
                BinaryOperatorType.MoreThan => Convert.ToSingle(left) > Convert.ToSingle(right),
                BinaryOperatorType.LessEqual => Convert.ToSingle(left) <= Convert.ToSingle(right),
                BinaryOperatorType.MoreEqual => Convert.ToSingle(left) >= Convert.ToSingle(right),
                BinaryOperatorType.Equal => Equals(left, right),
                BinaryOperatorType.Exponent => Math.Pow(Convert.ToSingle(left), Convert.ToSingle(right)),
                _ => throw new Exception($"Unexpected binary operator {b.Op} (Evaluator)")
            };
        }

        return op.BoundType switch
        {
            BinaryOperatorType.Addition => (int)left + (int)right,
            BinaryOperatorType.Subtraction => (int)left - (int)right,
            BinaryOperatorType.Multiplication => (int)left * (int)right,
            BinaryOperatorType.Division => Convert.ToSingle(left) / Convert.ToSingle(right),
            BinaryOperatorType.Modulo => (int)left % (int)right,
            BinaryOperatorType.LogicalOr => (bool)left || (bool)right,
            BinaryOperatorType.LogicalAnd => (bool)left && (bool)right,
            BinaryOperatorType.LessThan => (int)left < (int)right,
            BinaryOperatorType.MoreThan => (int)left > (int)right,
            BinaryOperatorType.LessEqual => (int)left <= (int)right,
            BinaryOperatorType.MoreEqual => (int)left >= (int)right,
            BinaryOperatorType.Equal => Equals(left, right),
            BinaryOperatorType.NotEqual => !Equals(left, right),
            BinaryOperatorType.Exponent => Math.Pow((int)left, (int)right),
            _ => throw new Exception($"Unexpected binary operator {b.Op} (Evaluator)")
        };
    }

    private object? EvaluateUnaryExpression(IBoundExpression root)
    {
        if (root is not UnaryBoundExpression u) return null;

        object? operand = EvaluateExpression(u.Operand);
        if (operand is null) return null;

        return u.Op.BoundType switch
        {
            UnaryOperatorType.Negation => -(int)operand,
            UnaryOperatorType.BooleanNegation => !(bool)operand,
            _ => throw new Exception($"Unexpected unary operator {u.Op}")
        };
    }

    private object? EvaluateVariableExpression(IBoundExpression root)
    {
        if (root is not VariableBoundExpression v) return null;
        return Variables.GetVariableValue(v.Variable.Name);
    }

    private object? EvaluateLiteralExpression(IBoundExpression root)
    {
        return root is not LiteralBoundExpression n ? null : n.Value;
    }
}