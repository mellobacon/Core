using System;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis;

public class Variable
{
    private string Name { get; }
    private TypeSymbol Type { get; }
    public Variable(string varName, TypeSymbol resultType)
    {
        Name = varName;
        Type = resultType;
    }
}