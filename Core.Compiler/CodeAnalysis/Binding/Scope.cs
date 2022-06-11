using System.Collections.Immutable;
using Core.Compiler.CodeAnalysis.Binding.Statements;
using Core.Compiler.CodeAnalysis.Errors;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Binding;

public class Scope
{
    public Scope Previous { get; }
    public ImmutableArray<Error> Errors { get; }
    public ImmutableArray<VariableSymbol> Variables { get; }
    public IBoundStatement Statement { get; }

    public Scope(Scope previous, ImmutableArray<Error> errors, ImmutableArray<VariableSymbol> variables, 
        IBoundStatement statement)
    {
        Previous = previous;
        Errors = errors;
        Variables = variables;
        Statement = statement;
    }
}