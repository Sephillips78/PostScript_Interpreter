using NUnit.Framework;
using Interpreter;

namespace Interpreter_Tests
{
    [TestFixture]
    public class OperatorTests
    {

        [Test]
        public void Dup_DuplicatesTop()
        {
            var stack = new PSStack();
            var interpreter = new PSInterpreter();
            stack.Push(5);

            var ops = new Operators(stack, interpreter);
            ops.Dup();

            Assert.That(stack.Pop(), Is.EqualTo(5));
            Assert.That(stack.Pop(), Is.EqualTo(5));
        }

        [Test]
        public void Exch_SwapsTopTwo()
        {
            var stack = new PSStack();
            var interpreter = new PSInterpreter();
            stack.Push(1);
            stack.Push(2);

            var ops = new Operators(stack, interpreter);
            ops.Exch();

            Assert.That(stack.Pop(), Is.EqualTo(1));
            Assert.That(stack.Pop(), Is.EqualTo(2));
        }

        [Test]
        public void Pop_RemovesTop()
        {
            var stack = new PSStack();
            var interpreter = new PSInterpreter();
            stack.Push(10);

            var ops = new Operators(stack, interpreter);
            ops.Pop();

            Assert.That(stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void Add_Works()
        {
            var stack = new PSStack();
            var interpreter = new PSInterpreter();
            stack.Push(3);
            stack.Push(4);

            var ops = new Operators(stack, interpreter);
            ops.Add();

            Assert.That(stack.Pop(), Is.EqualTo(7.0));
        }
    }
}