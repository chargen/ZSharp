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
            var fun = TestHarness.TestPositive(FunctionSignature.parser, "fun test:A -> B");

            Assert.AreEqual("test", fun.name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)fun.Input).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)fun.Output).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_Parses_TInput_TOutput()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.parser, "fun test:(A) -> (B)");

            Assert.AreEqual("test", fun.name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)((Type.TypeSignature.T)fun.Input).Item.ElementTypes.Head).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)((Type.TypeSignature.T)fun.Output).Item.ElementTypes.Head).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_Parses_LInput_LOutput()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.parser, "fun test:[A] -> [B]");

            Assert.AreEqual("test", fun.name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)((Type.TypeSignature.L)fun.Input).Item.ElementType).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)((Type.TypeSignature.L)fun.Output).Item.ElementType).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_Parses_FInput_FOutput()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.parser, "fun test:(A -> B) -> (A -> B)");

            Assert.AreEqual("test", fun.name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)((Type.TypeSignature.F)fun.Input).Item.Input).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)((Type.TypeSignature.F)fun.Input).Item.Output).Item.Name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)((Type.TypeSignature.F)fun.Output).Item.Input).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)((Type.TypeSignature.F)fun.Output).Item.Output).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_DoesNotParse_WithNoSpaceForFun()
        {
            TestHarness.TestNegative(FunctionSignature.parser, "funtest:A -> B");
        }

        [TestMethod]
        public void FunctionHeader_ParsesName_WithNumbers()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.parser, "fun test1 : A -> B");

            Assert.AreEqual("test1", fun.name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)fun.Input).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)fun.Output).Item.Name);
        }

        [TestMethod]
        public void FunctionHeader_Parses_WithSpaceAroundColon()
        {
            var fun = TestHarness.TestPositive(FunctionSignature.parser, "fun test : A -> B");

            Assert.AreEqual("test", fun.name);

            Assert.AreEqual("A", ((Type.TypeSignature.N)fun.Input).Item.Name);
            Assert.AreEqual("B", ((Type.TypeSignature.N)fun.Output).Item.Name);
        }
    }
}
