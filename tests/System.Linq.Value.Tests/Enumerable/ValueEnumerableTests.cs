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
    }
}
