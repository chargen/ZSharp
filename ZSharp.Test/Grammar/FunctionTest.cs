using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSharp.Parser;

namespace ZSharp.Test.Grammar
{
    [TestClass]
    public class FunctionTest
    {
        [TestMethod]
        public void FunctionHeader_Parses_FInput_FOutput()
        {
            var fun = TestHarness.TestPositive(Functions.function_parser,
@"fun test:(A -> B) -> (A -> B)
    where [ A : B ]
    let hello = 1");

            Assert.AreEqual("test", fun.Signature.Name);
        }
    }
}
