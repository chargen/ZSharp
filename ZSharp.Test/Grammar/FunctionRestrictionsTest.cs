using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSharp.Parser;
using Type = System.Type;

namespace ZSharp.Test.Grammar
{
    [TestClass]
    public class FunctionRestrictionsTest
    {
        [TestMethod]
        public void FunctionRestrictions_Parser_BasicRestrictTypeToOtherType()
        {
            var res = TestHarness.TestPositive(FunctionRestrictions.parser, "where A : B");

            var direct = ((FunctionRestrictions.Restriction.D)res).Item;
            var right = ((Parser.Type.TypeSignature.N)direct.Right).Item.Name;

            Assert.AreEqual("A", direct.Left);
            Assert.AreEqual("B", right);
        }
    }
}
