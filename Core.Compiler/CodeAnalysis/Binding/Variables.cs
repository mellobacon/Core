using System.Collections.Generic;

namespace Core.Compiler.CodeAnalysis.Binding;

public static class Variables
{
    private static readonly Dictionary<Variable, object?> _variableList = new();

    public static object? GetVariableValue(string name)
    {
        foreach (Variable variable in _variableList.Keys)
        {
            if (variable.Name != name) continue;
            return _variableList[variable];
        }

        return null;
    }

    public static Variable? GetVariable(string? name)
    {
        foreach (Variable variable in _variableList.Keys)
        {
            if (variable.Name != name) continue;
            return variable;
        }
        return null;
    }

    public static void AddVariable(Variable variable)
    {
        _variableList[variable] = null;
    }

    public static void SetVariable(Variable variable, object? value)
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