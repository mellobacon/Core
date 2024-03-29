﻿using System.Collections.Generic;
using System.Collections.Immutable;
using Core.Compiler.CodeAnalysis.Errors;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Expressions;
using Core.Compiler.CodeAnalysis.Parser.Statements;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Parser;
public class Parser
{
    // keep track of the tokens we parse
    private int _position;
    private readonly List<SyntaxToken> _tokens = new();
    private SyntaxToken Current => Peek(0);
    private readonly ErrorList _errors = new();
    private readonly SourceText _text;

    public Parser(SourceText text)
    {
        _text = text;
        // Lex the tokens and add them to the tokens list for parsing
        var lexer = new Lexer.Lexer(text);
        while (true)
        {
            SyntaxToken token = lexer.Lex();

            if (token.Type is SyntaxTokenType.WhiteSpaceToken or SyntaxTokenType.BadToken)
            {
                continue;
            }
            _tokens.Add(token);
            
            if (token.Type == SyntaxTokenType.EofToken)
            {
                break;
            }
        }
        // add errors to the errors list
        _errors.Concat(lexer.Errors);
    }

    public SyntaxTree Parse()
    {
        StatementSyntax expression = ParseStatement();
        SyntaxToken eofToken = MatchToken(SyntaxTokenType.EofToken);
        return new SyntaxTree(expression, eofToken, _errors, _text);
    }

    private StatementSyntax ParseStatement()
    {
        if (Current.Type == SyntaxTokenType.VariableToken && Peek(1).Type == SyntaxTokenType.VariableToken &&
            Peek(2).Type == SyntaxTokenType.OpenParenToken)
        {
            SyntaxToken functype = MatchToken(SyntaxTokenType.VariableToken);
            SyntaxToken name = MatchToken(SyntaxTokenType.VariableToken);
            SyntaxToken openparen = MatchToken(SyntaxTokenType.OpenParenToken);
            var args = ParseArgs();
            SyntaxToken closedparen = MatchToken(SyntaxTokenType.ClosedParenToken);
            StatementSyntax statement = ParseStatement();
            return new FunctionDeclarationExpression(functype, name, openparen, args, closedparen, statement);
        }
        return Current.Type switch
        {
            SyntaxTokenType.OpenBracketToken => ParseBlockStatement(),
            SyntaxTokenType.IfKeyword => ParseIfStatement(),
            SyntaxTokenType.WhileKeyword => ParseWhileStatement(),
            SyntaxTokenType.ForKeyword => ParseForStatement(),
            SyntaxTokenType.VariableKeyword => ParseVariableStatement(),
            _ => ParseExpressionStatement()
        };
    }

    private StatementSyntax ParseBlockStatement()
    {
        ImmutableArray<StatementSyntax>.Builder statements = ImmutableArray.CreateBuilder<StatementSyntax>();
        SyntaxToken openbracket = MatchToken(SyntaxTokenType.OpenBracketToken);

        while (Current.Type != SyntaxTokenType.EofToken && Current.Type != SyntaxTokenType.ClosedBracketToken)
        {
            SyntaxToken starttoken = Current; // rename this
                
            StatementSyntax statement = ParseStatement();
            statements.Add(statement);

            if (Current == starttoken)
            {
                NextToken();
            }
        }
        
        SyntaxToken closedbracket = MatchToken(SyntaxTokenType.ClosedBracketToken);
            
        return new BlockStatement(openbracket, statements.ToImmutable(), closedbracket);
    }

    private StatementSyntax ParseIfStatement()
    {
        SyntaxToken ifkeyword = MatchToken(SyntaxTokenType.IfKeyword);
        SyntaxToken openparen = MatchToken(SyntaxTokenType.OpenParenToken);
        ExpressionSyntax condition = ParseBinaryExpression();
        SyntaxToken closeparen = MatchToken(SyntaxTokenType.ClosedParenToken);
        StatementSyntax statements = ParseStatement();
        ElseStatement? elseStatement = ParseElseStatement();
        return new IfStatement(ifkeyword, openparen, condition, closeparen, statements, elseStatement);
    }

    private ElseStatement? ParseElseStatement()
    {
        if (Current.Type != SyntaxTokenType.ElseKeyword)
        {
            if (Current.Type is SyntaxTokenType.IfKeyword)
            {
                ParseStatement();
            }
            return null;
        }

        SyntaxToken elsekeyword = NextToken();
        StatementSyntax statements = ParseStatement();
        return new ElseStatement(elsekeyword, statements);
    }

    private StatementSyntax ParseWhileStatement()
    {
        SyntaxToken whilekeyword = MatchToken(SyntaxTokenType.WhileKeyword);
        SyntaxToken openparen = MatchToken(SyntaxTokenType.OpenParenToken);
        ExpressionSyntax condition = ParseBinaryExpression();
        SyntaxToken closeparen = MatchToken(SyntaxTokenType.ClosedParenToken);
        StatementSyntax statements = ParseStatement();
        return new WhileStatement(whilekeyword, openparen, condition, closeparen, statements);
    }

    private StatementSyntax ParseForStatement()
    {
        SyntaxToken forkeyword = MatchToken(SyntaxTokenType.ForKeyword);
        SyntaxToken openparen = MatchToken(SyntaxTokenType.OpenParenToken);
        StatementSyntax init = ParseStatement();
        ExpressionSyntax condition = ParseAssignmentExpression();
        SyntaxToken semicolon = MatchToken(SyntaxTokenType.SemicolonToken);
        ExpressionSyntax iterator = ParseAssignmentExpression();
        SyntaxToken closeparen = MatchToken(SyntaxTokenType.ClosedParenToken);
        StatementSyntax statement = ParseStatement();
        return new ForStatement(forkeyword, openparen, init, condition, semicolon, iterator, closeparen, statement);
    }

    private StatementSyntax ParseVariableStatement()
    {
        SyntaxToken keyword = MatchToken(SyntaxTokenType.VariableKeyword);
        SyntaxToken vartype = MatchToken(SyntaxTokenType.VariableToken);
        SyntaxToken variable = MatchToken(SyntaxTokenType.VariableToken);
        SyntaxToken equals = MatchToken(SyntaxTokenType.EqualsToken);
        ExpressionSyntax expression = ParseAssignmentExpression();
        SyntaxToken semicolon = MatchToken(SyntaxTokenType.SemicolonToken);
        return new VariableStatement(keyword, vartype, variable, equals, expression, semicolon);
    }

    private StatementSyntax ParseExpressionStatement()
    {
        ExpressionSyntax expression = ParseAssignmentExpression();
        SyntaxToken semicolon = MatchToken(SyntaxTokenType.SemicolonToken);
        return new ExpressionStatement(expression, semicolon);
    }

    private ExpressionSyntax ParseAssignmentExpression()
    {
        if (Current.Type == SyntaxTokenType.VariableToken && Peek(1).Type == SyntaxTokenType.EqualsToken)
        {
            SyntaxToken variable = NextToken();
            SyntaxToken equalstoken = NextToken();
            ExpressionSyntax expression = ParseAssignmentExpression();
            return new AssignmentExpression(variable, equalstoken, expression);
        }

        if (Current.Type == SyntaxTokenType.VariableToken && SyntaxInfo.IsCompoundOperator(Peek(1)))
        {
            SyntaxToken compoundop = SyntaxInfo.GetOp(Peek(1));
            bool iscompoundop = SyntaxInfo.IsCompoundOperator(Peek(1));
            SyntaxToken variable = NextToken();
            SyntaxToken equalstoken = NextToken();
            ExpressionSyntax expression = ParseBinaryExpression();
            return new AssignmentExpression(variable, equalstoken, compoundop, expression, iscompoundop);
        }

        if (Current.Type == SyntaxTokenType.VariableToken && Peek(1).Type == SyntaxTokenType.OpenParenToken)
        {
            SyntaxToken name = MatchToken(SyntaxTokenType.VariableToken);
            SyntaxToken openparen = MatchToken(SyntaxTokenType.OpenParenToken);
            var arg = ParseArgs();
            SyntaxToken closedparen = MatchToken(SyntaxTokenType.ClosedParenToken);
            return new FunctionCallExpression(name, openparen, arg, closedparen);
        }

        return ParseBinaryExpression();
    }

    private Parameters<ExpressionSyntax> ParseArgs()
    {
        ImmutableArray<SyntaxNode>.Builder x = ImmutableArray.CreateBuilder<SyntaxNode>();
        var nextparam = true;
        while (nextparam && Current.Type != SyntaxTokenType.ClosedParenToken 
                         && Current.Type != SyntaxTokenType.EofToken)
        {
            ExpressionSyntax expression = ParseLiteralExpression();
            x.Add(expression);
            if (Current.Type == SyntaxTokenType.CommaToken)
            {
                SyntaxToken comma = MatchToken(SyntaxTokenType.CommaToken);
                x.Add(comma);
            }
            else
            {
                nextparam = false;
            }
        }

        return new Parameters<ExpressionSyntax>(x.ToImmutable());
    }

    private ExpressionSyntax ParseUnaryExpression(int precedence = 0)
    {
        int currentPrecedence = SyntaxInfo.GetUnaryPrecedence(Current.Type);
        if (currentPrecedence != 0 && currentPrecedence >= precedence)
        {
            SyntaxToken op = NextToken();
            ExpressionSyntax operand = ParseBinaryExpression(currentPrecedence);
            return new UnaryExpression(op, operand);
        }

        return ParseLiteralExpression();
    }

    private ExpressionSyntax ParseBinaryExpression(int precedence = 0)
    {
        ExpressionSyntax left = ParseUnaryExpression(precedence);
        while (true)
        {
            int currentPrecedence = SyntaxInfo.GetBinaryPrecedence(Current.Type);
            if (currentPrecedence.Equals(0) || currentPrecedence <= precedence)
            {
                break;
            }

            SyntaxToken op = NextToken();
            ExpressionSyntax right = ParseBinaryExpression(currentPrecedence);
            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    private ExpressionSyntax ParseLiteralExpression()
    {
        switch (Current.Type)
        {
            case SyntaxTokenType.OpenParenToken:
                SyntaxToken openparen = MatchToken(SyntaxTokenType.OpenParenToken);
                ExpressionSyntax expression = ParseBinaryExpression();
                SyntaxToken closedparen = MatchToken(SyntaxTokenType.ClosedParenToken);
                return new GroupedExpression(openparen, expression, closedparen);
            case SyntaxTokenType.FalseKeyword:
            case SyntaxTokenType.TrueKeyword:
                SyntaxToken keywordtoken = NextToken();
                bool value = keywordtoken.Type == SyntaxTokenType.TrueKeyword;
                return new LiteralExpression(keywordtoken, value);
            case SyntaxTokenType.VariableToken:
                SyntaxToken variabletoken = MatchToken(SyntaxTokenType.VariableToken);
                return new VariableExpression(variabletoken);
            case SyntaxTokenType.StringToken:
                SyntaxToken stringtoken = MatchToken(SyntaxTokenType.StringToken);
                return new LiteralExpression(stringtoken, stringtoken.Value);
            default:
                SyntaxToken number = MatchToken(SyntaxTokenType.NumberToken);
                return new LiteralExpression(number, number.Value);
        }
    }

    private SyntaxToken Peek(int offset)
    {
        int index = _position + offset;
        return index >= _tokens.Count ? _tokens[^1] : _tokens[index];
    }

    private SyntaxToken MatchToken(SyntaxTokenType type)
    {
        if (Current.Type == type)
        {
            return NextToken();
        }
        _errors.ReportUnExpectedToken(Current.TextSpan, Current.Text, Current.Type, type);
        return new SyntaxToken(null, null, type, Current.Position);
    }

    private SyntaxToken NextToken()
    {
        SyntaxToken current = Current;
        _position++;
        return current;
    }
}
