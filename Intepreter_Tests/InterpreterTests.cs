using NUnit.Framework;
using Interpreter;

namespace Interpreter_Tests
{
    [TestFixture]
    public class InterpreterTests
    {
        [Test]
        public void Execute_FullProgram_Works()
        {
            var interpreter = new PSInterpreter();

            interpreter.Execute("3 4 add");

            Assert.That(interpreter.Pop(), Is.EqualTo(7.0));
        }

        [Test]
        public void UnknownToken_Throws()
        {
            var interpreter = new PSInterpreter();

            Assert.Throws<System.Exception>(() =>
                interpreter.ExecuteToken("unknown"));
        }
    }
}