using System;

namespace Core.Compiler.CodeAnalysis.Symbols;

public class VariableSymbol : Symbol
{
    public Type VarType { get; } // change to type symbol later. im lazy
    public VariableSymbol(string name, Type type) : base(name)
    {
        VarType = type;
    }

    public override SymbolType Type => SymbolType.VariableSymbol;
}