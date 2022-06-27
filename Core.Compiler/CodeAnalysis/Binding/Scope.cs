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
    
    private static readonly Dictionary<VariableSymbol, object?> _variableList = new();

    public object? GetVariableValue(string name)
    {
        foreach (VariableSymbol variable in _variableList.Keys)
        {
            if (variable.Name != name) continue;
            return _variableList[variable];
        }

        return null;
    }

    public VariableSymbol? GetVariable(string? name)
    {
        foreach (VariableSymbol variable in _variableList.Keys)
        {
            if (variable.Name != name) continue;
            return variable;
        }
        return null;
    }

    public void AddVariable(VariableSymbol variable)
    {
        _variableList[variable] = null;
    }

    public void SetVariable(VariableSymbol variable, object? value)
    {
        foreach (VariableSymbol v in _variableList.Keys)
        {
            if (v.Name == variable.Name)
            {
                _variableList[v] = value;
                return;
            }
        }
        _variableList[variable] = value;
    }
    
    public static Dictionary<VariableSymbol, object?> GetVariables()
    {
        return _variableList;
    }
}