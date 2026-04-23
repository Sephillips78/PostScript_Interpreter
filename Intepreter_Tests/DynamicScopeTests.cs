using NUnit.Framework;
using Interpreter;

namespace Interpreter_Tests
{
    [TestFixture]
    public class DynamicScopeTests
    {
        // ----------------------------
        // 1. Basic shadowing
        // ----------------------------
        [Test]
        public void Shadowing_Works_InNestedScopes()
        {
            var i = new PSInterpreter();

            i.Execute(@"
                /x 10 def
                5 dict begin
                    /x 20 def
                    x
                end
                x
            ");

            Assert.That(i.Pop(), Is.EqualTo(10.0));
            Assert.That(i.Pop(), Is.EqualTo(20.0));
        }

        // ----------------------------
        // 2. Inner scope does not leak
        // ----------------------------
        [Test]
        public void InnerScope_DoesNotLeak()
        {
            var i = new PSInterpreter();

            i.Execute(@"
                5 dict begin
                    /x 99 def
                end
            ");

            Assert.Throws<System.Exception>(() =>
                i.Execute("x")
            );
        }

        // ----------------------------
        // 3. Deep lookup (multi-scope chain)
        // ----------------------------
        [Test]
        public void Lookup_SearchesAllScopes()
        {
            var i = new PSInterpreter();

            i.Execute(@"
                /a 1 def
                5 dict begin
                    5 dict begin
                        a
                    end
                end
            ");

            Assert.That(i.Pop(), Is.EqualTo(1.0));
        }

        // ----------------------------
        // 4. Shadow precedence (nearest wins)
        // ----------------------------
        [Test]
        public void NearestScope_Wins()
        {
            var i = new PSInterpreter();

            i.Execute(@"
                /x 1 def
                5 dict begin
                    /x 2 def
                    5 dict begin
                        /x 3 def
                        x
                    end
                end
            ");

            Assert.That(i.Pop(), Is.EqualTo(3.0));
        }

        // ----------------------------
        // 5. Begin/End restores scope correctly
        // ----------------------------
        [Test]
        public void BeginEnd_RestoresPreviousScope()
        {
            var i = new PSInterpreter();

            i.Execute(@"
                /x 100 def
                5 dict begin
                    /x 200 def
                end
                x
            ");

            Assert.That(i.Pop(), Is.EqualTo(100.0));
        }

        // ----------------------------
        // 6. Variable isolation
        // ----------------------------
        [Test]
        public void Variables_AreIsolatedBetweenScopes()
        {
            var i = new PSInterpreter();

            i.Execute(@"
                5 dict begin
                    /a 1 def
                    /b 2 def
                end
            ");

            Assert.Throws<System.Exception>(() => i.Execute("a"));
            Assert.Throws<System.Exception>(() => i.Execute("b"));
        }

        // ----------------------------
        // 7. Rebinding in same scope
        // ----------------------------
        [Test]
        public void Rebinding_OverwritesInSameScope()
        {
            var i = new PSInterpreter();

            i.Execute(@"
                /x 1 def
                /x 2 def
                x
            ");

            Assert.That(i.Pop(), Is.EqualTo(2.0));
        }

        // ----------------------------
        // 8. Cross-scope lookup after exit
        // ----------------------------
        [Test]
        public void CrossScopeLookup_AfterEnd()
        {
            var i = new PSInterpreter();

            i.Execute(@"
                5 dict begin
                    /x 42 def
                    x
                end
            ");

            Assert.That(i.Pop(), Is.EqualTo(42.0));

            Assert.Throws<System.Exception>(() =>
                i.Execute("x")
            );
        }
    }
}