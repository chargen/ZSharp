using FParsec;
using Microsoft.FSharp.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSharp.Parser;

namespace ZSharp.Test.Grammar
{
    [TestClass]
    public class ParserCoreIdentifierTest
    {
        [TestMethod]
        public void CoreIdentifier_Parses_Word()
        {
            var id = TestHarness.TestPositive(Identifier.identifier, "Hello");

            Assert.AreEqual("Hello", id);
        }

        [TestMethod]
        public void CoreIdentifier_Parses_WordWithNumber()
        {
            var id = TestHarness.TestPositive(Identifier.identifier, "Hello1");

            Assert.AreEqual("Hello1", id);
        }

        [TestMethod]
        public void CoreIdentifier_Parses_WordWithUnderscore()
        {
            var id = TestHarness.TestPositive(Identifier.identifier, "Hello_World");

            Assert.AreEqual("Hello_World", id);
        }

        [TestMethod]
        public void CoreIdentifier_Parses_WordWithLeadingUnderscore()
        {
            var id = TestHarness.TestPositive(Identifier.identifier, "_Hello_World");

            Assert.AreEqual("_Hello_World", id);
        }

        [TestMethod]
        public void CoreIdentifier_Throws_WordWithPunctuation()
        {
            TestHarness.TestNegative(Identifier.identifier, ",Hello");
        }

        [TestMethod]
        public void CoreIdentifier_Throws_WordWithLeadingNumber()
        {
            TestHarness.TestNegative(Identifier.identifier, "1Hello");
        }
    }
}
