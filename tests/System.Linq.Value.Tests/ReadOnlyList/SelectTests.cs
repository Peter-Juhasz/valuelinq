using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.ReadOnlyLists.Tests
{
    [TestClass]
    public class SelectTests
    {
        [TestMethod]
        public void Select()
        {
            var array = new int[] { 2, 1, 5 };
            var result = array.ValueSelect(i => 2 * i).ToArray();
            var expected = new int[] { 4, 2, 10 };
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SelectWithIndex()
        {
            var array = new int[] { 2, 1, 5 };
            var result = array.ValueSelect((v, i) => i).ToArray();
            var expected = new int[] { 0, 1, 2 };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
