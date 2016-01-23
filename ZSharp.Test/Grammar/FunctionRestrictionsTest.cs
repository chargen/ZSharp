using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSharp.Parser;

namespace ZSharp.Test.Grammar
{
    [TestClass]
    public class FunctionRestrictionsTest
    {
        [TestMethod]
        public void FunctionRestrictions_Parser_DirectTypeRestriction_ToNamedType()
        {
            var res = TestHarness.TestPositive(FunctionRestrictions.parser, "A : B");

            var direct = ((FunctionRestrictions.Restriction.D)res).Item;

            var left = direct.Left.Name;
            var right = ((Type.TypeSignature.N)direct.Right).Item.Name;

            Assert.AreEqual("A", left);
            Assert.AreEqual("B", right);
        }

        [TestMethod]
        public void FunctionRestrictions_Parser_DirectTypeRestriction_ToFunctionType()
        {
            var res = TestHarness.TestPositive(FunctionRestrictions.parser, "A : (B -> C)");

            var direct = ((FunctionRestrictions.Restriction.D)res).Item;

            var left = direct.Left.Name;

            var right = ((Type.TypeSignature.F)direct.Right).Item;
            var rin = ((Type.TypeSignature.N)right.Input).Item.Name;
            var rout = ((Type.TypeSignature.N)right.Output).Item.Name;

            Assert.AreEqual("A", left);
            Assert.AreEqual("B", rin);
            Assert.AreEqual("C", rout);
        }
    }
}
