using System;

namespace Core.Compiler.CodeAnalysis.Symbols;

public class FunctionSymbol : Symbol
{
    public readonly Type _type;
    public ParameterSymbol Param { get; }

    public FunctionSymbol(string name, ParameterSymbol param, Type type) : base(name)
    {
        _type = type;
        Param = param;
    }

    public override SymbolType Type => SymbolType.FunctionSymbol;
}