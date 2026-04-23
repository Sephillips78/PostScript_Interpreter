using Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter_Tests
{
    [TestFixture]
    public class StringTests
    {
        [Test]
        public void String_Is_Pushed()
        {
            var interp = new PSInterpreter();

            interp.Execute("(hello)");

            Assert.That(interp.Pop(), Is.EqualTo("hello"));
        }

        [Test]
        public void Equals_PrintsValue()
        {
            var i = new PSInterpreter();

            var sw = new StringWriter();
            Console.SetOut(sw);

            i.Execute("5 =");

            Assert.That(sw.ToString().Trim(), Is.EqualTo("5"));
        }

        [Test]
        public void DoubleEquals_PrintsValue()
        {
            var i = new PSInterpreter();

            var sw = new StringWriter();
            Console.SetOut(sw);

            i.Execute("5 ==");

            Assert.That(sw.ToString().Trim(), Is.EqualTo("5"));
        }

        [Test]
        public void Equals_PopsValue()
        {
            var i = new PSInterpreter();
            i.Execute("5 =");

            Assert.Throws<InvalidOperationException>(() => i.Pop());
        }

        [Test]
        public void Length_Works()
        {
            var i = new PSInterpreter();
            i.Execute("(hello) length");
            Assert.That(i.Pop(), Is.EqualTo(5.0));
        }

        [Test]
        public void Get_Works()
        {
            var i = new PSInterpreter();
            i.Execute("(abc) 1 get");
            Assert.That(i.Pop(), Is.EqualTo(98.0)); // 'b'
        }

        [Test]
        public void GetInterval_Works()
        {
            var i = new PSInterpreter();
            i.Execute("(hello) 1 3 getinterval");
            Assert.That(i.Pop(), Is.EqualTo("ell"));
        }

        [Test]
        public void PutInterval_Works()
        {
            var i = new PSInterpreter();
            i.Execute("(hello) 1 (yy) putinterval");
            Assert.That(i.Pop(), Is.EqualTo("hyylo"));
        }

        [Test]
        public void Get_OutOfRange_Throws()
        {
            var i = new PSInterpreter();
            Assert.Throws<System.Exception>(() =>
                i.Execute("(abc) 5 get"));
        }

        [Test]
        public void GetInterval_OutOfRange_Throws()
        {
            var i = new PSInterpreter();
            Assert.Throws<System.Exception>(() =>
                i.Execute("(abc) 2 5 getinterval"));
        }

        [Test]
        public void PutInterval_OutOfRange_Throws()
        {
            var i = new PSInterpreter();
            Assert.Throws<System.Exception>(() =>
                i.Execute("(abc) 2 (hello) putinterval"));
        }
    }
}
