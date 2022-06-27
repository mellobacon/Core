using System.Collections;
using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Binding.Statements;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding;

public class GlobalScope
{
    public GlobalScope? _GlobalScope { get; }
    public IBoundStatement Statement { get; }

    public Dictionary<VariableSymbol, object?> Variables { get; }
    
    public GlobalScope(GlobalScope? scope, IBoundStatement statement)
    {
        _GlobalScope = scope;
        Statement = statement;
        Variables = Scope.GetVariables();
    }
}