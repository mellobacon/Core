namespace Core.Compiler.CodeAnalysis.Symbols;

public class TypeSymbol : Symbol
{
    public static readonly TypeSymbol String = new("string");
    public static readonly TypeSymbol Int = new("int");
    public static readonly TypeSymbol Bool = new("bool");
    public static readonly TypeSymbol Float = new("float");
    public static readonly TypeSymbol Double = new("double");
    public static readonly TypeSymbol Void = new("void");
    public static readonly TypeSymbol Object = new("object");
    public static readonly TypeSymbol Error = new("undefined");

    private TypeSymbol(string name) : base(name)
    {
    }

    public override SymbolType Type => SymbolType.TypeSymbol;
}