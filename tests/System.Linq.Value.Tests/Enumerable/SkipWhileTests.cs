using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class SkipWhileTests
    {
        [TestMethod]
        public void SkipWhile()
        {
            var array = new int[] { 2, 1, 5, 3 };
            var result = array.ValueSkipWhile(i => i < 3).ToArray();
            var expected = new int[] { 5, 3 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
