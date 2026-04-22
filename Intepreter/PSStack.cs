using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class PSStack
    {
        private readonly Stack<object> _stack = new();

        public void Push(object value)
        {
            _stack.Push(value);
        }

        public object Pop()
        {
            if (_stack.Count == 0)
                throw new InvalidOperationException("Stack underflow");

            return _stack.Pop();
        }

        public object Peek()
        {
            if (_stack.Count == 0)
                throw new InvalidOperationException("Stack is empty");

            return _stack.Peek();
        }

        public int Count => _stack.Count;

        public void Clear()
        {
            _stack.Clear();
        }
    }
}