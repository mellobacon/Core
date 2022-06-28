using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding;

public class Scope
{
    public readonly Scope? ParentScope;

    public Scope(Scope? parent)
    {
        ParentScope = parent;
    }

    private readonly Dictionary<VariableSymbol, object?> _variableList = new();

    public VariableSymbol? GetVariable(string? name)
    {
        foreach (VariableSymbol variable in _variableList.Keys)
        {
            if (variable.Name != name) continue;
            return variable;
        }

        return ParentScope?.GetVariable(name);
    }

    public bool AddVariable(VariableSymbol variable)
    {
        if (_variableList.ContainsKey(variable))
        {
            return false;
        }
        _variableList.Add(variable, null);
        return true;
    }

    public Dictionary<VariableSymbol, object?> GetVariables()
    {
        return _variableList;
    }
}