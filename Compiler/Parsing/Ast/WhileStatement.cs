using System;

namespace Compiler.Parsing.Ast
{
    internal class WhileStatement : IStatement, IConstructive
    {
        private readonly IExpression _condition;
        private readonly IStatement _statements;

        public WhileStatement(IExpression condition, IStatement statements)
        {
            _condition = condition;
            _statements = statements;
        }

        void IConstructive.IfLast(IStatement kind)
        {
            if (this == kind)
                ((IConstructive) this).NoNewLine();
            else
                ((IConstructive) this).NewLine();
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

        void IStatement.Execute()
        {
            new BasicImplementation().WhileStatement(_condition, _statements);
        }

        Type IStatement.CalledBy { get; set; }
    }
}