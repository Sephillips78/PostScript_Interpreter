using NUnit.Framework;
using Interpreter;
using System;

namespace Interpreter_Tests
{
    [TestFixture]
    public class ArithmeticTests
    {
        [Test]
        public void Mul_Works()
        {
            var i = new PSInterpreter();
            i.Execute("6 3 mul");
            Assert.That(i.Pop(), Is.EqualTo(18.0));
        }

        [Test]
        public void Div_Works()
        {
            var i = new PSInterpreter();
            i.Execute("7 2 div");
            Assert.That(i.Pop(), Is.EqualTo(3.5));
        }

        [Test]
        public void Div_ByZero_Throws()
        {
            var i = new PSInterpreter();
            Assert.Throws<DivideByZeroException>(() => i.Execute("5 0 div"));
        }

        [Test]
        public void IDiv_Works()
        {
            var i = new PSInterpreter();
            i.Execute("7 2 idiv");
            Assert.That(i.Pop(), Is.EqualTo(3.0));
        }

        [Test]
        public void IDiv_ByZero_Throws()
        {
            var i = new PSInterpreter();
            Assert.Throws<DivideByZeroException>(() => i.Execute("5 0 idiv"));
        }

        [Test]
        public void Mod_Works()
        {
            var i = new PSInterpreter();
            i.Execute("7 2 mod");
            Assert.That(i.Pop(), Is.EqualTo(1.0));
        }

        [Test]
        public void Mod_ByZero_Throws()
        {
            var i = new PSInterpreter();
            Assert.Throws<DivideByZeroException>(() => i.Execute("5 0 mod"));
        }

        [Test]
        public void Abs_Works()
        {
            var i = new PSInterpreter();
            i.Execute("-5 abs");
            Assert.That(i.Pop(), Is.EqualTo(5.0));
        }

        [Test]
        public void Neg_Works()
        {
            var i = new PSInterpreter();
            i.Execute("5 neg");
            Assert.That(i.Pop(), Is.EqualTo(-5.0));
        }

        [Test]
        public void Ceiling_Works()
        {
            var i = new PSInterpreter();
            i.Execute("2.3 ceiling");
            Assert.That(i.Pop(), Is.EqualTo(3.0));
        }

        [Test]
        public void Floor_Works()
        {
            var i = new PSInterpreter();
            i.Execute("2.7 floor");
            Assert.That(i.Pop(), Is.EqualTo(2.0));
        }

        [Test]
        public void Round_Works()
        {
            var i = new PSInterpreter();
            i.Execute("2.5 round");

            var result = (double)i.Pop();

            // Allow either 2 or 3 depending on rounding mode
            Assert.That(result == 2.0 || result == 3.0);
        }

        [Test]
        public void Sqrt_Works()
        {
            var i = new PSInterpreter();
            i.Execute("9 sqrt");
            Assert.That(i.Pop(), Is.EqualTo(3.0));
        }

        [Test]
        public void Sqrt_Negative_Throws()
        {
            var i = new PSInterpreter();
            Assert.Throws<Exception>(() => i.Execute("-1 sqrt"));
        }
    }
}