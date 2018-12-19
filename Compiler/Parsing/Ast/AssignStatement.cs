using System;

namespace Compiler.Parsing.Ast
{
    internal class AssignStatement : IStatement, IConstructive
    {
        private readonly IExpression _name;

        private readonly IExpression _right;

        public AssignStatement(IExpression name, IExpression right)
        {
            _name = name;
            _right = right;

            if (!(_name is VariableExpression | _name is ArrayUseExpression)) throw new Exception("Wtf u dying");
        }

        void IConstructive.NoNewLine()
        {
            ((ITabControl) _name).WithFrontAndBackSpace();
            Print();
        }

        void IConstructive.NewLine()
        {
            ((ITabControl) _name).WithBackSpace();
            Print();
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

        private void Print()
        {
            new BasicImplementation().AssignStatement(_right);
        }
    }
}