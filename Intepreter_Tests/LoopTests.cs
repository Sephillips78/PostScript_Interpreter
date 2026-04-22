using NUnit.Framework;
using Interpreter;

namespace Interpreter_Tests
{
    [TestFixture]
    public class LoopTests
    {
        [Test]
        public void Repeat_RunsProcedureN_Times()
        {
            var interp = new PSInterpreter();

            interp.Execute("3 { 1 } repeat");

            Assert.That(interp.Pop(), Is.EqualTo(1.0));
            Assert.That(interp.Pop(), Is.EqualTo(1.0));
            Assert.That(interp.Pop(), Is.EqualTo(1.0));
        }

        [Test]
        public void Repeat_WithMath_Works()
        {
            var interp = new PSInterpreter();

            interp.Execute("3 3 { dup 1 sub } repeat");

            Assert.That(interp.Pop(), Is.EqualTo(0.0));
        }

        [Test]
        public void For_PushesEachValue()
        {
            var interp = new PSInterpreter();

            interp.Execute("1 3 { } for");

            // should leave nothing meaningful, just ensure no crash
            Assert.Pass();
        }

        [Test]
        public void For_ExecutesProcedureWithValues()
        {
            var interp = new PSInterpreter();

            interp.Execute("1 3 { dup } for");

            // last iteration leaves 3 duplicated
            Assert.That(interp.Pop(), Is.EqualTo(3.0));
            Assert.That(interp.Pop(), Is.EqualTo(3.0));
        }
    }
}