namespace Core.Compiler.CodeAnalysis.Symbols;

public abstract class Symbol
{
    public string Name { get; }

    protected Symbol(string name)
    {
        Name = name;
    }
    public abstract SymbolType Type { get; }
    public override string ToString() => Name;
}