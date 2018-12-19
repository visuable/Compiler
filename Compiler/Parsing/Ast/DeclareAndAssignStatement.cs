using System;
using Compiler.Parsing.Parser;

namespace Compiler.Parsing.Ast
{
    internal class DeclareAndAssignStatement : IStatement, IConstructive
    {
        private readonly VariableExpression _name;
        private readonly IExpression _right;
        private readonly ParserType _type;

        public DeclareAndAssignStatement(ParserType type, VariableExpression name, IExpression right)
        {
            _type = type;
            _name = name;
            _right = right;
        }

        void IConstructive.NewLine()
        {
            Execution();
            Console.WriteLine();
        }

        void IConstructive.NoNewLine()
        {
            Execution();
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
            ((IConstructive)this).NewLine();
        }

        Type IStatement.CalledBy { get; set; }

        private void Execution()
        {
            new BasicImplementation().DeclareAndAssignStatement(_type, _name, _right);
        }
    }
}