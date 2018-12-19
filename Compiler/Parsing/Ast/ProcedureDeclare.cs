using System;

namespace Compiler.Parsing.Ast
{
    internal class ProcedureDeclare : IStatement, IConstructive
    {
        private readonly IStatement _arguments;
        private readonly VariableExpression _name;
        private readonly IStatement _statements;

        public ProcedureDeclare(VariableExpression name, IStatement arguments, IStatement statements)
        {
            _name = name;
            _arguments = arguments;
            _statements = statements;
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
            new BasicImplementation().ProcedureDeclare(_arguments, _name, _statements);
        }

        Type IStatement.CalledBy { get; set; }
    }
}