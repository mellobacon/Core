using System.Collections.Immutable;

namespace Core.Compiler.CodeAnalysis.Symbols;

public class FunctionSymbol : Symbol
{
    public readonly TypeSymbol FunctionType;
    private ImmutableArray<ParameterSymbol> Param { get; }

    public FunctionSymbol(string name, ImmutableArray<ParameterSymbol> param, TypeSymbol functionType) : base(name)
    {
        FunctionType = functionType;
        Param = param;
    }

    public override SymbolType Type => SymbolType.FunctionSymbol;
}