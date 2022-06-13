using System;

namespace Core.Compiler.CodeAnalysis.Symbols;

public class ParameterSymbol : Symbol
{

    public readonly Type ParamType;
    public ParameterSymbol(string name, Type type) : base(name)
    {
        ParamType = type;
    }

    public override SymbolType Type => SymbolType.ParameterSymbol;
}