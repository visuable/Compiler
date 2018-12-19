using System;

namespace Compiler.Parsing.Ast
{
    internal class CallFunctionExpression : IExpression, ITabControl
    {
        private readonly IStatement _arguments;
        private readonly VariableExpression _name;

        public CallFunctionExpression(VariableExpression name, IStatement arguments)
        {
            _name = name;
            _arguments = arguments;
        }

        void IExpression.Evaluate()
        {
            ((ITabControl) this).WithFrontSpace();
        }

        void ITabControl.WithFrontSpace()
        {
            ((ITabControl) _name).WithFrontSpace();
            ((IConstructive) _arguments).NoNewLine();
        }

        void ITabControl.WithBackSpace()
        {
            ((ITabControl) _name).WithoutFrontSpace();
            ((IConstructive) _arguments).NoNewLine();
            Console.Write(" ");
        }

        void ITabControl.WithFrontAndBackSpace()
        {
            ((ITabControl) _name).WithFrontSpace();
            ((IConstructive) _arguments).NoNewLine();
            Console.Write(" ");
        }

        void ITabControl.WithoutFrontSpace()
        {
            ((ITabControl) _name).WithoutFrontSpace();
            ((IConstructive) _arguments).NoNewLine();
        }
    }
}