using System.IO;
using Core.Compiler;

namespace Core;

internal static class Program
{
    public static void Main(string[] args)
    {
        var repl = new Repl()
        {
            Prompt = "» ",
            MultilinePrompt = "→ "
        };
        repl.Run("../../../testfile.corelang"); //../../../testfile.corelang"
    }
}