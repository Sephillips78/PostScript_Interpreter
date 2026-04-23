using NUnit.Framework;
using Interpreter;

namespace Interpreter_Tests
{
    [TestFixture]
    public class ProcedureTests
    {
        [Test]
        public void Procedure_IsCreated_AsProcedureObject()
        {
            var interpreter = new PSInterpreter();

            interpreter.Execute("{ 3 4 add }");

            var result = interpreter.Pop();

            Assert.That(result, Is.InstanceOf<Procedure>());
        }

        [Test]
        public void Procedure_IsExecutable()
        {
            var interpreter = new PSInterpreter();

            interpreter.Execute("{ 3 4 add }");

            var proc = interpreter.Pop() as Procedure;

            Assert.That(proc, Is.Not.Null);

            interpreter.ExecuteProcedure(proc);

            Assert.That(interpreter.Pop(), Is.EqualTo(7.0));
        }

        [Test]
        public void Procedure_ContainsBodyTokens()
        {
            var interpreter = new PSInterpreter();

            interpreter.Execute("{ 3 4 add }");

            var proc = interpreter.Pop() as Procedure;

            Assert.That(proc, Is.Not.Null);

            Assert.That(proc.Body.Count, Is.EqualTo(3));
        }
    }
}