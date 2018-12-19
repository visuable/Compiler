using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parsing.Ast
{
    internal class ArrayUseExpression : IExpression, ITabControl
    {
        private readonly List<IExpression> _offset;
        private readonly VariableExpression _variable;

        public ArrayUseExpression(VariableExpression variable, List<IExpression> offset)
        {
            _variable = variable;
            _offset = offset;
        }

        void IExpression.Evaluate()
        {
            ((ITabControl) this).WithFrontSpace();
        }

        void ITabControl.WithoutFrontSpace()
        {
            ((ITabControl)_variable).WithoutFrontSpace();
            GetSize();
        }

        private void GetSize()
        {
            new BasicImplementation().ArrayUseExpression(_offset, _variable);
        }

        void ITabControl.WithFrontSpace()
        {
            ((ITabControl) _variable).WithFrontSpace();
            GetSize();
        }

        void ITabControl.WithBackSpace()
        {
            ((ITabControl) _variable).WithoutFrontSpace();
            GetSize();
            Console.Write(" ");
        }

        void ITabControl.WithFrontAndBackSpace()
        {
            ((ITabControl) _variable).WithFrontSpace();
            GetSize();
            Console.Write(" ");
        }
    }
}