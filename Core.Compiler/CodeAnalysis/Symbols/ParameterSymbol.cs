using System;

namespace Core.Compiler.CodeAnalysis.Symbols;

public class ParameterSymbol : Symbol
{
    private readonly Type _paramType;
    public ParameterSymbol(string name, Type type) : base(name)
    {
        _paramType = type;
    }

    public override SymbolType Type => SymbolType.ParameterSymbol;
}