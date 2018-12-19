using System;

namespace Compiler.Parsing.Ast
{
    internal class VariableExpression : IExpression, ITabControl
    {
        private readonly string _value;

        public VariableExpression(string value)
        {
            _value = value;
        }

        void IExpression.Evaluate()
        {
            ((ITabControl) this).WithFrontSpace();
        }

        void ITabControl.WithoutFrontSpace()
        {
            Console.Write(_value);
        }

        void ITabControl.WithFrontSpace()
        {
            Console.Write(" " + _value);
        }

        void ITabControl.WithBackSpace()
        {
            Console.Write(_value + " ");
        }

        void ITabControl.WithFrontAndBackSpace()
        {
            Console.Write(" " + _value + " ");
        }
    }
}