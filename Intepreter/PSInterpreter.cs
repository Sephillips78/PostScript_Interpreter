using Interpreter;
using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class PSInterpreter
    {
        private readonly PSStack _stack = new();
        private readonly Tokenizer _tokenizer = new();
        private readonly Dictionary<string, Action> _operators;
        internal readonly Dictionary<string, Object> _env = new();

        public PSInterpreter()
        {
            var ops = new Operators(_stack, this);

            _operators = new Dictionary<string, Action>
            {
                { "add", ops.Add },
                { "dup", ops.Dup },
                { "exch", ops.Exch },
                { "pop", ops.Pop },
                { "if", ops.If },
                { "ifelse", ops.IfElse },
                { "def", ops.Def },
                { "repeat", ops.Repeat },
                { "sub", ops.Sub },
                { "for", ops.For },
                { "print", ops.Print }
            };
        }

        public void Execute(string input)
        {
            var tokens = _tokenizer.Tokenize(input);

            foreach (var token in tokens)
            {
                ExecuteToken(token);
            }
        }

        public void ExecuteToken(object token)
        {
            if (token is Token t)
            {
                switch (t.Type)
                {
                    // -------------------
                    // VALUES → push directly
                    // -------------------
                    case Token.TokenType.Number:
                    case Token.TokenType.Boolean:
                    case Token.TokenType.String:
                        _stack.Push(t.Value);
                        return;

                    // -------------------
                    // NAME (/x in PostScript)
                    // -------------------
                    case Token.TokenType.Name:
                        _stack.Push(t.Value);
                        return;

                    // -------------------
                    // PROCEDURE { ... }
                    // -------------------
                    case Token.TokenType.Procedure:
                        _stack.Push(t.Value);
                        return;

                    // -------------------
                    // OPERATORS / VARIABLES / FUNCTIONS
                    // -------------------
                    case Token.TokenType.Operator:
                        ExecuteOperator((string)t.Value);
                        return;
                }
            }

            throw new Exception($"Invalid token: {token}");
        }

        public object Pop()
        {
            return _stack.Pop();
        }

        public void ExecuteProcedure(List<Token> proc)
        {
            foreach (var token in proc)
            {
                ExecuteToken(token);
            }
        }

        public void ExecuteOperator(string op)
        {
            // 1. variable lookup FIRST
            if (_env.ContainsKey(op))
            {
                var value = _env[op];

                if (value is List<Token> proc)
                {
                    ExecuteProcedure(proc);
                    return;
                }

                _stack.Push(value);
                return;
            }

            // 2. built-in operators
            if (_operators.ContainsKey(op))
            {
                _operators[op]();
                return;
            }

            throw new Exception($"Unknown operator or name: {op}");
        }
    }
}