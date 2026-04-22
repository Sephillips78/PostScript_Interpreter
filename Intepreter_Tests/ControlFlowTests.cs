using NUnit.Framework;
using System.Collections.Generic;
using Interpreter;

namespace Interpreter_Tests
{
    [TestFixture]
    public class ControlFlowTests
    {
        [Test]
        public void If_True_ExecutesProcedure()
        {
            var interpreter = new PSInterpreter();

            interpreter.Execute("true { 5 } if");

            Assert.That(interpreter.Pop(), Is.EqualTo(5.0));
        }

        [Test]
        public void If_False_DoesNotExecuteProcedure()
        {
            var interpreter = new PSInterpreter();

            interpreter.Execute("false { 5 } if");

            Assert.Throws<System.InvalidOperationException>(() => interpreter.Pop());
        }

        [Test]
        public void IfElse_True_ExecutesFirstProcedure()
        {
            var interp = new PSInterpreter();

            interp.Execute("true { 1 } { 2 } ifelse");

            Assert.That(interp.Pop(), Is.EqualTo(1.0));
        }

        [Test]
        public void IfElse_False_ExecutesSecondProcedure()
        {
            var interp = new PSInterpreter();

            interp.Execute("false { 1 } { 2 } ifelse");

            Assert.That(interp.Pop(), Is.EqualTo(2.0));
        }
    }
}