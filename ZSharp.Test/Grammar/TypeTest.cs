using Microsoft.FSharp.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Type = ZSharp.Parser.Type;

namespace ZSharp.Test.Grammar
{
    [TestClass]
    public class TypeTest
    {
        [TestMethod]
        public void Type_ParsesUnitType()
        {
            var type = TestHarness.TestPositive(Type.anyType, "()");

            Assert.IsTrue(type.IsTuple);
            Assert.IsTrue(((Type.TypeSignature.Tuple)type).Item.IsEmpty);
        }

        [TestMethod]
        public void Type_DoesNotParse_UnbalancedBrackets()
        {
            TestHarness.TestNegative(Type.anyType, "(()");
        }

        [TestMethod]
        public void Type_ParsesNamedType()
        {
            var type = TestHarness.TestPositive(Type.anyType, "T");

            Assert.IsTrue(type.IsName);
            Assert.AreEqual("T", ((Type.TypeSignature.Name)type).Item);
        }

        [TestMethod]
        public void Type_DoesNotParse_InvalidNamedType()
        {
            TestHarness.TestNegative(Type.anyType, "1");
        }

        [TestMethod]
        public void Type_DoesNotParse_TupleWithTrailingComma()
        {
            TestHarness.TestNegative(Type.anyType, "(A, )");
        }

        [TestMethod]
        public void Type_ParsesTupleType()
        {
            var type = TestHarness.TestPositive(Type.anyType, "(A, B, {A -> B}, ((), C))");

            Assert.IsTrue(type.IsTuple);

            var tuple = (Type.TypeSignature.Tuple)type;
            Assert.AreEqual("A", (tuple.Item.Head as Type.TypeSignature.Name)?.Item);
            Assert.AreEqual("B", (tuple.Item.Tail.Head as Type.TypeSignature.Name)?.Item);
        }

        [TestMethod]
        public void Type_ParsesListType()
        {
            var type = TestHarness.TestPositive(Type.anyType, "[T]");

            Assert.IsTrue(type.IsList);
            Assert.AreEqual("T", ((Type.TypeSignature.Name)((Type.TypeSignature.List)type).Item).Item);

            Assert.AreEqual("T", ((type as Type.TypeSignature.List).Item as Type.TypeSignature.Name)?.Item);
        }

        [TestMethod]
        public void Type_ParsesFuncType()
        {
            var type = TestHarness.TestPositive(Type.anyType, "{A -> B}");

            Assert.IsTrue(type.IsFunction);

            var func = (Type.TypeSignature.Function)type;
            Assert.AreEqual("A", (func.Item1 as Type.TypeSignature.Name)?.Item);
            Assert.AreEqual("B", (func.Item2 as Type.TypeSignature.Name)?.Item);
        }
    }
}
