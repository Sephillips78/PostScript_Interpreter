using System;

namespace Interpreter
{
    public class Operators
    {
        private readonly PSStack _stack;
        private readonly PSInterpreter _interpreter;

        public Operators(PSStack stack, PSInterpreter interpreter)
        {
            _stack = stack;
            _interpreter = interpreter;
        }

        public void Dup()
        {

            if (_stack.Count < 1)
                throw new Exception("Stack underflow in dup");

            var val = _stack.Peek();
            _stack.Push(val);
        }

        public void Exch()
        {
            var a = _stack.Pop();
            var b = _stack.Pop();

            _stack.Push(a);
            _stack.Push(b);
        }

        public void Pop()
        {
            _stack.Pop();
        }

        public void Add()
        {
            var b = Convert.ToDouble(_stack.Pop());
            var a = Convert.ToDouble(_stack.Pop());

            _stack.Push(a + b);
        }

        public void Sub()
        {
            if (_stack.Count < 2)
                throw new Exception("Stack underflow in sub");

            var b = Convert.ToDouble(_stack.Pop());
            var a = Convert.ToDouble(_stack.Pop());

            _stack.Push(a - b);
        }

        public void If()
        {
            var proc = _stack.Pop();
            var condition = _stack.Pop();

            if (condition is bool b && b)
            {
                _interpreter.ExecuteProcedure((List<Token>)proc);
            }
        }

        public void IfElse()
        {
            var elseProc = _stack.Pop();
            var ifProc = _stack.Pop();
            var condition = _stack.Pop();

            if (condition is bool b)
            {
                var procToRun = b ? ifProc : elseProc;
                _interpreter.ExecuteProcedure((List<Token>)procToRun);
            }
        }

        public void Def()
        {
            var value = _stack.Pop();
            var name = _stack.Pop();

            if (name is string s)
            {
                _interpreter._env[s] = value;
                return;
            }

            throw new Exception("Invalid name for def");
        }

        public void Repeat()
        {

            var proc = _stack.Pop();

            var count = Convert.ToInt32(_stack.Pop());

            if (proc is List<Token> body)
            {
                for (int i = 0; i < count; i++)
                {
                    _interpreter.ExecuteProcedure(body);
                }
            }
            else
            {
                throw new Exception("Repeat expects a procedure");
            }
        }

        public void For()
        {
            var proc = _stack.Pop();
            var end = Convert.ToDouble(_stack.Pop());
            var start = Convert.ToDouble(_stack.Pop());

            if (proc is List<Token> body)
            {
                for (double i = start; i <= end; i++)
                {
                    _stack.Push(i); // loop variable
                    _interpreter.ExecuteProcedure(body);
                }
            }
            else
            {
                throw new Exception("for expects a procedure");
            }
        }

        public void Print()
        {
            var value = _stack.Pop();
            Console.WriteLine(value);
        }
    }
}