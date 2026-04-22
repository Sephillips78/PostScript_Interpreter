using NUnit.Framework;
using Interpreter;
using System;

namespace Interpreter_Tests
{
    [TestFixture]
    public class StackTests
    {
        [Test]
        public void Push_Pop()
        {
            var stack = new PSStack();

            stack.Push(5);
            var result = stack.Pop();

            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void Peek()
        {
            var stack = new PSStack();
            stack.Push(10);

            var result = stack.Peek();

            Assert.That(result, Is.EqualTo(10));
            Assert.That(stack.Count, Is.EqualTo(1));
        }

        [Test]
        public void Pop_Empty()
        {
            var stack = new PSStack();

            Assert.Throws<InvalidOperationException>(() => stack.Pop());
        }

        [Test]
        public void Peek_Empty()
        {
            var stack = new PSStack();

            Assert.Throws<InvalidOperationException>(() => stack.Peek());
        }

        [Test]
        public void Count()
        {
            var stack = new PSStack();

            stack.Push(1);
            stack.Push(2);

            Assert.That(stack.Count, Is.EqualTo(2));

            stack.Pop();

            Assert.That(stack.Count, Is.EqualTo(1));
        }

        [Test]
        public void Clear()
        {
            var stack = new PSStack();

            stack.Push(1);
            stack.Push(2);
            stack.Clear();

            Assert.That(stack.Count, Is.EqualTo(0));
        }
    }
}