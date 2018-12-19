using System;

namespace Compiler.Parsing.Ast
{
    internal class IfStatement : IStatement, IConstructive
    {
        private readonly IExpression _condition;
        private readonly bool _containElse;
        private readonly IStatement _elseStatement;
        private readonly IStatement _statement;

        public IfStatement(IExpression condition, IStatement statement, IStatement elseStatement, bool containElse)
        {
            _condition = condition;
            _statement = statement;
            _elseStatement = elseStatement;
            _containElse = containElse;
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
            Console.Write("If");
            ((ITabControl) _condition).WithFrontAndBackSpace();
            Console.WriteLine("Then");
            ((IConstructive) _statement).NewLine();
            if (_containElse)
            {
                Console.WriteLine("Else");
                ((IConstructive) _elseStatement).NoNewLine();
            }

            Console.WriteLine("Endif");
        }

        Type IStatement.CalledBy { get; set; }
    }
}