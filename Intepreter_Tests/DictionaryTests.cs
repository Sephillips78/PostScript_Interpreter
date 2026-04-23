using NUnit.Framework;
using Interpreter;
using System.Collections.Generic;

namespace Interpreter_Tests
{
    [TestFixture]
    public class DictionaryTests
    {
        [Test]
        public void Dict_CreatesDictionary()
        {
            var i = new PSInterpreter();
            i.Execute("5 dict");

            var result = i.Pop();

            Assert.That(result, Is.InstanceOf<Dictionary<string, object>>());
        }

        [Test]
        public void Begin_End_ScopeWorks()
        {
            var i = new PSInterpreter();

            // x is defined inside scoped dictionary, should NOT leak
            i.Execute("5 dict begin /x 10 def end");

            Assert.Throws<System.Exception>(() =>
            {
                i.Execute("x");
            });
        }

        [Test]
        public void NestedScope_Works()
        {
            var i = new PSInterpreter();

            i.Execute(@"
                5 dict begin
                    /x 10 def

                    5 dict begin
                        /x 20 def
                        x
                    end

                    x
                end
            ");

            // IMPORTANT:
            // stack order: last executed value is top

            Assert.That(i.Pop(), Is.EqualTo(10.0));
            Assert.That(i.Pop(), Is.EqualTo(20.0));
        }

        [Test]
        public void Length_OnDict_Works()
        {
            var i = new PSInterpreter();

            i.Execute("5 dict begin /x 10 def /y 20 def currentdict length end");

            Assert.That(i.Pop(), Is.EqualTo(2.0));
        }
    }
}