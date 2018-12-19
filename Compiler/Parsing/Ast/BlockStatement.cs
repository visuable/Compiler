using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parsing.Ast
{
    internal class BlockStatement : IStatement, IConstructive
    {
        private readonly IStatement _last;
        private readonly List<IStatement> _statements;

        public BlockStatement(List<IStatement> statements)
        {
            _statements = statements;
            if (_statements.Count > 0)
            {
                _last = statements.Last();
                CheckModule();
            }
        }

        public bool IsModule { get; private set; }

        void IConstructive.NewLine()
        {
            ((IStatement)this).Execute();
            Console.WriteLine();
        }

        void IConstructive.NoNewLine()
        {
            ((IStatement)this).Execute();
        }

        void IConstructive.IfLast(IStatement kind)
        {
            if (this == kind)
                ((IConstructive)this).NoNewLine();
            else
                ((IConstructive)this).NewLine();
        }

        void IStatement.Execute()
        {
            new BasicImplementation().BlockStatement(_statements, _last, this);
        }

        Type IStatement.CalledBy { get; set; }

        private void CheckModule()
        {
            foreach (var statement in _statements)
                if (!(statement is FunctionDeclare || statement is ProcedureDeclare))
                    return;

            IsModule = true;
        }
    }
}