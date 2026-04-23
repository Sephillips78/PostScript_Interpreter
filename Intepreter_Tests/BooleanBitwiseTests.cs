using NUnit.Framework;
using Interpreter;

namespace Interpreter_Tests
{
    [TestFixture]
    public class BooleanAndComparisonTests
    {
        [Test]
        public void Eq_Works()
        {
            var i = new PSInterpreter();
            i.Execute("5 5 eq");
            Assert.That(i.Pop(), Is.EqualTo(true));
        }

        [Test]
        public void Ne_Works()
        {
            var i = new PSInterpreter();
            i.Execute("5 4 ne");
            Assert.That(i.Pop(), Is.EqualTo(true));
        }

        [Test]
        public void Gt_Works()
        {
            var i = new PSInterpreter();
            i.Execute("5 3 gt");
            Assert.That(i.Pop(), Is.EqualTo(true));
        }

        [Test]
        public void Ge_Works()
        {
            var i = new PSInterpreter();
            i.Execute("5 5 ge");
            Assert.That(i.Pop(), Is.EqualTo(true));
        }

        [Test]
        public void Lt_Works()
        {
            var i = new PSInterpreter();
            i.Execute("2 3 lt");
            Assert.That(i.Pop(), Is.EqualTo(true));
        }

        [Test]
        public void Le_Works()
        {
            var i = new PSInterpreter();
            i.Execute("3 3 le");
            Assert.That(i.Pop(), Is.EqualTo(true));
        }

        [Test]
        public void And_Works()
        {
            var i = new PSInterpreter();
            i.Execute("true false and");
            Assert.That(i.Pop(), Is.EqualTo(false));
        }

        [Test]
        public void Or_Works()
        {
            var i = new PSInterpreter();
            i.Execute("true false or");
            Assert.That(i.Pop(), Is.EqualTo(true));
        }

        [Test]
        public void Not_Works()
        {
            var i = new PSInterpreter();
            i.Execute("true not");
            Assert.That(i.Pop(), Is.EqualTo(false));
        }
    }
}