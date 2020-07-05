using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class SkipTests
    {
        [TestMethod]
        public void Skip()
        {
            var array = new int[] { 2, 1, 5, 3 };
            var result = array.ValueSkip(2).ToArray();
            var expected = new int[] { 5, 3 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
