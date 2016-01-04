using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;
using ZSharp.Grammar.Function;

namespace ZSharp.Test
{
    [TestClass]
    public class Playground
    {
        [TestMethod]
        public void BasicQSort()
        {
            Function.Parser.Parse(@"
fun qsort : [T] -> [T]
    []        => []
    [x]       => [x]
    [x, ..xs] => qsort(lessthan(x), xs)) ++ [x] ++ qsort(greaterthan(x), xs))
");
        }

        [TestMethod]
        public void SubstructuralQSort()
        {
            Function.Parser.Parse(@"fun qsort : (uint, [T]) -> [T]
    (0, x)               => x
    (_, [])              => []
    (_, [x])             => [x]
    (c, [x, ..xs])       => qsortRecursive(c - 1, filter((a => a < x), xs)) ++ [x] ++ qsortRecursive(c - 1, filter((a => a > x), xs))
");
        }
    }
}
