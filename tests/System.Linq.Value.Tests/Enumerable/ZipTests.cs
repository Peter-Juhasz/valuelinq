using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class ZipTests
    {
        [TestMethod]
        public void Zip()
        {
            var array = new int[] { 2, 1, 5 };
            var result = array.ValueZip(new[] { 2, 3 }, (x, y) => x + y).ToArray();
            var expected = new int[] { 4, 4 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
