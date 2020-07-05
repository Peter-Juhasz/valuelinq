using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class ConcatTests
    {
        [TestMethod]
        public void Concat()
        {
            var array = new int[] { 2, 1, 5 };
            var result = array.ValueConcat(new[] { 2, 3 }).ToArray();
            var expected = new int[] { 2, 1, 5, 2, 3 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
