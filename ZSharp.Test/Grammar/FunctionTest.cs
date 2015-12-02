using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;
using ZSharp.Grammar;
using ZSharp.Grammar.Function;

namespace ZSharp.Test.Grammar
{
    [TestClass]
    public class FunctionTest
    {
        [TestMethod]
        public void FunctionHeader_ParsesName()
        {
            var function = Function.Parser.Parse("fun test : () -> T\n\ta => a");
            Assert.AreEqual("test", function.Name);
        }

        [TestMethod]
        public void FunctionHeader_ParsesName_WithNumbers()
        {
            var function = Function.Parser.Parse("fun test1 : () -> T\n\ta => a");
            Assert.AreEqual("test1", function.Name);
        }

        [TestMethod]
        public void FunctionHeader_ParsesName_WithNoSpaceAroundColon()
        {
            var function = Function.Parser.Parse("fun test1:() -> T\n\ta => a");
            Assert.AreEqual("test1", function.Name);
        }

        [TestMethod]
        public void FunctionHeader_ParsesName_WithLotsOfSpaceAroundColon()
        {
            var function = Function.Parser.Parse("fun test1           :              () -> T\n\ta => a");
            Assert.AreEqual("test1", function.Name);
        }

        [TestMethod]
        public void FunctionHeader_ParsesInput_WithUnitInputOutput()
        {
            var func = Function.Parser.Parse("fun a : () -> ()\n\ta => a");

            Assert.AreEqual("Unit", ((NamedType)func.Signature.Input).Name);
            Assert.AreEqual("Unit", ((NamedType)func.Signature.Output).Name);
        }

        [TestMethod]
        public void TypeSignature_ParsesOutput_WithTOutput()
        {
            var func = Function.Parser.Parse("fun a : () -> T\n\ta => a");

            Assert.AreEqual("Unit", ((NamedType)func.Signature.Input).Name);
            Assert.AreEqual("T", ((NamedType)func.Signature.Output).Name);
        }

        [TestMethod]
        public void TypeSignature_ParsesOutput_WithListOutput()
        {
            var func = Function.Parser.Parse("fun a : () -> [T]\n\ta => a");

            Assert.AreEqual("Unit", ((NamedType)func.Signature.Input).Name);
            Assert.AreEqual("T", ((NamedType)((ListType)func.Signature.Output).Element).Name);
        }

        [TestMethod]
        public void TypeSignature_ParsesOutput_WithFuncOutput()
        {
            var func = Function.Parser.Parse("fun a : {A -> B} -> C\n\ta => a");

            Assert.AreEqual("A", ((NamedType)((FuncType)func.Signature.Input).Input).Name);
            Assert.AreEqual("B", ((NamedType)((FuncType)func.Signature.Input).Output).Name);
            Assert.AreEqual("C", ((NamedType)func.Signature.Output).Name);
        }
    }
}
