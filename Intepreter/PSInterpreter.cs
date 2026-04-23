using Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interpreter
{
    public class PSInterpreter
    {
        public enum ScopeMode
        {
            Dynamic,
            Static
        }

        private readonly ScopeMode _scopeMode;
        private readonly PSStack _stack = new();
        private readonly Tokenizer _tokenizer = new();
        private readonly Dictionary<string, Action> _operators;
        private readonly Stack<Dictionary<string, object>> _dictStack = new();
        private readonly Dictionary<string, object> _globalDict;

        public PSInterpreter(ScopeMode mode = ScopeMode.Dynamic)
        {
            _scopeMode = mode;

            var ops = new Operators(_stack, this);
            _dictStack.Push(new Dictionary<string, object>());

            _globalDict = _dictStack.Peek();

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
                { "print", ops.Print },
                { "copy", ops.Copy },
                { "clear", ops.Clear },
                { "count", ops.Count },
                { "mul", ops.Mul },
                { "div", ops.Div },
                { "idiv", ops.IDiv },
                { "mod", ops.Mod },
                { "abs", ops.Abs },
                { "neg", ops.Neg },
                { "ceiling", ops.Ceiling },
                { "floor", ops.Floor },
                { "round", ops.Round },
                { "sqrt", ops.Sqrt },
                { "eq", ops.Eq },
                { "ne", ops.Ne },
                { "gt", ops.Gt },
                { "ge", ops.Ge },
                { "lt", ops.Lt },
                { "le", ops.Le },
                { "and", ops.And },
                { "or", ops.Or },
                { "not", ops.Not },
                { "=", ops.PrintSimple },
                { "==", ops.PrintDetailed },
                { "length", ops.Length },
                { "get", ops.Get },
                { "getinterval", ops.GetInterval },
                { "putinterval", ops.PutInterval },
                { "dict", ops.Dict },
                { "maxlength", ops.MaxLength },
                { "begin", ops.Begin },
                { "end", ops.End },
                { "currentdict", ops.CurrentDict },
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
                        {
                            var procTokens = (List<Token>)t.Value;

                            Dictionary<string, object>? captured = null;

                            if (_scopeMode == ScopeMode.Static)
                            {
                                captured = new Dictionary<string, object>(CurrentDict);
                            }

                            var proc = new Procedure(procTokens, captured);
                            _stack.Push(proc);
                            return;
                        }

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

        public void ExecuteProcedure(Procedure proc)
        {
            if (_scopeMode == ScopeMode.Static && proc.CapturedEnv != null)
            {
                var previous = _dictStack.Peek();

                try
                {
                    _dictStack.Push(proc.CapturedEnv);

                    foreach (var token in proc.Body)
                        ExecuteToken(token);
                }
                finally
                {
                    _dictStack.Pop();
                }

                return;
            }

            foreach (var token in proc.Body)
                ExecuteToken(token);
        }

        public void ExecuteOperator(string op)
        {
            // 1. dictionary lookup first (variables, procedures)
            if (TryLookup(op, out var value))
            {
                if (value is Procedure proc)
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

        /// <summary>
        /// ############## DICTIONARY STACK HELPER FUNCTIONS ###################
        /// </summary>
        public Dictionary<string, object> CurrentDict => _dictStack.Peek();

        public void Define(string name, object value)
        {
            if (_scopeMode == ScopeMode.Static && value is Procedure proc)
            {
                _globalDict[name] = proc;   // survive end
                Console.WriteLine("Boom");
                return;
            }

            Console.WriteLine("Still");
            CurrentDict[name] = value;
        }

        public bool TryLookup(string name, out object value)
        {
            foreach (var dict in _dictStack)
            {
                if (dict.ContainsKey(name))
                {
                    value = dict[name];
                    return true;
                }
            }

            value = null!;
            return false;
        }

        public void PushDict(Dictionary<string, object> dict)
        {
            _dictStack.Push(dict);
        }

        public void PopDict()
        {
            if (_dictStack.Count <= 1)
                throw new Exception("Cannot pop global dictionary");

            _dictStack.Pop();
        }
    }
}