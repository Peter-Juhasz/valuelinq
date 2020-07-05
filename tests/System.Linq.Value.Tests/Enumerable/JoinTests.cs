using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class JoinTests
    {
        [TestMethod]
        public void Join()
        {
            var array = new int[] { 2, 1, 5 };
            var result = array.ValueJoin(new[] { 2, 5 }, x => x, y => y, (x, y) => x * y).ToArray();
            var expected = new int[] { 4, 0, 25 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
