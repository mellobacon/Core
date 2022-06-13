using System;

namespace Core.Compiler.CodeAnalysis.Symbols;

public class VariableSymbol : Symbol
{
    public TypeSymbol VarType { get; } // change to type symbol later. im lazy
    public VariableSymbol(string name, TypeSymbol type) : base(name)
    {
        VarType = type;
    }

    public override SymbolType Type => SymbolType.VariableSymbol;
}