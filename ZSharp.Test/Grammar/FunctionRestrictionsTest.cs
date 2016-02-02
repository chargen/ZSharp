using System.Linq;
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
            var res = TestHarness.TestPositive(Functions.restriction_parser, "A : B");

            var direct = ((Functions.Restriction.D)res).Item;

            var left = direct.Left.Name;
            var right = ((Type.TypeSignature.N)direct.Right).Item.Name;

            Assert.AreEqual("A", left);
            Assert.AreEqual("B", right);
        }

        [TestMethod]
        public void FunctionRestrictions_Parser_DirectTypeRestriction_ToFunctionType()
        {
            var res = TestHarness.TestPositive(Functions.restriction_parser, "A : (B -> C)");

            var direct = ((Functions.Restriction.D)res).Item;

            var left = direct.Left.Name;

            var right = ((Type.TypeSignature.F)direct.Right).Item;
            var rin = ((Type.TypeSignature.N)right.Input).Item.Name;
            var rout = ((Type.TypeSignature.N)right.Output).Item.Name;

            Assert.AreEqual("A", left);
            Assert.AreEqual("B", rin);
            Assert.AreEqual("C", rout);
        }

        [TestMethod]
        public void FunctionHeader_TypeRestrictions_Parses_SingleDirectRestriction()
        {
            var fun = TestHarness.TestPositive(Functions.restrictions_list, "where [ A : B ]").Single();

            Assert.AreEqual("A", (((Functions.Restriction.D)fun).Item).Left.Name);

            var r = (((Functions.Restriction.D)fun).Item).Right;
            Assert.AreEqual("B", ((Type.TypeSignature.N)r).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_TypeRestrictions_Parses_MultipleCommaSeparatedDirectRestrictions()
        {
            var fun = TestHarness.TestPositive(Functions.restrictions_list, "where [ A : B, C : D ]").ToArray();

            Assert.AreEqual(2, fun.Length);

            var first = fun[0];
            Assert.AreEqual("A", ((Functions.Restriction.D)first).Item.Left.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)(((Functions.Restriction.D)first).Item.Right)).Item.Name);

            var second = fun[1];
            Assert.AreEqual("C", ((Functions.Restriction.D)second).Item.Left.Name);
            Assert.AreEqual("D", ((Type.TypeSignature.N)(((Functions.Restriction.D)second).Item.Right)).Item.Name);
        }
    }
}
