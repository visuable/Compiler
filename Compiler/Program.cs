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
        private const string FreeBasicCompilerPath = @"fbc\fbc.exe";
        private const string FreeBasicCompilerArgument = "Output.bas -O 3";

        private static void Main(string[] args)
        {
            var text = ArgumentHandler(args);
            var lexer = new Lexer(text);
            var parser = new ASTMaker(lexer);
            var statements = parser.ParseTokens();
            foreach (var item in lexer.Tokens) Console.WriteLine(item);

            var save = SetOutput(out var writerSave);
            ((IStatement) statements).Execute();
            Restore(save);
            if (parser.ParseErrors.Count > 0)
                foreach (var error in parser.ParseErrors)
                    throw error;
            try
            {
                Process.Start(new ProcessStartInfo(FreeBasicCompilerPath, FreeBasicCompilerArgument));
            }
            catch
            {
                Console.WriteLine("Compiler does not exists");
            }

            writerSave.Close();
        }

        private static string ArgumentHandler(string[] args)
        {
            if (args.Length == 0) throw new ArgumentNullException("Empty argument: usage file");
            if (args.Length > 1) throw new ArgumentException("Invalid count arguments: usage file");
            var reader = new StreamReader(args[0]);
            try
            {
                return reader.ReadToEnd();
            }
            catch
            {
                Console.WriteLine("File does not exists");
            }

            return "";
        }

        private static TextWriter SetOutput(out StreamWriter writerSave)
        {
            var tmp = Console.Out;
            var writer = new StreamWriter("Output.bas");
            writerSave = writer;
            Console.SetOut(writer);
            return tmp;
        }

        private static void Restore(TextWriter w)
        {
            Console.SetOut(w);
        }
    }
}