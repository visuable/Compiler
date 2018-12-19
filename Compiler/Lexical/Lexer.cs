using System.Collections.Generic;
using System.Text;

namespace Compiler.Lexical
{
    internal class Lexer
    {
        public const char EndOfFile = '\0';

        private readonly string _input;
        private char _current;
        private int _index;

        public Lexer(string input)
        {
            _input = input;
            _current = ' ';
            _index = 0;
            Tokens = new List<Token>();
        }

        public List<Token> Tokens { get; }

        private void Slide()
        {
            if (_index < _input.Length)
            {
                _current = _input[_index];
                _index++;
                return;
            }

            _current = EndOfFile;
        }

        public Token GetNext()
        {
            while (_current != EndOfFile)
                if (char.IsWhiteSpace(_current))
                {
                    Slide();
                }
                else if (char.IsLetter(_current))
                {
                    var temp = ParseWord();
                    Tokens.Add(temp);
                    return temp;
                }
                else if (char.IsDigit(_current))
                {
                    var temp = ParseNumber();
                    Tokens.Add(temp);
                    return temp;
                }
                else if (_current == '\'')
                {
                    var temp = ParseText();
                    Tokens.Add(temp);
                    return temp;
                }
                else
                {
                    var temp = ParseChar();
                    Tokens.Add(temp);
                    return temp;
                }

            return new Token(TokenType.EndOfFile);
        }

        private Token ParseText()
        {
            Slide();
            var sb = new StringBuilder();
            while (_current != EndOfFile && _current != '\'')
            {
                sb.Append(_current);
                Slide();
            }

            Slide();
            return new Token(sb.ToString(), TokenType.String);
        }

        private Token ParseWord()
        {
            var sb = new StringBuilder();
            while (char.IsLetterOrDigit(_current))
            {
                sb.Append(_current);
                Slide();
            }

            if (sb.ToString().Equals("for")) return new Token(TokenType.For);
            if (sb.ToString().Equals("while")) return new Token(TokenType.While);
            if (sb.ToString().Equals("ret")) return new Token(TokenType.Return);
            if (sb.ToString().Equals("str")) return new Token(TokenType.String);
            if (sb.ToString().Equals("int")) return new Token(TokenType.Int);
            if (sb.ToString().Equals("show")) return new Token(TokenType.Print);
            if (sb.ToString().Equals("proc")) return new Token(TokenType.Procedure);
            if (sb.ToString().Equals("fun")) return new Token(TokenType.Function);
            if (sb.ToString().Equals("if")) return new Token(TokenType.If);
            if (sb.ToString().Equals("else")) return new Token(TokenType.Else);
            if (sb.ToString().Equals("call")) return new Token(TokenType.Call);
            if (sb.ToString().Equals("arr")) return new Token(TokenType.Array);
            if (sb.ToString().Equals("excl")) return new Token(TokenType.Import);
            if (sb.ToString().Equals("db")) return new Token(TokenType.Double);
            if (sb.ToString().Equals("ch")) return new Token(TokenType.Char);
            if (sb.ToString().Equals("loop")) return new Token(TokenType.Loop);
            return new Token(sb.ToString(), TokenType.Id);
        }

        private Token ParseNumber()
        {
            var sb = new StringBuilder();
            while (char.IsDigit(_current))
            {
                sb.Append(_current);
                Slide();
            }

            return new Token(sb.ToString(), TokenType.Number);
        }

        private Token ParseChar()
        {
            var sb = new StringBuilder();
            if ("<>={}+-*/();,[].!".Contains(_current.ToString()))
            {
                sb.Append(_current);
                Slide();
            }

            return new Token(sb.ToString());
        }
    }
}