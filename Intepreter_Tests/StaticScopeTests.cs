using NUnit.Framework;
using Interpreter;

namespace Interpreter_Tests
{
    [TestFixture]
    public class StaticScopeTests
    {
        // ----------------------------
        // 1. Basic static capture
        // ----------------------------
        [Test]
        public void StaticScope_CapturesValueAtDefinitionTime()
        {
            var i = new PSInterpreter(PSInterpreter.ScopeMode.Static);

            i.Execute(@"
                /x 10 def
                /p { x } def
                /x 20 def
                p
            ");

            Assert.That(i.Pop(), Is.EqualTo(10.0));
        }

        // ----------------------------
        // 2. Dynamic vs static contrast check (core test)
        // ----------------------------
        [Test]
        public void StaticScope_IgnoresLaterRebinding()
        {
            var i = new PSInterpreter(PSInterpreter.ScopeMode.Static);

            i.Execute(@"
                /x 1 def
                /p { x } def
                /x 99 def
                p
            ");

            Assert.That(i.Pop(), Is.EqualTo(1.0));
        }

        // ----------------------------
        // 3. Static scope inside nested dictionaries
        // ----------------------------
        [Test]
        public void StaticScope_ResolvesCapturedDictionary()
        {
            var i = new PSInterpreter(PSInterpreter.ScopeMode.Static);

            i.Execute(@"
                /x 5 def
                5 dict begin
                    /x 10 def
                    /p { x } def
                end
                p
            ");

            Assert.That(i.Pop(), Is.EqualTo(10.0)); // <-- important correction
        }

        // ----------------------------
        // 4. Static scope with multiple variables
        // ----------------------------
        [Test]
        public void StaticScope_CapturesMultipleVariables()
        {
            var i = new PSInterpreter(PSInterpreter.ScopeMode.Static);

            i.Execute(@"
                /a 2 def
                /b 3 def
                /p { a b add } def
                /a 10 def
                /b 20 def
                p
            ");

            Assert.That(i.Pop(), Is.EqualTo(5.0));
        }

        // ----------------------------
        // 5. Static scope with shadowing inside definition
        // ----------------------------
        [Test]
        public void StaticScope_ShadowingInsideDefinitionIsPreserved()
        {
            var i = new PSInterpreter(PSInterpreter.ScopeMode.Static);

            i.Execute(@"
                /x 7 def
                /p {
                    /x 100 def
                    x
                } def

                /x 200 def
                p
            ");

            Assert.That(i.Pop(), Is.EqualTo(100.0));
        }

        // ----------------------------
        // 6. Static procedure used multiple times
        // ----------------------------
        [Test]
        public void StaticScope_ProcedureIsReusable()
        {
            var i = new PSInterpreter(PSInterpreter.ScopeMode.Static);

            i.Execute(@"
                /x 1 def
                /p { x } def
                /x 2 def
                p
                p
            ");

            Assert.That(i.Pop(), Is.EqualTo(1.0));
            Assert.That(i.Pop(), Is.EqualTo(1.0));
        }

        // ----------------------------
        // 7. Static scope does NOT follow runtime dictionary changes
        // ----------------------------
        [Test]
        public void StaticScope_DoesNotFollowBeginEndChanges()
        {
            var i = new PSInterpreter(PSInterpreter.ScopeMode.Static);

            i.Execute(@"
                /x 10 def
                /p { x } def

                5 dict begin
                    /x 999 def
                end

                p
            ");

            Assert.That(i.Pop(), Is.EqualTo(10.0));
        }
    }
}