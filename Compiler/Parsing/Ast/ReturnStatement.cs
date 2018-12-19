using System;

namespace Compiler.Parsing.Ast
{
    internal class ReturnStatement : IStatement, IConstructive
    {
        private readonly IExpression _right;

        public ReturnStatement(IExpression right)
        {
            _right = right;
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
            new BasicImplementation().ReturnStatement(_right, this);
        }

        Type IStatement.CalledBy { get; set; }
    }
}