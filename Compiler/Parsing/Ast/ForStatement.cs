using System;

namespace Compiler.Parsing.Ast
{
    internal class ForStatement : IStatement, IConstructive
    {
        private readonly IStatement _assigns;
        private readonly IStatement _statements;
        private readonly IStatement _step;

        public ForStatement(IStatement assigns, IStatement step, IStatement statements)
        {
            _assigns = assigns;
            _step = step;
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
            new BasicImplementation().ForStatement(_assigns, _statements, _step);
        }

        Type IStatement.CalledBy { get; set; }
    }
}