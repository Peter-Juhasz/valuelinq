using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class WhereTests
    {
        [TestMethod]
        public void Where()
        {
            var array = new int[] { 2, 1, 5 };
            var result = array.ValueWhere(i => i % 2 == 1).ToArray();
            var expected = new int[] { 1, 5 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
