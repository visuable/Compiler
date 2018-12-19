using System;

namespace Compiler.Parsing.Ast
{
    internal class ParenExpression : IExpression, ITabControl
    {
        private readonly IExpression _expression;

        public ParenExpression(IExpression expression)
        {
            _expression = expression;
        }

        void IExpression.Evaluate()
        {
            ((ITabControl) this).WithFrontSpace();
        }

        void ITabControl.WithoutFrontSpace()
        {
            Console.Write("(");
            ((ITabControl) _expression).WithoutFrontSpace();
            Console.Write(")");
        }

        void ITabControl.WithFrontSpace()
        {
            Console.Write(" (");
            ((ITabControl) _expression).WithoutFrontSpace();
            Console.Write(")");
        }

        void ITabControl.WithBackSpace()
        {
            Console.Write("(");
            ((ITabControl) _expression).WithoutFrontSpace();
            Console.Write(") ");
        }

        void ITabControl.WithFrontAndBackSpace()
        {
            Console.Write(" (");
            ((ITabControl) _expression).WithoutFrontSpace();
            Console.Write(") ");
        }
    }
}