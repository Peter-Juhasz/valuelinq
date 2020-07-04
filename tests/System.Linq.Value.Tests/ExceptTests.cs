using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class ExceptTests
    {
        [TestMethod]
        public void Except()
        {
            var array = new int[] { 2, 1, 5 };
            var result = array.ValueExcept(new[] { 2, 3 }).ToArray();
            var expected = new int[] { 1, 5 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
