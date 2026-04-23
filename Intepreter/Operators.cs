using System;
using System.Collections.Generic;
using System.Linq;

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

        // =========================
        // Stack manipulation
        // =========================

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
            Console.WriteLine(_stack.Pop());
        }

        public void Copy()
        {
            if (_stack.Count < 1)
                throw new Exception("copy requires a count");

            int n = Convert.ToInt32(_stack.Pop());

            if (_stack.Count < n)
                throw new Exception("copy: not enough elements on stack");

            var temp = new List<object>();

            // grab top n elements
            for (int i = 0; i < n; i++)
            {
                temp.Add(_stack.Pop());
            }

            temp.Reverse(); // restore original order

            // push original + copy
            foreach (var item in temp)
            {
                _stack.Push(item);
            }

            foreach (var item in temp)
            {
                _stack.Push(item);
            }
        }

        public void Clear()
        {
            _stack.Clear();
        }

        public void Count()
        {
            _stack.Push((double)_stack.Count);
        }

        // =========================
        // Arithmetic Operators
        // =========================

        public void Mul()
        {
            var b = Convert.ToDouble(_stack.Pop());
            var a = Convert.ToDouble(_stack.Pop());
            _stack.Push(a * b);
        }

        public void Div()
        {
            var b = Convert.ToDouble(_stack.Pop());
            var a = Convert.ToDouble(_stack.Pop());

            if (b == 0)
                throw new DivideByZeroException("div by zero");

            _stack.Push(a / b);
        }

        public void IDiv()
        {
            var b = Convert.ToDouble(_stack.Pop());
            var a = Convert.ToDouble(_stack.Pop());

            if (b == 0)
                throw new DivideByZeroException("idiv by zero");

            _stack.Push((double)((int)(a / b)));
        }

        public void Mod()
        {
            var b = Convert.ToDouble(_stack.Pop());
            var a = Convert.ToDouble(_stack.Pop());

            if (b == 0)
                throw new DivideByZeroException("mod by zero");

            _stack.Push(a % b);
        }

        public void Abs()
        {
            var a = Convert.ToDouble(_stack.Pop());
            _stack.Push(Math.Abs(a));
        }

        public void Neg()
        {
            var a = Convert.ToDouble(_stack.Pop());
            _stack.Push(-a);
        }

        public void Ceiling()
        {
            var a = Convert.ToDouble(_stack.Pop());
            _stack.Push(Math.Ceiling(a));
        }

        public void Floor()
        {
            var a = Convert.ToDouble(_stack.Pop());
            _stack.Push(Math.Floor(a));
        }

        public void Round()
        {
            var a = Convert.ToDouble(_stack.Pop());
            _stack.Push(Math.Round(a));
        }

        public void Sqrt()
        {
            var a = Convert.ToDouble(_stack.Pop());

            if (a < 0)
                throw new Exception("sqrt of negative number");

            _stack.Push(Math.Sqrt(a));
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

        // =========================
        // Comparison & Boolean (Bitwise-like) Operators
        // =========================

        public void Eq()
        {
            var b = _stack.Pop();
            var a = _stack.Pop();

            _stack.Push(Equals(a, b));
        }

        public void Ne()
        {
            var b = _stack.Pop();
            var a = _stack.Pop();

            _stack.Push(!Equals(a, b));
        }

        public void Gt()
        {
            var b = Convert.ToDouble(_stack.Pop());
            var a = Convert.ToDouble(_stack.Pop());

            _stack.Push(a > b);
        }

        public void Ge()
        {
            var b = Convert.ToDouble(_stack.Pop());
            var a = Convert.ToDouble(_stack.Pop());

            _stack.Push(a >= b);
        }

        public void Lt()
        {
            var b = Convert.ToDouble(_stack.Pop());
            var a = Convert.ToDouble(_stack.Pop());

            _stack.Push(a < b);
        }

        public void Le()
        {
            var b = Convert.ToDouble(_stack.Pop());
            var a = Convert.ToDouble(_stack.Pop());

            _stack.Push(a <= b);
        }

        public void And()
        {
            var b = Convert.ToBoolean(_stack.Pop());
            var a = Convert.ToBoolean(_stack.Pop());

            _stack.Push(a && b);
        }

        public void Or()
        {
            var b = Convert.ToBoolean(_stack.Pop());
            var a = Convert.ToBoolean(_stack.Pop());

            _stack.Push(a || b);
        }

        public void Not()
        {
            var a = Convert.ToBoolean(_stack.Pop());

            _stack.Push(!a);
        }

        // =========================
        // Flow Control
        // =========================

        public void If()
        {
            var proc = _stack.Pop();
            var condition = _stack.Pop();

            if (proc is Procedure p && condition is bool b && b)
            {
                _interpreter.ExecuteProcedure(p);
            }
        }

        public void IfElse()
        {
            var elseProc = _stack.Pop();
            var ifProc = _stack.Pop();
            var condition = _stack.Pop();

            if (condition is bool b)
            {
                var selected = b ? ifProc : elseProc;

                if (selected is Procedure p)
                    _interpreter.ExecuteProcedure(p);
            }
        }

        public void Def()
        {
            var value = _stack.Pop();
            var name = _stack.Pop();

            if (name is string s)
            {
                _interpreter.Define(s, value);
                return;
            }

            throw new Exception("Invalid name for def");
        }

        public void Repeat()
        {
            var proc = _stack.Pop();
            var count = Convert.ToInt32(_stack.Pop());

            if (proc is Procedure p)
            {
                for (int i = 0; i < count; i++)
                    _interpreter.ExecuteProcedure(p);
                return;
            }

            throw new Exception("Repeat expects a procedure");
        }

        public void For()
        {
            var proc = _stack.Pop();
            var end = Convert.ToDouble(_stack.Pop());
            var start = Convert.ToDouble(_stack.Pop());

            if (proc is Procedure p)
            {
                for (double i = start; i <= end; i++)
                {
                    _stack.Push(i);
                    _interpreter.ExecuteProcedure(p);
                }
                return;
            }

            throw new Exception("for expects a procedure");
        }

        // =========================
        // Input/Output Helpers
        // =========================

        public void PrintSimple() // "="
        {
            var value = _stack.Pop();
            Console.WriteLine(FormatValue(value, simple: true));
        }

        public void PrintDetailed() // "=="
        {
            var value = _stack.Pop();
            Console.WriteLine(FormatValue(value, simple: false));
        }

        public void Print()
        {
            var value = _stack.Pop();
            Console.WriteLine(value);
        }

        // helper
        private string FormatValue(object value, bool simple)
        {
            if (value is List<Token> proc)
            {
                return simple
                    ? "{ ... }"
                    : "{ " + string.Join(" ", proc.Select(t => t.Value)) + " }";
            }

            return value?.ToString() ?? "null";
        }

        // =========================
        // String Operators
        // =========================

        // (hello) length → 5
        public void Length()
        {
            var obj = _stack.Pop();

            if (obj is string s)
            {
                _stack.Push((double)s.Length);
                return;
            }

            if (obj is Dictionary<string, object> d)
            {
                _stack.Push((double)d.Count);
                return;
            }

            throw new Exception("length expects string or dictionary");
        }

        // (hello) 1 get → 101  (ASCII of 'e')
        public void Get()
        {
            var index = Convert.ToInt32(_stack.Pop());
            var str = _stack.Pop() as string
                ?? throw new Exception("get expects a string");

            if (index < 0 || index >= str.Length)
                throw new Exception("get: index out of range");

            _stack.Push((double)(int)str[index]); // ASCII value
        }

        // (hello) 1 3 getinterval → "ell"
        public void GetInterval()
        {
            var count = Convert.ToInt32(_stack.Pop());
            var index = Convert.ToInt32(_stack.Pop());
            var str = _stack.Pop() as string
                ?? throw new Exception("getinterval expects a string");

            if (index < 0 || index + count > str.Length)
                throw new Exception("getinterval: out of range");

            _stack.Push(str.Substring(index, count));
        }

        // (hello) 1 (yy) putinterval → "hyylo"
        public void PutInterval()
        {
            var replacement = _stack.Pop() as string
                ?? throw new Exception("putinterval expects a string replacement");

            var index = Convert.ToInt32(_stack.Pop());
            var original = _stack.Pop() as string
                ?? throw new Exception("putinterval expects a string");

            if (index < 0 || index + replacement.Length > original.Length)
                throw new Exception("putinterval: out of range");

            var result =
                original.Substring(0, index) +
                replacement +
                original.Substring(index + replacement.Length);

            _stack.Push(result);
        }

        // =========================
        // Dictionary Operators
        // =========================

        // n dict → creates empty dictionary
        public void Dict()
        {
            var size = Convert.ToInt32(_stack.Pop());

            var dict = new Dictionary<string, object>(size);
            _stack.Push(dict);
        }

        // dict length → number of entries
        public void DictLength()
        {
            var dict = _stack.Pop() as Dictionary<string, object>
                ?? throw new Exception("length expects a dictionary");

            _stack.Push((double)dict.Count);
        }

        // dict maxlength → capacity (approximate)
        public void MaxLength()
        {
            var dict = _stack.Pop() as Dictionary<string, object>
                ?? throw new Exception("maxlength expects a dictionary");

            _stack.Push((double)dict.Count); // C# doesn't expose capacity cleanly
        }

        // dict begin → push onto dict stack
        public void Begin()
        {
            var dict = _stack.Pop() as Dictionary<string, object>
                ?? throw new Exception("begin expects a dictionary");

            _interpreter.PushDict(dict);
        }

        // end → pop dictionary scope
        public void End()
        {
            _interpreter.PopDict();
        }

        public void CurrentDict()
        {
            _stack.Push(_interpreter.CurrentDict);
        }
    }
}