using System.Collections.Generic;

namespace Core.Compiler.CodeAnalysis.Binding;

public static class Variables
{
    private static Dictionary<Variable, object?> VariableList = new();

    public static object? GetVariableValue(string name)
    {
        foreach (Variable variable in VariableList.Keys)
        {
            if (variable.Name != name) continue;
            return VariableList[variable];
        }

        return null;
    }

    public static Variable? GetVariable(string? name)
    {
        foreach (Variable variable in VariableList.Keys)
        {
            if (variable.Name != name) continue;
            return variable;
        }
        return null;
    }

    public static void AddVariable(Variable variable)
    {
        VariableList[variable] = null;
    }

    public static void SetVariable(Variable variable, object? value)
    {
        foreach (var v in VariableList.Keys)
        {
            if (v.Name == variable.Name)
            {
                VariableList[v] = value;
                return;
            }
        }
        VariableList[variable] = value;
    }
}