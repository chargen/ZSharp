using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;
using ZSharp.Grammar;

namespace ZSharp.Test.Grammar
{
    [TestClass]
    public class TypeTest
    {
        [TestMethod]
        public void Type_ParsesUnitType()
        {
            var type = (NamedType)Type.Parser.Parse("()");

            Assert.AreEqual("Unit", type.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void Type_DoesNotParse_UnbalancedBrackets()
        {
            Type.Parser.Parse("(()");
        }

        [TestMethod]
        public void Type_ParsesNamedType()
        {
            var type = (NamedType)Type.Parser.Parse("T");

            Assert.AreEqual("T", type.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void Type_DoesNotParse_InvalidNamedType()
        {
            Type.Parser.Parse("1");
        }

        [TestMethod]
        public void Type_ParsesTupleType()
        {
            var type = (TupleType)Type.Parser.Parse("(A, (), [T], {B -> C})");

            Assert.AreEqual("A", ((NamedType)type.Elements[0]).Name);
            Assert.AreEqual("Unit", ((NamedType)type.Elements[1]).Name);
            Assert.AreEqual("T", ((NamedType)((ListType)type.Elements[2]).Element).Name);
            Assert.AreEqual("B", ((NamedType)((FuncType)type.Elements[3]).Input).Name);
            Assert.AreEqual("C", ((NamedType)((FuncType)type.Elements[3]).Output).Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void Type_DoesNotParse_TupleWithTrailingComma()
        {
            Type.Parser.Parse("(A, )");
        }

        [TestMethod]
        public void Type_ParsesListType()
        {
            var type = (ListType)Type.Parser.Parse("[T]");

            Assert.AreEqual("T", ((NamedType)type.Element).Name);
        }

        [TestMethod]
        public void Type_ParsesFuncType()
        {
            var type = (FuncType)Type.Parser.Parse("{A -> B}");

            Assert.AreEqual("A", ((NamedType)type.Input).Name);
            Assert.AreEqual("B", ((NamedType)type.Output).Name);
        }

        [TestMethod]
        public void Type_ParsesFuncType_WithComplexInputOutput()
        {
            var type = (FuncType)Type.Parser.Parse("{(A,[B],(C,{D->E}))->[{(A)->B}]}");

            var input = (TupleType)type.Input;

            Assert.AreEqual("A", ((NamedType)input.Elements[0]).Name);
        }
    }
}
