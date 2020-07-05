using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.StringTokenizers.Tests
{
    [TestClass]
    public class SelectTests
    {
        [TestMethod]
        public void Select()
        {
            var array = new StringTokenizer("abc,23,,fg", new char[] { ',' });
            var result = array.ValueSelect(i => i.Length).ToArray();
            var expected = new int[] { 3, 2, 0, 2 };
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SelectWithIndex()
        {
            var array = new StringTokenizer("abc,23,,fg", new char[] { ',' });
            var result = array.ValueSelect((s, i) => i + s.Length).ToArray();
            var expected = new int[] { 3, 3, 2, 5 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
