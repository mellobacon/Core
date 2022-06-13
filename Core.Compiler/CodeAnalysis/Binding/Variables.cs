using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding;

public static class Variables
{
    private static readonly Dictionary<VariableSymbol, object?> _variableList = new();

    public static object? GetVariableValue(string name)
    {
        foreach (VariableSymbol variable in _variableList.Keys)
        {
            if (variable.Name != name) continue;
            return _variableList[variable];
        }

        return null;
    }

    public static VariableSymbol? GetVariable(string? name)
    {
        foreach (VariableSymbol variable in _variableList.Keys)
        {
            if (variable.Name != name) continue;
            return variable;
        }
        return null;
    }

    public static void AddVariable(VariableSymbol variable)
    {
        _variableList[variable] = null;
    }

    public static void SetVariable(VariableSymbol variable, object? value)
    {
        foreach (var v in _variableList.Keys)
        {
            if (v.Name == variable.Name)
            {
                _variableList[v] = value;
                return;
            }
        }
        _variableList[variable] = value;
    }
}