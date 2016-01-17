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
            var type = TestHarness.TestPositive(Type.parser, "()");

            Assert.IsTrue(type.IsT);
            Assert.IsTrue(((Type.TypeSignature.T)type).Item.ElementTypes.IsEmpty);
        }

        [TestMethod]
        public void Type_DoesNotParse_UnbalancedBrackets()
        {
            TestHarness.TestNegative(Type.parser, "(()");
        }

        [TestMethod]
        public void Type_ParsesNamedType()
        {
            var type = TestHarness.TestPositive(Type.parser, "T");

            Assert.IsTrue(type.IsN);
            Assert.AreEqual("T", ((Type.TypeSignature.N)type).Item.Name);
        }

        [TestMethod]
        public void Type_DoesNotParse_InvalidNamedType()
        {
            TestHarness.TestNegative(Type.parser, "1");
        }

        [TestMethod]
        public void Type_DoesNotParse_TupleWithTrailingComma()
        {
            TestHarness.TestNegative(Type.parser, "(A, )");
        }

        [TestMethod]
        public void Type_ParsesTupleType()
        {
            var type = TestHarness.TestPositive(Type.parser, "(A, (), (A -> B))");

            Assert.IsTrue(type.IsT);

            var tuple = (Type.TypeSignature.T)type;
            Assert.AreEqual("A", ((Type.TypeSignature.N)tuple.Item.ElementTypes.Head).Item.Name);
            Assert.IsTrue(tuple.Item.ElementTypes.Tail.Head.IsT);
        }

        [TestMethod]
        public void Type_ParsesListType()
        {
            var type = TestHarness.TestPositive(Type.parser, "[T]");

            Assert.IsTrue(type.IsL);
            var list = (Type.TypeSignature.L)type;

            Assert.AreEqual("T", ((Type.TypeSignature.N)list.Item.ElementType).Item.Name);
        }

        [TestMethod]
        public void Type_ParsesFuncType()
        {
            var type = TestHarness.TestPositive(Type.parser, "(A -> B)");

            Assert.IsTrue(type.IsF);

            var func = (Type.TypeSignature.F)type;
            Assert.AreEqual("A", (func.Item.Input as Type.TypeSignature.N)?.Item.Name);
            Assert.AreEqual("B", (func.Item.Output as Type.TypeSignature.N)?.Item.Name);
        }
    }
}
