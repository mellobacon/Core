namespace Core.Compiler.CodeAnalysis;

public class TextLine
{
    private readonly SourceText _text;
    public readonly int Start;
    private readonly int _length;
    private readonly int _lengthWithBreak;
    private int End => Start + _length;
    private TextSpan Span => new(Start, _length);
    public TextSpan SpanWithBreak => new(Start, _lengthWithBreak);

    public TextLine(SourceText text, int start, int length, int lengthwithbreak)
    {
        _text = text;
        Start = start;
        _length = length;
        _lengthWithBreak = lengthwithbreak;
    }

    public override string ToString() => _text.Substring(Start, _lengthWithBreak);
    public string ToString(int start, int length) => _text.Substring(Span);
    public string ToString(TextSpan span) => ToString(span.Start, span.Length);
}