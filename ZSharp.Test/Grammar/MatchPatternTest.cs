using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;
using ZSharp.Grammar.Function;

namespace ZSharp.Test.Grammar
{
    [TestClass]
    public class MatchPatternTest
    {
        [TestMethod]
        public void MatchPattern_ParsesNamedInput()
        {
            var match = (NamedValue)MatchPattern.Parser.Parse("x");

            Assert.AreEqual("x", match.Name);
        }

        [TestMethod]
        public void MatchPattern_ParsesValuedInput()
        {
            var match = (NamedValue)MatchPattern.Parser.Parse("0");

            Assert.AreEqual("0", match.Name);
        }

        [TestMethod]
        public void MatchPattern_ParsesIgnoredNamedInput()
        {
            var match = (NamedValue)MatchPattern.Parser.Parse("_");

            Assert.AreEqual("_", match.Name);
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithNoItems()
        {
            var match = MatchPattern.Parser.Parse("[ ]");
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithSingleItem()
        {
            var match = MatchPattern.Parser.Parse("[x]");
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithSingleValue()
        {
            var match = MatchPattern.Parser.Parse("[1]");
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithTwoItems()
        {
            var match = MatchPattern.Parser.Parse("[x, y]");
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithTwoValues()
        {
            var match = MatchPattern.Parser.Parse("[2, 3]");
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithItemAndTail()
        {
            var match = MatchPattern.Parser.Parse("[x, ..tail]");
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithValueAndTail()
        {
            var match = MatchPattern.Parser.Parse("[4, ..tail]");
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithHeadAndItems()
        {
            var match = MatchPattern.Parser.Parse("[head.., x, y]");
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithHeadAndValues()
        {
            var match = MatchPattern.Parser.Parse("[head.., 5, 6]");
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithHeadAndItems_WithInsignificantWhitespace()
        {
            var match = MatchPattern.Parser.Parse("[ head.. , x , y ]");
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithItemAndBodyAndItem()
        {
            var match = MatchPattern.Parser.Parse("[a, b, ..mid.., x, y]");
        }

        [TestMethod]
        public void MatchPattern_ParsesList_WithValueAndBodyAndItem()
        {
            var match = MatchPattern.Parser.Parse("[7, 8, ..mid.., x, y]");
        }

        [TestMethod]
        public void MatchPattern_ParsesTuple_WithSingleItem()
        {
            var match = (TupleDestructure)MatchPattern.Parser.Parse("(a)");

            Assert.AreEqual("a", match.Names[0]);
        }

        [TestMethod]
        public void MatchPattern_ParsesTuple_WithSingleValue()
        {
            var match = (TupleDestructure)MatchPattern.Parser.Parse("(9)");
        }

        [TestMethod]
        public void MatchPattern_ParsesTuple_WithMultipleItems()
        {
            var match = (TupleDestructure)MatchPattern.Parser.Parse("(a, bc, def, _)");

            Assert.AreEqual("a", match.Names[0]);
            Assert.AreEqual("bc", match.Names[1]);
            Assert.AreEqual("def", match.Names[2]);
        }

        [TestMethod]
        public void MatchPattern_ParsesTuple_WithMultipleValues()
        {
            var match = (TupleDestructure)MatchPattern.Parser.Parse("(1, 2, 3, 4)");
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void MatchPattern_FailsParsingTuple_WithHangingComma()
        {
            MatchPattern.Parser.Parse("(a, )");
        }

        [TestMethod]
        public void MatchPattern_ParsesHierarchicalMatch()
        {
            var match = (TupleDestructure)MatchPattern.Parser.Parse("([], 0, a, (1, [b, ..c.., 2]))");
        }
    }
}
