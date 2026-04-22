using NUnit.Framework;
using Interpreter;

namespace Interpreter_Tests
{
    [TestFixture]
    public class VariableTests
    {
        [Test]
        public void Def_StoresValue()
        {
            var interp = new PSInterpreter();

            interp.Execute("/x 10 def");
            interp.Execute("x");

            Assert.That(interp.Pop(), Is.EqualTo(10.0));
        }

        [Test]
        public void Def_AllowsReuse()
        {
            var interp = new PSInterpreter();

            interp.Execute("/x 10 def");
            interp.Execute("x 5 add");

            Assert.That(interp.Pop(), Is.EqualTo(15.0));
        }
    }
}