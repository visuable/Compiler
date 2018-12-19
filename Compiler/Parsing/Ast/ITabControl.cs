namespace Compiler.Parsing.Ast
{
    internal interface ITabControl
    {
        void WithoutFrontSpace();
        void WithFrontSpace();
        void WithBackSpace();
        void WithFrontAndBackSpace();
    }
}