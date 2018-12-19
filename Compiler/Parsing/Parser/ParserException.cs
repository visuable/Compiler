using System;
using Compiler.Lexical;

namespace Compiler.Parsing.Parser
{ 
    internal class ParserException : Exception
    {
        private readonly ParserError _error;
        private readonly Token _token;

        public ParserException(ParserError error, Token token) : base("Parsing error in: ")
        {
            _error = error;
            _token = token;
            Handle();
        }

        private void Handle()
        {
            switch (_error)
            {
                case ParserError.ArgumentMismatch:
                    throw new Exception("The number of function arguments does not match the function description");
                case ParserError.ParenCountMismatch:
                    throw new Exception("The number of paren count does not match even number");
                case ParserError.UndefinedType:
                    throw new Exception("Undefined Type in: description function, declare of variable");
                case ParserError.ForSplitter:
                    throw new Exception("Does not find for splitter ';'");
                case ParserError.UnknownFactor:
                    throw new Exception("Invalid factor expression, correct syntax");
                case ParserError.ForStatementSyntaxError:
                    throw new Exception("Invalid declaration in for statement");
                case ParserError.ProcedureReturnValue:
                    throw new Exception("Procedure statement does not return a value");
                default:
                    throw new Exception("Invalid syntax  TOKEN [ " + _token + " ]");
            }
        }
    }

    internal enum ParserError
    {
        ArgumentMismatch,
        ParenCountMismatch,
        ProcedureReturnValue,

        UndefinedType,

        UnknownFactor,

        ForStatementSyntaxError,
        ForSplitter,

        UnknownSyntax
    }
}