using System;

namespace Compiler.Parsing.Ast
{
    internal class ExpressionEvalStatement : IStatement, IConstructive
    {
        private readonly IExpression _expression;

        public ExpressionEvalStatement(IExpression expression)
        {
            _expression = expression;
        }

        void IConstructive.NewLine()
        {
            ((ITabControl) _expression).WithoutFrontSpace();
            Console.WriteLine();
        }

        void IConstructive.NoNewLine()
        {
            ((ITabControl) _expression).WithFrontSpace();
        }

        void IConstructive.IfLast(IStatement kind)
        {
            if (this == kind) ((IConstructive) this).NoNewLine();
            else ((IConstructive) this).NewLine();
        }

        void IStatement.Execute()
        {
            ((IConstructive) this).NewLine();
        }

        Type IStatement.CalledBy { get; set; }
    }
}