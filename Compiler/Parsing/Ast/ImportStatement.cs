using System;
using System.IO;
using Compiler.Lexical;

namespace Compiler.Parsing.Ast
{
    internal class ImportStatement : IStatement, IConstructive
    {
        private readonly string _name;
        private IStatement _statements;

        public ImportStatement(string name)
        {
            _name = name;
            _statements = null;
        }

        void IConstructive.IfLast(IStatement kind)
        {
            if (this == kind) ((IConstructive) this).NoNewLine();
            else ((IConstructive) this).NewLine();
        }

        void IConstructive.NewLine()
        {
            ((IStatement) this).Execute();
            if (((BlockStatement) _statements).IsModule) ((IConstructive) _statements).NewLine();
        }

        void IConstructive.NoNewLine()
        {
            ((IStatement) this).Execute();
            if (((BlockStatement) _statements).IsModule) ((IConstructive) _statements).NoNewLine();
        }

        void IStatement.Execute()
        {
            new BasicImplementation().ImportStatement(_name, out _statements);
        }

        Type IStatement.CalledBy { get; set; }
    }
}