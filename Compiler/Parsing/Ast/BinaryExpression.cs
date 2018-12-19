
using System;

namespace Compiler.Parsing.Ast
{
    internal class BinaryExpression : IExpression, ITabControl
    {
        private readonly IExpression _left;
        private readonly Operation _operation;
        private readonly IExpression _right;

        public BinaryExpression(IExpression left, IExpression right, Operation operation)
        {
            _left = left;
            _right = right;
            _operation = operation;
        }

        void IExpression.Evaluate()
        {
            ((ITabControl) this).WithFrontSpace();
        }

        void ITabControl.WithBackSpace()
        {
            ((ITabControl) _left).WithBackSpace();
            Operation();
            ((ITabControl) _right).WithFrontSpace();
        }

        void ITabControl.WithFrontAndBackSpace()
        {
            ((ITabControl) _left).WithFrontAndBackSpace();
            Operation();
            ((ITabControl) _right).WithFrontAndBackSpace();
        }

        void ITabControl.WithFrontSpace()
        {
            ((ITabControl) _left).WithFrontAndBackSpace();
            Operation();
            ((ITabControl) _right).WithFrontSpace();
        }

        void ITabControl.WithoutFrontSpace()
        {
            ((ITabControl) _left).WithBackSpace();
            Operation();
            ((ITabControl) _right).WithFrontSpace();
        }

        private void Operation()
        {
            new BasicImplementation().BinaryExpression(_operation);
        }
    }

    public enum Operation
    {
        Addition,
        Subtract,
        Times,
        Divide,

        LowOrEqual,
        GreaterOrEqual,

        Low,
        Greater,

        NotEqual,
        Equal
    }
}