using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parsing.Ast
{
    internal class DeclareArgumentBlock : IStatement, IConstructive
    {
        private readonly List<Argument> _arguments;

        public DeclareArgumentBlock(List<Argument> arguments)
        {
            _arguments = arguments;
        }

        void IConstructive.NoNewLine()
        {
            GetStringFromArgs();
        }

        void IConstructive.NewLine()
        {
            GetStringFromArgs();
            Console.WriteLine();
        }

        void IConstructive.IfLast(IStatement kind)
        {
            if (this == kind)
                ((IConstructive) this).NoNewLine();
            else ((IConstructive) this).NewLine();
        }

        void IStatement.Execute()
        {
            ((IConstructive) this).NewLine();
        }

        Type IStatement.CalledBy { get; set; }

        private void GetStringFromArgs()
        {
            Console.Write("(");
            if (_arguments.Count >= 1)
            {
                var last = _arguments.Last();
                foreach (var argument in _arguments)
                    if (argument == last)
                    {
                        ((ITabControl) argument.Name).WithBackSpace();
                        Console.Write("As " + argument.Type);
                    }
                    else
                    {
                        ((ITabControl) argument.Name).WithBackSpace();
                        Console.Write("As " + argument.Type + ", ");
                    }
            }

            Console.Write(")");
        }
    }
}