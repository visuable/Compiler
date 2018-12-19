using System;
using Compiler.Parsing.Parser;

namespace Compiler.Parsing.Ast
{
    internal class DeclareStatement : IStatement, IConstructive
    {
        private readonly ParserType _type;
        private readonly VariableExpression _value;

        public DeclareStatement(VariableExpression value, ParserType type)
        {
            _value = value;
            _type = type;
        }

        void IConstructive.NoNewLine()
        {
            Execution();
        }

        void IConstructive.NewLine()
        {
            Execution();
            Console.WriteLine();
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

        private void Execution()
        {
            if (_type == ParserType.Integer)
                PrintIntDeclare();
            else
                PrintTextDeclare();
        }

        private void PrintIntDeclare()
        {
            Console.Write("Dim");
            ((ITabControl) _value).WithFrontAndBackSpace();
            Console.Write("As Integer ");
        }

        private void PrintTextDeclare()
        {
            Console.Write("Dim");
            ((ITabControl) _value).WithFrontAndBackSpace();
            Console.Write("As String ");
        }
    }
}