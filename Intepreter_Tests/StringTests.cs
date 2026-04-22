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
    }
}
