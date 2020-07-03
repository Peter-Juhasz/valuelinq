using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class TakeTests
    {
        [TestMethod]
        public void Take()
        {
            var array = new int[] { 2, 1, 5 };
            var result = array.ValueTake(2).ToArray();
            var expected = new int[] { 2, 1 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
