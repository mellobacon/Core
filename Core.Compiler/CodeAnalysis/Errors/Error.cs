namespace Core.Compiler.CodeAnalysis.Errors;
public class Error
{
    public TextSpan TextSpan;
    public readonly string Message;
    public Error(TextSpan span, string message)
    {
        TextSpan = span;
        Message = message;
    }

    public override string ToString()
    {
        return Message;
    }
}