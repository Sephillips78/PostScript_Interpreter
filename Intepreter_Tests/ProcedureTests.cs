using NUnit.Framework;
using System.Collections.Generic;
using Interpreter;

namespace Interpreter_Tests
{
    [TestFixture]
    public class ProcedureTests
    {
        [Test]
        public void Procedure_IsPushed_AsList()
        {
            var interpreter = new PSInterpreter();

            interpreter.Execute("{ 3 4 add }");

            var result = interpreter.Pop();

            Assert.That(result, Is.InstanceOf<List<Token>>());
        }

        [Test]
        public void Procedure_ContainsCorrectTokens()
        {
            var interpreter = new PSInterpreter();

            interpreter.Execute("{ 3 4 add }");

            var result = (List<Token>)interpreter.Pop();

            Assert.That(result.Count, Is.EqualTo(3));
        }
    }
}