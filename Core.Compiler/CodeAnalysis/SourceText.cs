using System.Collections.Immutable;

namespace Core.Compiler.CodeAnalysis;

public class SourceText
{
    private readonly string _text;
    public ImmutableArray<TextLine> Lines;
    public SourceText(string text)
    {
        _text = text;
        Lines = ParseLines(this, text);
    }

    private static ImmutableArray<TextLine> ParseLines(SourceText sourcetext, string text)
    {
        ImmutableArray<TextLine>.Builder result = ImmutableArray.CreateBuilder<TextLine>();
        var position = 0;
        var start = 0;
        while (position < text.Length)
        {
            int linebreakwidth = GetLineBreakWidth(text, position);
            if (linebreakwidth == 0)
            {
                position++;
            }
            else
            {
                AddLine(sourcetext, position, start, linebreakwidth, result);
                position += linebreakwidth;
                start = position;
            }
        }

        if (position >= start)
        {
            AddLine(sourcetext, position, start, 0, result);
        }

        return result.ToImmutable();
    }

    private static int GetLineBreakWidth(string text, int i)
    {
        char character = text[i];
        char length = i + 1 >= text.Length ? '\0' : text[i + 1];
        return character switch
        {
            '\r' when length == '\n' => 2,
            '\r' or '\n' => 1,
            _ => 0
        };
    }

    private static void AddLine(SourceText text, int position, int start, int linebreakwidth, 
        ImmutableArray<TextLine>.Builder result)
    {
        int linelength = position - start;
        int linelengthwithbreak = linelength + linebreakwidth;
        var line = new TextLine(text, start, linelength, linelengthwithbreak);
        result.Add(line);
    }

    public int GetLineIndex(int position)
    {
        var lower = 0;
        int upper = Lines.Length - 1;
        while (lower <= upper)
        {
            int index = lower + (upper - lower) / 2;
            int start = Lines[index].Start;
            if (position == start)
            {
                return index;
            }
            if (start > position)
            {
                upper = index - 1;
            }
            else
            {
                lower = index + 1;
            }
        }

        return lower - 1;
    }

    public static SourceText From(string text)
    {
        return new SourceText(text);
    }
    
    public char this[int index] => _text[index];
    public int Length => _text.Length;
    public bool Contains(char i) => _text.Contains(i);
    public override string ToString() => _text;
    public string Substring(int start, int length) => _text.Substring(start, length);
    public string Substring(TextSpan span) => Substring(span.Start, span.Length);
}