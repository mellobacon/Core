using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Core.Compiler.CodeAnalysis.Symbols;

public class Functions
{
    public static readonly FunctionSymbol Print = new("print", ImmutableArray.Create(new ParameterSymbol("text", TypeSymbol.Object)), TypeSymbol.Void);
    public static readonly FunctionSymbol PrintLn = new("println", ImmutableArray.Create(new ParameterSymbol("text", TypeSymbol.Object)), TypeSymbol.Void);
    
    internal static IEnumerable<FunctionSymbol?> GetAll() => 
        typeof(Functions).GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(FunctionSymbol)).Select(f => (FunctionSymbol)f.GetValue(null)!);
}