using System;

namespace Core.Compiler.CodeAnalysis.Symbols;

public class FunctionSymbol : Symbol
{
    public readonly TypeSymbol _type;
    private ParameterSymbol Param { get; }

    public FunctionSymbol(string name, ParameterSymbol param, TypeSymbol type) : base(name)
    {
        _type = type;
        Param = param;
    }

    public override SymbolType Type => SymbolType.FunctionSymbol;
}