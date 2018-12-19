using System;

namespace Compiler.Parsing.Ast
{
    internal interface IStatement
    {
        Type CalledBy { get; set; }
        void Execute();
    }
}