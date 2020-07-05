using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class AppendTests
    {
        [TestMethod]
        public void Append()
        {
            var array = new int[] { 2, 1, 5, 3 };
            var result = array.ValueAppend(2).ToArray();
            var expected = new int[] { 2, 1, 5, 3, 2 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
