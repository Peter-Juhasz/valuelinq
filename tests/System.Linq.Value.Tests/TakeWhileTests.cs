using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class TakeWhileTests
    {
        [TestMethod]
        public void TakeWhile()
        {
            var array = new int[] { 2, 1, 5 };
            var result = array.ValueTakeWhile(i => i < 3).ToArray();
            var expected = new int[] { 2, 1 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
