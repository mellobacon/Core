using Core.Compiler.CodeAnalysis.Binding;
using Core.Compiler.CodeAnalysis.Binding.Expressions;
using Core.Compiler.CodeAnalysis.Errors;
using Core.Compiler.CodeAnalysis.Parser;

namespace Core.Compiler.CodeAnalysis.Evaluator;
public class Compilation
{
    private object? _value;
    private readonly SyntaxTree _tree;
    public Compilation(SyntaxTree tree)
    {
        _tree = tree;
    }
    
    public Result Evaluate()
    {
        var binder = new Binder();
        IBoundExpression expression = binder.BindExpression(_tree.Root);

        _tree.Errors.Concat(binder.Errors);
        ErrorList errors = _tree.Errors;
        var evaluator = new Evaluator(expression);
        _value = evaluator.Evaluate();

        return errors.Any() ? new Result(errors, null) : new Result(errors, _value);
    }
}