﻿using System.Threading;
using Core.Compiler.CodeAnalysis.Binding;
using Core.Compiler.CodeAnalysis.Binding.Statements;
using Core.Compiler.CodeAnalysis.Errors;
using Core.Compiler.CodeAnalysis.Parser;

namespace Core.Compiler.CodeAnalysis.Evaluator;
public class Compilation
{
    private object? _value;
    private readonly SyntaxTree _tree;
    private readonly Compilation? _previousCompilation;

    public Compilation(Compilation? previousComp, SyntaxTree tree)
    {
        _previousCompilation = previousComp;
        _tree = tree;
    }

    // this is used to set any previous compilation
    public Compilation Continue(SyntaxTree tree)
    {
        return new Compilation(this, tree);
    }

    private GlobalScope? _globalScope;
    // get global scope
    private GlobalScope GlobalScope
    {
        get
        {
            if (_globalScope is null)
            {
                var binder = new Binder();
                GlobalScope globalscope = binder.BindScope(_previousCompilation?._globalScope, _tree.Root);
                // replace scope with other if theres two to prevent thread lock
                Interlocked.CompareExchange(ref _globalScope, globalscope, null);
            }

            return _globalScope;
        }
    }
    
    public Result Evaluate()
    {
        var binder = new Binder();
        GlobalScope globalscope = GlobalScope;
        //IBoundStatement expression = binder.BindStatement(_tree.Root);
        IBoundStatement expression = globalscope.Statement;
        _tree.Errors.Concat(binder.Errors);
        ErrorList errors = _tree.Errors;
        var evaluator = new Evaluator(expression, globalscope.Variables);
        _value = evaluator.Evaluate();

        return errors.Any() ? new Result(errors, null) : new Result(errors, _value);
    }
}