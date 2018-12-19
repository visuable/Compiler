using System;
using Compiler.Parsing.Parser;

namespace Compiler.Parsing.Ast
{
    internal class FunctionDeclare : IStatement, IConstructive
    {
        private readonly IStatement _arguments;
        private readonly string _name;
        private readonly IStatement _statements;
        private readonly ParserType _type;

        public FunctionDeclare(string name, IStatement arguments, IStatement statements, ParserType type)
        {
            _name = name;
            _arguments = arguments;
            _statements = statements;
            _type = type;
        }

        void IConstructive.NewLine()
        {
            ((IStatement) this).Execute();
            Console.WriteLine();
        }

        void IConstructive.NoNewLine()
        {
            ((IStatement) this).Execute();
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
            new BasicImplementation().FunctionDeclare(_arguments, _name, _statements, _type);
        }

        Type IStatement.CalledBy { get; set; }
    }
}