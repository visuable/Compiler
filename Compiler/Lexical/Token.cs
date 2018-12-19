namespace Compiler.Lexical
{
    internal class Token
    {
        public Token(TokenType type)
        {
            Value = "";
            Type = type;
        }

        public Token(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }

        public Token(string value)
        {
            Value = value;
            Type = TokenType.Default;
        }

        public TokenType Type { get; }
        public string Value { get; }

        public override string ToString()
        {
            var temp = Value + " : " + Type;
            return temp;
        }
    }

    public enum TokenType
    {
        Number,
        Id,

        For,
        While,
        Loop,

        Call,
        Return,

        Print,

        If,
        Else,

        Function,
        Procedure,

        Import,

        Int,
        String,
        Array,
        Double,
        Char,

        Default,
        Error,
        EndOfFile
    }
}