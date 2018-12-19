using System;

namespace Compiler.Parsing.Ast
{
    internal class PrintStatement : IStatement, IConstructive
    {
        private readonly IExpression _expression;

        public PrintStatement(IExpression expression)
        {
            _expression = expression;
        }

        void IConstructive.NoNewLine()
        {
            new BasicImplementation().PrintStatement(_expression);
        }

        void IConstructive.NewLine()
        {
            new BasicImplementation().PrintStatement(_expression);
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
            ((IConstructive) this).NewLine();
        }

        Type IStatement.CalledBy { get; set; }
    }
}