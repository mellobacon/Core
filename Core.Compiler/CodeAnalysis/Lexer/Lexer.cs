﻿using Core.Compiler.CodeAnalysis.Errors;
using Core.Compiler.CodeAnalysis.Symbols;

namespace Core.Compiler.CodeAnalysis.Lexer;
public class Lexer
{
    private readonly SourceText _text;
    private object? _value;
    private SyntaxTokenType _type;

    // keeps track of where the lexer is lexing in the string
    private int _start;
    private int _position;
    private char Current => GetToken(0);
    
    // keeps track of any errors during when lexing
    public ErrorList Errors { get; } = new();

    public Lexer(SourceText text)
    {
        _text = text;
    }

    // gets the current character getting lexed
    private char GetToken(int offset)
    {
        int index = _position + offset;
        return index >=_text.Length ? '\0' : _text[index];
    }

    // increments the position in the string
    private void Advance(int amount)
    {
        _position += amount;
    }

    private enum NumberType
    {
        IntType,
        FloatType,
        InvalidType
    }
    
    private object? GetValue(NumberType type, string text)
    {
        object? value = null;

        if (type == NumberType.IntType)
        {
            if (int.TryParse(text.Replace("_", ""), out int i))
            {
                value = i;
            }
            else
            {
                Errors.ReportInvalidNumberConversion(new TextSpan(_start, text.Length), text, TypeSymbol.Int);
            }
        }
        else if (type == NumberType.FloatType)
        {
            if (float.TryParse(text.Replace("_", ""), out float f))
            {
                value = f;
            }
            else
            {
                // report error
                Errors.ReportInvalidNumberConversion(new TextSpan(_start, text.Length), text, TypeSymbol.Float);
            }
        }
        else
        {
            Errors.ReportInvalidToken(new TextSpan(_start, text.Length), text);
        }

        return value;
    }

    public SyntaxToken Lex()
    {
        _start = _position;
        _value = null;
        _type = SyntaxTokenType.BadToken;
        var text = string.Empty;

        switch (Current)
        {
            case '\0':
                _type = SyntaxTokenType.EofToken;
                break;
            case var _ when char.IsDigit(Current) || Current is '.':
                text = LexNumber();
                break;
            case var _ when char.IsWhiteSpace(Current):
                text = LexWhiteSpace();
                break;
            case var _ when char.IsLetter(Current) || Current is '_':
                text = LexKeyword();
                break;
            case var _ when Current is '"':
                text = LexString();
                break;
            case '+':
                if (GetToken(1) == '=')
                {
                    _type = SyntaxTokenType.PlusEqualsToken;
                    Advance(2);
                }
                else
                {
                    _type = SyntaxTokenType.PlusToken;
                    Advance(1);
                }
                break;
            case '-':
                if (GetToken(1) == '=')
                {
                    _type = SyntaxTokenType.MinusEqualsToken;
                    Advance(2);
                }
                else
                {
                    _type = SyntaxTokenType.MinusToken;
                    Advance(1);
                }
                break;
            case '/':
                if (GetToken(1) == '=')
                {
                    _type = SyntaxTokenType.SlashEqualsToken;
                    Advance(2);
                }
                else
                {
                    _type = SyntaxTokenType.SlashToken;
                    Advance(1);
                }
                break;
            case '*':
                if (GetToken(1) == '=')
                {
                    _type = SyntaxTokenType.StarEqualsToken;
                    Advance(2);
                }
                else
                {
                    _type = SyntaxTokenType.StarToken;
                    Advance(1);
                }
                break;
            case '%':
                if (GetToken(1) == '=')
                {
                    _type = SyntaxTokenType.ModuloEqualsToken;
                    Advance(2);
                }
                else
                {
                    _type = SyntaxTokenType.ModuloToken;
                    Advance(1);
                }
                break;
            case '!':
                if (GetToken(1) == '=')
                {
                    _type = SyntaxTokenType.NotEqualsToken;
                    Advance(2);
                }
                else
                {
                    _type = SyntaxTokenType.BangToken;
                    Advance(1);
                }
                break;
            case '^':
                _type = SyntaxTokenType.HatToken;
                Advance(1);
                break;
            case '<':
                if (GetToken(1) == '=')
                {
                    _type = SyntaxTokenType.LessEqualsToken;
                    Advance(2);
                }
                else
                {
                    _type = SyntaxTokenType.LessThanToken;
                    Advance(1);
                }

                break;
            case '>':
                if (GetToken(1) == '=')
                {
                    _type = SyntaxTokenType.MoreEqualsToken;
                    Advance(2);
                }
                else
                {
                    _type = SyntaxTokenType.MoreThanToken;
                    Advance(1);
                }

                break;
            case '=':
                if (GetToken(1) == '=')
                {
                    _type = SyntaxTokenType.EqualsEqualsToken;
                    Advance(2);
                }
                else
                {
                    _type = SyntaxTokenType.EqualsToken;
                    Advance(1);
                }

                break;
            case '|':
                if (GetToken(1) == '|')
                {
                    _type = SyntaxTokenType.DoublePipeToken;
                }
                Advance(2);
                break;
            case '&':
                if (GetToken(1) == '&')
                {
                    _type = SyntaxTokenType.DoubleAmpersandToken;
                }
                Advance(2);
                break;
            case '(':
                _type = SyntaxTokenType.OpenParenToken;
                Advance(1);
                break;
            case ')':
                _type = SyntaxTokenType.ClosedParenToken;
                Advance(1);
                break;
            case '{':
                _type = SyntaxTokenType.OpenBracketToken;
                Advance(1);
                break;
            case '}':
                _type = SyntaxTokenType.ClosedBracketToken;
                Advance(1);
                break;
            case ';':
                _type = SyntaxTokenType.SemicolonToken;
                Advance(1);
                break;
            case ',':
                _type = SyntaxTokenType.CommaToken;
                Advance(1);
                break;
            default:
                Errors.ReportBadCharacter(Current, _position);
                Advance(1);
                break;
        }
        
        if (SyntaxInfo.GetText(_type) is not null)
        {
            text = SyntaxInfo.GetText(_type);
        }

        return new SyntaxToken(text!, _value, _type, _position);
    }
    
    private string LexNumber()
    {
        var numberType = NumberType.IntType;
        // continues getting the number or valid number character until there isn't another one to read
        while (char.IsDigit(Current) || Current is '_' or '.')
        {
            if (Current is '.') numberType = NumberType.FloatType;
            Advance(1);
            
            // a number cannot end with '.'(31.) or '_'(1000_)
            if (Current is '_' or '.' && GetToken(1) is not '_' && !char.IsDigit(GetToken(1)))
            {
                numberType = NumberType.InvalidType;
            }
            else if (GetToken(1) is '\0')
            {
                if (numberType is NumberType.InvalidType && Current is not '\0') Advance(1);
            }
        }
        int length = _position - _start;
        string text = _text.Substring(_start, length);
        object? value = GetValue(numberType, text);
        _type = value == null ? SyntaxTokenType.BadToken : SyntaxTokenType.NumberToken;
        _value = value;
        
        return text;
    }
    
    private string LexString()
    {
        Advance(1);
        while (Current is not '"')
        {
            Advance(1);
            if (Current is '"')
            {
                Advance(1);
                break;
            }
        }
        int length = _position - _start;
        string text = _text.Substring(_start, length);
        _type = SyntaxTokenType.StringToken;
        _value = text[1..^1]; // ignore surrounding quotes
        return text;
    }

    private string LexKeyword()
    {
        while (char.IsLetterOrDigit(Current) || Current is '_' or '@')
        {
            Advance(1);
        }
        int length = _position - _start;
        string text = _text.Substring(_start, length);
        _type = SyntaxInfo.GetKeywordType(text);
        _value = _type switch
        {
            SyntaxTokenType.TrueKeyword => true,
            SyntaxTokenType.FalseKeyword => false,
            _ => _value
        };
        
        return text;
    }
    
    private string LexWhiteSpace()
    {
        while (char.IsWhiteSpace(Current))
        {
            Advance(1);
        }

        _type = SyntaxTokenType.WhiteSpaceToken;
        return " ";
    }
}
