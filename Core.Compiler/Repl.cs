﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using Core.Compiler.CodeAnalysis;
using Core.Compiler.CodeAnalysis.Errors;
using Core.Compiler.CodeAnalysis.Evaluator;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser;

namespace Core.Compiler;
/**
 * Runs the compiler based on input
 */
public class Repl
{
    /**
     * The commandline prompt
     */
    public string Prompt = ">";

    /**
     * The commandline prompt for multilines
     */
    public string MultilinePrompt = "#";

    private Compilation? _previousCompilation;

    /**
     * <summary>Runs the compiler. If there is no argument specified, it will run via inputs from the cmd.</summary>
     * <param name="path">The path to a text file</param>
     */
    public void Run(string? path = null)
    {
        var textbuilder = new StringBuilder();
        
        // Set the file for getting input from it
        StreamReader? file = null;
        if (path != null)
        {
            file = new StreamReader(path);
        }
        
        while (true)
        {
            string? input;
            if (path is null) // Get input from the cmd
            {
                if (textbuilder.Length == 0)
                {
                    Console.Write(Prompt);
                    input = Console.ReadLine();
                }
                else
                {
                    Console.Write(MultilinePrompt);
                    input = Console.ReadLine();
                }
            }
            else // Get input from the file
            {
                input = file!.ReadLine();
            }
            
            bool isblank = string.IsNullOrWhiteSpace(input);
            if (isblank && textbuilder.Length == 0)
            {
                break;
            }
            textbuilder.AppendLine(input);
            var text = textbuilder.ToString();
            
            SyntaxTree tree = SyntaxTree.Parse(text);
            if (!isblank && tree.Errors.Any())
            {
                continue;
            }
            
            // Evaluate the expression and print the output, along with any errors
            Compilation compilation;
            if (_previousCompilation is null)
            {
                compilation = new Compilation(null, tree);
            }
            else
            {
                compilation = _previousCompilation.Continue(tree);
            }
            
            Result result = compilation.Evaluate();

            if (!result.Errors.Any())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine(result.Value);
                Console.ResetColor();
                _previousCompilation = compilation;
            }
            
            SourceText sourcetext = tree.Text;
            foreach (Error error in result.Errors.ToArray())
            {
                int lineindex = sourcetext.GetLineIndex(error.TextSpan.Start);
                int linenumber = lineindex + 1;
                int character = error.TextSpan.Start - sourcetext.Lines[lineindex].Start + 1;
                
                if (error.ToString().Contains("Eof")) break;
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"({linenumber}, {character}): {error}");
                Console.ResetColor();

                // dont print the extra stuff for files since the input is all read at once (TODO: Fix this)
                if (path is not null)
                {
                    continue;
                }
                string prefix = text[..error.TextSpan.Start];
                string e = text.Substring(error.TextSpan.Start, error.TextSpan.Length);
                string suffix = text[error.TextSpan.End..];
                
                Console.Write(prefix);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(e);
                Console.ResetColor();
                Console.Write(suffix.TrimEnd());
                Console.WriteLine();
                
                Console.ForegroundColor = ConsoleColor.DarkRed;
                for (var _ = 0; _ < prefix.Length; _++)
                {
                    Console.Write(" ");
                }

                for (var _ = 0; _ < e.Length; _++)
                {
                    Console.Write("^");
                }
                Console.WriteLine();
                Console.ResetColor();
            }

            textbuilder.Clear();
        }
    }

    /**
     * <summary>Prints the syntax tree</summary>
     * <param name="node">The node to put in the tree</param>
     * <param name="indent">The string to indent with. Defaults to an empty string</param>
     * <param name="isLast">Defines if the node is the last in the branch. Defaults to true</param>
     */
    private static void ShowTree(SyntaxNode node, string indent = "", bool isLast = true)
    {
        string marker = isLast ? "└──" : "├──";
        Console.Write(indent);
        Console.Write(marker);
        
        switch (node.Type)
        {
            case SyntaxTokenType.FunctionCallExpression:
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write(node.Type);
                Console.ResetColor();
                break;
            case SyntaxTokenType.BinaryExpression: 
            case SyntaxTokenType.UnaryExpression: 
            case SyntaxTokenType.AssignmentExpression:
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write(node.Type);
                Console.ResetColor();
                break;
            case SyntaxTokenType.LiteralExpression:
            case SyntaxTokenType.VariableToken:
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(node.Type);
                Console.ResetColor();
                break;
            case SyntaxTokenType.StarToken:
            case SyntaxTokenType.SlashToken:
            case SyntaxTokenType.PlusToken:
            case SyntaxTokenType.MinusToken:
            case SyntaxTokenType.ModuloToken: 
            case SyntaxTokenType.LessThanToken: 
            case SyntaxTokenType.MoreThanToken: 
            case SyntaxTokenType.LessEqualsToken: 
            case SyntaxTokenType.MoreEqualsToken: 
            case SyntaxTokenType.EqualsToken: 
            case SyntaxTokenType.DoublePipeToken: 
            case SyntaxTokenType.DoubleAmpersandToken: 
            case SyntaxTokenType.EqualsEqualsToken:
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(node.Type);
                Console.ResetColor();
                break;
            default:
                Console.ResetColor();
                Console.Write(node.Type);
                break;
        }

        // Print numbers / token text
        if (node is SyntaxToken token)
        {
            if (token.Value is not null)
            {
                Console.Write(" ");
                Console.Write(token.Value);
            }
            else if (token.Text is not null)
            {
                Console.Write(" ");
                Console.Write(token.Text);
            }
        }

        Console.WriteLine();
        indent += isLast ? "   " : "│  ";
        SyntaxNode? last = node.GetChildren().LastOrDefault();
        foreach (SyntaxNode child in node.GetChildren())
        {
            ShowTree(child, indent, child == last);
        }
    }
}
