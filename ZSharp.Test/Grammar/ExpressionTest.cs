using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSharp.Parser;

namespace ZSharp.Test.Grammar
{
    [TestClass]
    public class ExpressionTest
    {
        [TestMethod]
        public void Expression_ParsesReadVariable()
        {
            var exp = TestHarness.TestPositive(Expressions.parser, "hello");

            var read = (Expressions.Expression.Read)exp;
            Assert.AreEqual("hello", read.Item.Name);
        }

        [TestMethod]
        public void Expression_ParsesBindVariable_WithTypeBinding()
        {
            var exp = TestHarness.TestPositive(Expressions.parser, "let a : T = b");

            var left = (Expressions.Expression.Bind)exp;
            Assert.AreEqual("a", left.Item.Name);
            Assert.AreEqual("T", ((Type.TypeSignature.N)left.Item.Type.Value).Item.Name);

            var right = (Expressions.Expression.Read)left.Item.Value;
            Assert.AreEqual("b", right.Item.Name);
        }

        [TestMethod]
        public void Expression_FailsParseBindVariable_WithNoSpaceAfterLet()
        {
            TestHarness.TestNegative(Expressions.parser, "leta = b");
        }

        [TestMethod]
        public void Expression_ParsesConditional()
        {
            var exp = TestHarness.TestPositive(Expressions.parser, "if a then b else c");

            var ifexp = (Expressions.Expression.Conditional)exp;

            Assert.AreEqual("a", ((Expressions.Expression.Read)ifexp.Item.Condition).Item.Name);
            Assert.AreEqual("b", ((Expressions.Expression.Read)ifexp.Item.True).Item.Name);
            Assert.AreEqual("c", ((Expressions.Expression.Read)ifexp.Item.False).Item.Name);
        }

        [TestMethod]
        public void Expression_FailsParseConditional_WithNoSpaceAfterIf()
        {
            TestHarness.TestNegative(Expressions.parser, "ifa then b else c");
        }

        [TestMethod]
        public void Expression_FailsParseConditional_WithNoSpaceAfterThen()
        {
            TestHarness.TestNegative(Expressions.parser, "if a thenb else c");
        }

        [TestMethod]
        public void Expression_FailsParseConditional_WithNoSpaceAfterElse()
        {
            TestHarness.TestNegative(Expressions.parser, "if a then b elsec");
        }
    }
}
