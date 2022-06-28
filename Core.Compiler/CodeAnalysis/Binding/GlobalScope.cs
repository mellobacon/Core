using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Binding.Statements;
using Core.Compiler.CodeAnalysis.Errors;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding;

public class GlobalScope
{
    public GlobalScope? _GlobalScope { get; }
    public IBoundStatement Statement { get; }

    public Dictionary<VariableSymbol, object?> Variables { get; }

    public ErrorList Errors { get; }
    
    public GlobalScope(GlobalScope? globalScope, IBoundStatement statement, Dictionary<VariableSymbol, object?> variables, ErrorList errors)
    {
        _GlobalScope = globalScope;
        Statement = statement;
        Variables = variables;
        Errors = errors;
    }
}