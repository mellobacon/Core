using System;

namespace Core.Compiler.CodeAnalysis;

public class Variable
{
    public string Name { get; }
    public Type Type { get; }
    public Variable(string varName, Type resultType)
    {
        Name = varName;
        Type = resultType;
    }
}