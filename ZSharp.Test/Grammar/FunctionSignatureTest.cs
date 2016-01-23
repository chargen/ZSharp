using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSharp.Parser;

namespace ZSharp.Test.Grammar
{
    [TestClass]
    public class FunctionSignatureTest
    {
        [TestMethod]
        public void FunctionHeader_Parses_NInput_NOutput()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.signature_parser, "fun test:A -> B");

            Assert.AreEqual("test", fun.Name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)fun.Input).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)fun.Output).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_Parses_TInput_TOutput()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.signature_parser, "fun test:(A) -> (B)");

            Assert.AreEqual("test", fun.Name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)((Type.TypeSignature.T)fun.Input).Item.ElementTypes.Head).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)((Type.TypeSignature.T)fun.Output).Item.ElementTypes.Head).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_Parses_LInput_LOutput()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.signature_parser, "fun test:[A] -> [B]");

            Assert.AreEqual("test", fun.Name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)((Type.TypeSignature.L)fun.Input).Item.ElementType).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)((Type.TypeSignature.L)fun.Output).Item.ElementType).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_Parses_FInput_FOutput()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.signature_parser, "fun test:(A -> B) -> (A -> B)");

            Assert.AreEqual("test", fun.Name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)((Type.TypeSignature.F)fun.Input).Item.Input).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)((Type.TypeSignature.F)fun.Input).Item.Output).Item.Name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)((Type.TypeSignature.F)fun.Output).Item.Input).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)((Type.TypeSignature.F)fun.Output).Item.Output).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_DoesNotParse_WithNoSpaceForFun()
        {
            TestHarness.TestNegative(FunctionSignature.signature_parser, "funtest:A -> B");
        }

        [TestMethod]
        public void FunctionHeader_ParsesName_WithNumbers()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.signature_parser, "fun test1 : A -> B");

            Assert.AreEqual("test1", fun.Name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)fun.Input).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)fun.Output).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_Parses_WithSpaceAroundColon()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.signature_parser, "fun test : A -> B");

            Assert.AreEqual("test", fun.Name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)fun.Input).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)fun.Output).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_TypeRestrictions_Parses_SingleDirectRestriction()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.restrictions_list, "where A : B").Single();

            Assert.AreEqual("A", (((FunctionRestrictions.Restriction.D)fun).Item).Left.Name);

            var r = (((FunctionRestrictions.Restriction.D)fun).Item).Right;
            Assert.AreEqual("B", ((Type.TypeSignature.N)r).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_TypeRestrictions_Parses_MultipleCommaSeparatedDirectRestrictions()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.restrictions_list, "where A : B, C : D").ToArray();

            Assert.AreEqual(2, fun.Length);

            Assert.Fail("Check other stuff");
        }

        [TestMethod]
        public void FunctionHeader_TypeRestrictions_Parses_MultipleAmpersandSeparatedDirectRestrictions()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.restrictions_list, "where A : B & C : D").ToArray();

            Assert.AreEqual(2, fun.Length);

            Assert.Fail("Check other stuff");
        }

        [TestMethod]
        public void FunctionHeader_TypeRestrictions_Parses_MultipleNewlineSeparatedDirectRestrictions()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.restrictions_list, "where A : B \n C : D").ToArray();

            Assert.AreEqual(2, fun.Length);

            Assert.Fail("Check other stuff");
        }
    }
}
