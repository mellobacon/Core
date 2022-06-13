using System;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis;

public class Variable
{
    public string Name { get; }
    public TypeSymbol Type { get; }
    public Variable(string varName, TypeSymbol resultType)
    {
        Name = varName;
        Type = resultType;
    }
}