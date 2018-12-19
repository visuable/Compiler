namespace Compiler.Parsing.Ast
{
    internal class CallArgument
    {
        public CallArgument(IExpression value)
        {
            Value = value;
        }

        public IExpression Value { get; }
    }
}