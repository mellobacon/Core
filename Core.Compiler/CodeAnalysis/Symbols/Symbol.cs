namespace Core.Compiler.CodeAnalysis.Symbols;

public abstract class Symbol
{
    public string Name { get; }
    public Symbol(string name)
    {
        Name = name;
    }
    public abstract SymbolType Type { get; }
}