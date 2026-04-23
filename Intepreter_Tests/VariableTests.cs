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

        // ----------------------------
        // NEW: ensures lookup works after redefinition
        // ----------------------------
        [Test]
        public void Def_OverwritesValue_InSameScope()
        {
            var interp = new PSInterpreter();

            interp.Execute("/x 10 def");
            interp.Execute("/x 20 def");
            interp.Execute("x");

            Assert.That(interp.Pop(), Is.EqualTo(20.0));
        }

        // ----------------------------
        // NEW: ensures variable resolves inside expressions
        // ----------------------------
        [Test]
        public void Variable_UsedInExpression()
        {
            var interp = new PSInterpreter();

            interp.Execute("/x 10 def");
            interp.Execute("x x add");

            Assert.That(interp.Pop(), Is.EqualTo(20.0));
        }
    }
}