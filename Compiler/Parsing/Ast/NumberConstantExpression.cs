using System;

namespace Compiler.Parsing.Ast
{
    internal class NumberConstantExpression : IExpression, ITabControl
    {
        private readonly string _value;

        public NumberConstantExpression(string value)
        {
            _value = value;
        }

        void IExpression.Evaluate()
        {
            ((ITabControl) this).WithFrontSpace();
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

        void ITabControl.WithoutFrontSpace()
        {
            Console.Write(_value);
        }
    }
}