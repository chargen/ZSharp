using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;
using ZSharp.Grammar;

namespace ZSharp.Test.Grammar
{
    [TestClass]
    public class CoreTest
    {
        [TestMethod]
        public void CoreIdentifier_Parses_Word()
        {
            var id = Core.Identifier.Parse("Hello");

            Assert.AreEqual("Hello", id);
        }

        [TestMethod]
        public void CoreIdentifier_Parses_WordWithNumber()
        {
            var id = Core.Identifier.Parse("Hello1");

            Assert.AreEqual("Hello1", id);
        }

        [TestMethod]
        public void CoreIdentifier_Parses_WordWithUnderscore()
        {
            var id = Core.Identifier.Parse("Hello_World");

            Assert.AreEqual("Hello_World", id);
        }

        [TestMethod]
        public void CoreIdentifier_Parses_WordWithLeadingUnderscore()
        {
            var id = Core.Identifier.Parse("_Hello_World");

            Assert.AreEqual("_Hello_World", id);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void CoreIdentifier_Throws_WordWithPunctuation()
        {
            Core.Identifier.Parse("@Hello");
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void CoreIdentifier_Throws_WordWithLeadingNumber()
        {
            Core.Identifier.Parse("1Hello");
        }
    }
}
