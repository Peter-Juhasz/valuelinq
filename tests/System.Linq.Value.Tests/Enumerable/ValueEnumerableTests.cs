using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Value.Tests
{
    [TestClass]
    public class ValueEnumerableTests
    {
        [TestMethod]
        public void Empty()
        {
            var enumerable = ValueEnumerable.Empty<int>();
            Assert.AreEqual(0, enumerable.Count());
        }

        [TestMethod]
        public void Value1()
        {
            var enumerable = ValueEnumerable.Create(5);
            Assert.AreEqual(1, enumerable.Count());
            Assert.AreEqual(5, enumerable.Single());
        }

        [TestMethod]
        public void Value2()
        {
            var enumerable = ValueEnumerable.Create(5, 7);
            Assert.AreEqual(2, enumerable.Count());
            Assert.AreEqual(5, enumerable.First());
            Assert.AreEqual(7, enumerable.Last());
        }
    }
}
