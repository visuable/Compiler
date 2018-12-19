using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Parsing.Parser;

namespace Compiler.Parsing.Ast
{
    interface Gen
    {
        void ArgumentBlock(List<CallArgument> arguments);
        void ArrayDeclare(VariableExpression _name, List<IExpression> _size, ParserType _type);
        void ArrayUseExpression(List<IExpression> _offset, VariableExpression _variable);
        void AssignStatement(IExpression _right);
        void BinaryExpression(Operation _operation);
        void BlockStatement(List<IStatement> _statements, IStatement _last, object ths);
        void DeclareAndAssignStatement(ParserType _type, VariableExpression _name, IExpression _right);
        void WhileStatement(IExpression _condition, IStatement _statements);
        void ReturnStatement(IExpression _right, ReturnStatement obj);
        void ProcedureDeclare(IStatement _arguments, VariableExpression _name, IStatement _statements);
        void PrintStatement(IExpression _expression);
        void ImportStatement(string _name, out IStatement _statements);
        void FunctionDeclare(IStatement _arguments, string _name, IStatement _statements, ParserType _type);
        void ForStatement(IStatement _assigns, IStatement _statements, IStatement _step);

    }
}
