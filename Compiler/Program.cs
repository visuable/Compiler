using System;
using System.Diagnostics;
using System.IO;
using Compiler.Lexical;
using Compiler.Parsing.Ast;
using Compiler.Parsing.Parser;

namespace Compiler
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            if(args.Length == 0) return;
            var lexer = new Lexer(new StreamReader(args[0]).ReadToEnd());
            var parser = new ASTMaker(lexer);
            var statements = parser.ParseTokens();
            if (parser.ParseErrors.Count > 0)
            {
                foreach(var error in parser.ParseErrors) Console.WriteLine(error.Message);
            }
            SetOutput(out var writer);
            ((IStatement)statements).Execute();
            Restore(writer);
        }

        private static void SetOutput(out TextWriter writerSave)
        {
            var writer = new StreamWriter(Path.Combine(Settings.Default.OutputPath, "output"));
            writerSave = Console.Out;
            Console.SetOut(writer);
        }

        private static void Restore(TextWriter w)
        {
            Console.SetOut(w);
        }
    }
}