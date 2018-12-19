using System;
using System.Collections.Generic;
using System.Linq;
using Compiler.Parsing.Parser;

namespace Compiler.Parsing.Ast
{
    internal class ArrayDeclare : IStatement, IConstructive
    {
        private readonly VariableExpression _name;
        private readonly List<IExpression> _size;
        private readonly ParserType _type;

        public ArrayDeclare(VariableExpression name, ParserType type, List<IExpression> size)
        {
            _name = name;
            _type = type;
            _size = size;
        }

        void IConstructive.IfLast(IStatement kind)
        {
            if (this == kind)
                ((IConstructive) this).NoNewLine();
            else
                ((IConstructive) this).NewLine();
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

        void IStatement.Execute()
        {
            new BasicImplementation().ArrayDeclare(_name, _size, _type);
        }

        Type IStatement.CalledBy { get; set; }
    }
}