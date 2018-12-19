using Compiler.Parsing.Parser;

namespace Compiler.Parsing.Ast
{
    internal class Argument
    {
        public Argument(VariableExpression name, ParserType type)
        {
            Name = name;
            Type = type;
        }

        public VariableExpression Name { get; }
        public ParserType Type { get; }
    }
}