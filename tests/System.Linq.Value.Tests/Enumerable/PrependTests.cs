using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class PrependTests
    {
        [TestMethod]
        public void Prepend()
        {
            var array = new int[] { 2, 1, 5, 3 };
            var result = array.ValuePrepend(2).ToArray();
            var expected = new int[] { 2, 2, 1, 5, 3 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
