using Compiler.Lexical;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Compiler.Parsing.Parser;

namespace Compiler.Parsing.Ast
{
    class BasicImplementation : Gen
    {
        public void ArgumentBlock(List<CallArgument> arguments)
        {
            CallArgument last = null;
            if (arguments.Count > 0) last = arguments.Last();
            Console.Write("(");
            foreach (var argument in arguments)
            {
                ((ITabControl)argument.Value).WithFrontSpace();
                if (last != null && last != argument) Console.Write(", ");
            }

            Console.Write(") ");
        }

        public void ArrayDeclare(VariableExpression _name, List<IExpression> _size, ParserType _type)
        {
            Console.Write("Dim As ");
            switch (_type)
            {
                case ParserType.Double:
                    Console.Write("Double");
                    break;
                case ParserType.Integer:
                    Console.Write("Integer");
                    break;
                case ParserType.String:
                    Console.Write("String");
                    break;
            }

            ((ITabControl)_name).WithFrontSpace();
            Console.Write("(");
            var last = _size.Last();
            var first = _size.First();
            foreach (var size in _size)
            {
                if (size == first)
                    ((ITabControl)size).WithoutFrontSpace();
                else
                    ((ITabControl)size).WithFrontSpace();
                if (size != last) Console.Write(",");
            }
            Console.Write(")");
        }

        public void ArrayUseExpression(List<IExpression> _offset, VariableExpression _variable)
        {
            Console.Write("(");
            var last = _offset.Last();
            var first = _offset.First();
            foreach (var size in _offset)
            {
                if (size == first)
                    ((ITabControl)size).WithoutFrontSpace();
                else
                    ((ITabControl)size).WithFrontSpace();
                if (size != last) Console.Write(",");
            }
            Console.Write(")");
        }

        public void AssignStatement(IExpression _right)
        {
            Console.Write("=");
            ((ITabControl)_right).WithFrontSpace();
        }

        public void BinaryExpression(Operation _operation)
        {
            switch (_operation)
            {
                case Ast.Operation.Addition:
                    Console.Write("+");
                    break;
                case Ast.Operation.Subtract:
                    Console.Write("-");
                    break;
                case Ast.Operation.Divide:
                    Console.Write("/");
                    break;
                case Ast.Operation.Times:
                    Console.Write("*");
                    break;
                case Ast.Operation.Equal:
                    Console.Write("=");
                    break;
                case Ast.Operation.NotEqual:
                    Console.Write("<>");
                    break;
                case Ast.Operation.Low:
                    Console.Write("<");
                    break;
                case Ast.Operation.Greater:
                    Console.Write(">");
                    break;
                case Ast.Operation.LowOrEqual:
                    Console.Write("<=");
                    break;
                case Ast.Operation.GreaterOrEqual:
                    Console.Write(">=");
                    break;
            }
        }

        public void BlockStatement(List<IStatement> _statements, IStatement _last, object ths)
        {
            foreach (var statement in _statements)
            {
                statement.CalledBy = ((IStatement)ths).CalledBy == null ? typeof(BlockStatement) : ((IStatement)ths).CalledBy;
                ((IConstructive)statement).IfLast(_last);
            }
        }

        public void DeclareAndAssignStatement(ParserType _type, VariableExpression _name, IExpression _right)
        {
            Console.Write("Dim");
            ((ITabControl)_name).WithFrontAndBackSpace();
            if (_type == ParserType.Integer)
            {
                Console.Write("As Integer ");
            }
            else if (_type == ParserType.String)
            {
                Console.Write("As String ");
            }
            Console.Write("=");
            ((ITabControl)_right).WithFrontSpace();
        }

        public void WhileStatement(IExpression _condition, IStatement _statements)
        {
            Console.Write("While");
            ((ITabControl)_condition).WithFrontSpace();
            Console.WriteLine();
            ((IConstructive)_statements).NewLine();
            Console.WriteLine("Wend");
        }

        public void ReturnStatement(IExpression _right, ReturnStatement obj)
        {
            if (_right == null)
            {
                Console.Write("Return");
            }
            else
            {
                if (((IStatement)obj).CalledBy == typeof(ProcedureDeclare))
                    throw new ParserException(ParserError.ProcedureReturnValue, null);
                Console.Write("Return");
                ((ITabControl)_right).WithFrontSpace();
            }
        }

        public void ProcedureDeclare(IStatement _arguments, VariableExpression _name, IStatement _statements)
        {
            _statements.CalledBy = typeof(ProcedureDeclare);
            Console.Write("Sub");
            ((ITabControl)_name).WithFrontSpace();
            ((IConstructive)_arguments).NewLine();
            ((IConstructive)_statements).NewLine();
            Console.WriteLine("End Sub");
        }

        public void PrintStatement(IExpression _expression)
        {
            Console.Write("Print");
            _expression.Evaluate();
        }

        public void ImportStatement(string _name, out IStatement _statements)
        {
            var text = File.ReadAllText(_name);
            var lexer = new Lexer(text);
            var parser = new ASTMaker(lexer);
            _statements = parser.ParseTokens();
        }

        public void FunctionDeclare(IStatement _arguments, string _name, IStatement _statements, ParserType _type)
        {
            _statements.CalledBy = typeof(FunctionDeclare);
            Console.Write("Function " + _name);
            ((IConstructive)_arguments).NoNewLine();
            if (_type == ParserType.Integer)
                Console.WriteLine(" As Integer");
            else if (_type == ParserType.String)
                Console.WriteLine(" As String");
            else if (_type == ParserType.Undefined) throw new Exception("Undefined function Type");

            ((IConstructive)_statements).NewLine();
            Console.WriteLine("End Function");
        }

        public void ForStatement(IStatement _assigns, IStatement _statements, IStatement _step)
        {
            Console.Write("For");
            Console.Write("To");
            ((IConstructive)_step).NewLine();
            ((IConstructive)_statements).NewLine();
            Console.WriteLine();
            Console.WriteLine("Next");
            Console.WriteLine("End");
        }
    }
}
