using System.Collections.Generic;
using Core.Compiler.CodeAnalysis.Errors;
using Core.Compiler.CodeAnalysis.Lexer;
using Core.Compiler.CodeAnalysis.Parser.Expressions;

namespace Core.Compiler.CodeAnalysis.Parser;
public class Parser
{
    // keep track of the tokens we parse
    private int _position;
    private readonly List<SyntaxToken> _tokens = new();
    private SyntaxToken Current => Peek(0);
    private readonly ErrorList _errors = new();

    public Parser(string text)
    {
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
        // recursive decent parser
        ExpressionSyntax expression = ParseAssignmentExpression();
        SyntaxToken eofToken = MatchToken(SyntaxTokenType.EofToken);
        return new SyntaxTree(expression, eofToken, _errors);
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

        return ParseBinaryExpression();
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
