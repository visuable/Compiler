namespace Compiler.Parsing.Ast
{
    internal interface IConstructive
    {
        void NoNewLine();
        void NewLine();
        void IfLast(IStatement kind);
    }
}