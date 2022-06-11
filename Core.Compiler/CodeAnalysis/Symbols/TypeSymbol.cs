namespace Core.Compiler.CodeAnalysis.Symbols;

public class TypeSymbol : Symbol
{
    public TypeSymbol(string name) : base(name)
    {
    }

    public override SymbolType Type => SymbolType.TypeSymbol;
}