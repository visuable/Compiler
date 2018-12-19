using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parsing.Ast
{
    internal class ArgumentBlock : IStatement, IConstructive
    {
        private readonly List<CallArgument> _arguments;

        public ArgumentBlock(List<CallArgument> arguments)
        {
            _arguments = arguments;
        }

        void IConstructive.NoNewLine()
        {
            ((IStatement) this).Execute();
        }

        void IConstructive.NewLine()
        {
            ((IStatement) this).Execute();
            Console.WriteLine();
        }

        void IConstructive.IfLast(IStatement kind)
        {
            if (this == kind)
                ((IConstructive) this).NoNewLine();
            else
                ((IConstructive) this).NewLine();
        }

        void IStatement.Execute()
        {
            new BasicImplementation().ArgumentBlock(_arguments);
        }

        Type IStatement.CalledBy { get; set; }
    }
}