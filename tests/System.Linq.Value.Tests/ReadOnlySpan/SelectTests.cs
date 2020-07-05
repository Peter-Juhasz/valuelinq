using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.ReadOnlySpans.Tests
{
    [TestClass]
    public class SelectTests
    {
        [TestMethod]
        public void Select()
        {
            var array = new ReadOnlySpan<int>(new int[] { 2, 1, 5 });
            var result = array.ValueSelect(i => 2 * i);
            var expected = new int[] { 4, 2, 10 };
            var index = 0;
            foreach (var item in result)
            {
                Assert.AreEqual(expected[index], item);
                index++;
            }
        }

        [TestMethod]
        public void SelectWithIndex()
        {
            var array = new ReadOnlySpan<int>(new int[] { 2, 1, 5 });
            var result = array.ValueSelect((v, i) => i);
            var expected = new int[] { 0, 1, 2 };
            var index = 0;
            foreach (var item in result)
            {
                Assert.AreEqual(expected[index], item);
                index++;
            }
        }
    }
}
