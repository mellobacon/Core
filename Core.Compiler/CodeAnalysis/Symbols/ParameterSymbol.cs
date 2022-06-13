using System;

namespace Core.Compiler.CodeAnalysis.Symbols;

public class ParameterSymbol : Symbol
{
    private readonly TypeSymbol _paramType;
    public ParameterSymbol(string name, TypeSymbol type) : base(name)
    {
        _paramType = type;
    }

    public override SymbolType Type => SymbolType.ParameterSymbol;
}