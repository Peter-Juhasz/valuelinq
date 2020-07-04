using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class IntersectTests
    {
        [TestMethod]
        public void Intersect()
        {
            var array = new int[] { 2, 1, 5 };
            var result = array.ValueIntersect(new[] { 2, 3, 5 }).ToArray();
            var expected = new int[] { 2, 5 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
