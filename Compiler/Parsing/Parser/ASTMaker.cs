using System.Collections.Generic;
using Compiler.Lexical;
using Compiler.Parsing.Ast;

namespace Compiler.Parsing.Parser
{
    internal class ASTMaker
    {
        private readonly Lexer _analyzer;
        private readonly List<IStatement> _statements;
        private Token _currentToken;

        public ASTMaker(Lexer analyzer)
        {
            _analyzer = analyzer;
            _currentToken = _analyzer.GetNext();
            _statements = new List<IStatement>();
            ParseErrors = new Stack<ParserException>();
        }

        public Stack<ParserException> ParseErrors { get; private set; }

        public BlockStatement ParseTokens()
        {
            while (true)
            {
                if (Match(TokenType.EndOfFile)) return new BlockStatement(_statements);
                _statements.Add(Statement());
            }
        }

        private IStatement Statement()
        {
            if (LookMatch(TokenType.Int) | LookMatch(TokenType.String) | LookMatch(TokenType.Id))
                return AssignOrDeclareStatement();
            if (Match(TokenType.Array)) return DeclareArray();
            if (Match(TokenType.Call)) return new ExpressionEvalStatement(CallMethodExpression());
            if (Match(TokenType.Print)) return PrintStatement();
            if (Match(TokenType.Function)) return FunctionDeclare();
            if (Match(TokenType.Procedure)) return ProcedureDeclare();
            if (Match(TokenType.Return)) return ReturnStatement();
            if (Match(TokenType.For)) return ForStatement();
            if (Match(TokenType.If)) return IfElseStatement();
            if (Match(TokenType.Import)) return ImportStatement();
            if (LookMatch("{")) return BlockStatements();
            return Match(TokenType.While) ? WhileStatement() : ExpressionToStatement();
        }

        private IStatement ImportStatement()
        {
            var name = Consume(TokenType.String, ParserError.UnknownSyntax).Value;
            return new ImportStatement(name);
        }

        private IStatement DeclareArray()
        {
            var name = Consume(TokenType.Id, ParserError.UnknownSyntax);
            var type = SelectTypeOf();
            var sizes = GetArraySizes();
            return new ArrayDeclare(new VariableExpression(name.Value), type, sizes);
        }

        private List<IExpression> GetArraySizes()
        {
            var list = new List<IExpression>();
            while (LookMatch("["))
            {
                Consume("[", ParserError.UnknownSyntax);
                list.Add(GetAdditionalExpression());
                Consume("]", ParserError.UnknownSyntax);
            }

            return list;
        }
        private IStatement IfElseStatement()
        {
            var condition = GetConditionalExpression();
            IStatement executionBlock = BlockStatements();
            if (Match(TokenType.Else))
            {
                IStatement elseBlock = BlockStatements();
                return new IfStatement(condition, executionBlock, elseBlock, true);
            }

            return new IfStatement(condition, executionBlock, null, false);
        }

        private IExpression CallMethodExpression()
        {
            var name = Consume(TokenType.Id, ParserError.UnknownSyntax);
            var arguments = GetCallArguments();
            return new CallFunctionExpression(new VariableExpression(name.Value), arguments);
        }

        private IStatement AssignOrDeclareStatement()
        {
            if (Match(TokenType.Int)) return DeclareIntVariable();
            return Match(TokenType.String) ? DeclareStringVariable() : AssignVar();
        }

        private IStatement WhileStatement()
        {
            var condition = GetConditionalExpression();
            IStatement statements = BlockStatements();
            return new WhileStatement(condition, statements);
        }

        private IStatement ForStatement()
        {
            try
            {
                var assign = AssignOrDeclareStatement();
                Consume(";", ParserError.ForSplitter);
                var step = GetConditionalExpression();
                return assign is AssignStatement
                    ? new ForStatement(assign, new ExpressionEvalStatement(step), BlockStatements())
                    : null;
            }
            catch
            {
                Consume(TokenType.EndOfFile, ParserError.ForStatementSyntaxError);
            }

            return null;
        }

        private BlockStatement BlockStatements()
        {
            var statements = new List<IStatement>();
            if (Match("{"))
                while (!Match("}"))
                {
                    if (Match(TokenType.EndOfFile)) Consume(TokenType.Error, ParserError.UnknownSyntax);
                    statements.Add(Statement());
                }

            return new BlockStatement(statements);
        }

        private IStatement ReturnStatement()
        {
            return !Match(".") ? new ReturnStatement(GetConditionalExpression()) : new ReturnStatement(null);
        }

        private IStatement FunctionDeclare()
        {
            var name = Consume(TokenType.Id, ParserError.UnknownSyntax);
            var arguments = ParseDeclareArguments();
            var type = SelectTypeOf();
            IStatement statements = BlockStatements();
            return new FunctionDeclare(name.Value, arguments, statements, type);
        }

        private IStatement ProcedureDeclare()
        {
            var name = Consume(TokenType.Id, ParserError.UnknownSyntax);
            var arguments = ParseDeclareArguments();
            IStatement statements = BlockStatements();
            return new ProcedureDeclare(new VariableExpression(name.Value), arguments, statements);
        }

        private IStatement ParseDeclareArguments()
        {
            var arguments = new List<Argument>();
            Consume("(", ParserError.ParenCountMismatch);
            arguments = GetDeclareArguments(arguments);
            Consume(")", ParserError.ParenCountMismatch);
            return new DeclareArgumentBlock(arguments);
        }

        private List<Argument> GetDeclareArguments(List<Argument> arguments)
        {
            if (Match(TokenType.Int))
            {
                var name = Consume(TokenType.Id, ParserError.UnknownSyntax).Value;
                arguments.Add(new Argument(new VariableExpression(name), ParserType.Integer));
                if (Match(",")) arguments = GetDeclareArguments(arguments);
            }
            else if (Match(TokenType.String))
            {
                var name = Consume(TokenType.Id, ParserError.UnknownSyntax).Value;
                arguments.Add(new Argument(new VariableExpression(name), ParserType.String));
                if (Match(",")) arguments = GetDeclareArguments(arguments);
            }

            return arguments;
        }

        private IStatement PrintStatement()
        {
            var expression = GetConditionalExpression();
            return new PrintStatement(expression);
        }

        private IStatement AssignVar()
        {
            var left = GetPrimaryExpression();
            if (!(left is VariableExpression) && !(left is ArrayUseExpression))
                return new ExpressionEvalStatement(left);
            if (LookMatch("="))
            {
                Consume("=", ParserError.UnknownSyntax);
                var right = GetConditionalExpression();
                return new AssignStatement(left, right);
            }

            return new ExpressionEvalStatement(left);
        }

        private IStatement ExpressionToStatement()
        {
            return new ExpressionEvalStatement(GetConditionalExpression());
        }

        private IStatement DeclareStringVariable()
        {
            Match(TokenType.String);
            var name = Consume(TokenType.Id, ParserError.UnknownSyntax);
            if (!Match("=")) return new DeclareStatement(new VariableExpression(name.Value), ParserType.String);
            var right = GetConditionalExpression();
            return new DeclareAndAssignStatement(ParserType.String, new VariableExpression(name.Value), right);
        }

        private IStatement DeclareIntVariable()
        {
            Match(TokenType.Int);
            var name = Consume(TokenType.Id, ParserError.UnknownSyntax);
            if (Match("="))
            {
                var right = GetConditionalExpression();
                return new DeclareAndAssignStatement(ParserType.Integer, new VariableExpression(name.Value), right);
            }

            return new DeclareStatement(new VariableExpression(name.Value), ParserType.Integer);
        }

        private IExpression GetConditionalExpression()
        {
            var left = GetAdditionalExpression();
            while (true)
            {
                if (Match("="))
                {
                    left = new BinaryExpression(left, GetConditionalExpression(), Operation.Equal);
                    continue;
                }

                if (Match("<"))
                {
                    if (Match("=")) return new BinaryExpression(left, GetConditionalExpression(), Operation.LowOrEqual);
                    return new BinaryExpression(left, GetConditionalExpression(), Operation.Low);
                }

                if (Match(">"))
                {
                    left = Match("=")
                        ? new BinaryExpression(left, GetConditionalExpression(), Operation.GreaterOrEqual)
                        : new BinaryExpression(left, GetConditionalExpression(), Operation.Greater);
                    continue;
                }

                if (!LookMatch("!")) return left;
                var save = _currentToken;
                Match("!");
                if (Match("="))
                {
                    left = new BinaryExpression(left, GetConditionalExpression(), Operation.NotEqual);
                }
                else
                {
                    _currentToken = save;
                    return left;
                }

                return left;
            }
        }

        private IExpression GetAdditionalExpression()
        {
            var left = GetMultiplyExpression();
            while (true)
            {
                if (Match("+"))
                {
                    left = new BinaryExpression(left, GetMultiplyExpression(), Operation.Addition);
                    continue;
                }

                if (Match("-"))
                {
                    left = new BinaryExpression(left, GetMultiplyExpression(), Operation.Subtract);
                    continue;
                }

                return left;
            }
        }

        private IExpression GetMultiplyExpression()
        {
            var left = GetPrimaryExpression();
            while (true)
            {
                if (Match("*"))
                {
                    left = new BinaryExpression(left, GetPrimaryExpression(), Operation.Times);
                    continue;
                }

                if (Match("/"))
                {
                    left = new BinaryExpression(left, GetPrimaryExpression(), Operation.Divide);
                    continue;
                }

                return left;
            }
        }

        private IExpression GetPrimaryExpression()
        {
            if (LookMatch(TokenType.Number))
            {
                var value = Consume(TokenType.Number, ParserError.UnknownSyntax);
                return new NumberConstantExpression(value.Value);
            }

            if (LookMatch(TokenType.String))
            {
                var value = Consume(TokenType.String, ParserError.UnknownSyntax);
                return new TextConstantExpression(value.Value);
            }

            if (LookMatch(TokenType.Id))
            {
                var name = Consume(TokenType.Id, ParserError.UnknownFactor);
                if (!LookMatch("[")) return new VariableExpression(name.Value);
                var index = GetArraySizes();
                return new ArrayUseExpression(new VariableExpression(name.Value), index);
            }

            if (Match(TokenType.Call)) return CallMethodExpression();
            if (Match("("))
            {
                var expression = GetAdditionalExpression();
                Consume(")", ParserError.ParenCountMismatch);
                return new ParenExpression(expression);
            }

            Consume(TokenType.EndOfFile, ParserError.UnknownFactor);
            return null;
        }

        private ParserType SelectTypeOf()
        {
            if (Match(TokenType.Int)) return ParserType.Integer;
            return Match(TokenType.String) ? ParserType.String : ParserType.Undefined;
        }

        private IStatement GetCallArguments()
        {
            var arguments = new List<CallArgument>();
            Consume("(", ParserError.ParenCountMismatch);
            while (!Match(")") && !Match(TokenType.EndOfFile))
            {
                arguments.Add(new CallArgument(GetConditionalExpression()));
                if (!Match(","))
                {
                    Consume(")", ParserError.ParenCountMismatch);
                    break;
                }
            }

            return new ArgumentBlock(arguments);
        }

        private bool LookMatch(string value)
        {
            return _currentToken.Value == value;
        }

        private bool LookMatch(TokenType type)
        {
            return _currentToken.Type == type;
        }

        private bool Match(TokenType type)
        {
            if (_currentToken.Type != type) return false;
            Next();
            return true;
        }

        private bool Match(string value)
        {
            if (_currentToken.Value != value) return false;
            Next();
            return true;
        }

        private Token Consume(TokenType type, ParserError error)
        {
            if (_currentToken.Type != type) ParseErrors.Push(new ParserException(error, _currentToken));
            var temp = _currentToken;
            Match(type);
            return temp;
        }

        private void Consume(string value, ParserError error)
        {
            if (_currentToken.Value != value) ParseErrors.Push(new ParserException(error, _currentToken));
            Next();
        }

        private void Next()
        {
            _currentToken = _analyzer.GetNext();
        }
    }
}