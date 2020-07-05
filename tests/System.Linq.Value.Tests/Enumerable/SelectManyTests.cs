using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class SelectManyTests
    {
        [TestMethod]
        public void SelectMany()
        {
            var array = new int[] { 2, 1, 5 };
            var result = array.ValueSelectMany(i => new[] { 2 * i, 3 * i }).ToArray();
            var expected = new int[] { 4, 6, 2, 3, 10, 15 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
