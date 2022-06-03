using System;
using Core.Compiler.CodeAnalysis.Lexer;

namespace Core.Compiler.CodeAnalysis;

public class Variable
{
    private string Name { get; }
    private Type Type { get; }
    public Variable(string varName, Type resultType)
    {
        Name = varName;
        Type = resultType;
    }
}