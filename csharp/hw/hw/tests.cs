using NUnit.Framework;

namespace hw
{
    [TestFixture]
    public class tests
    {
        [Test]
        public void Test1()
        {
            var lru = new LRU<int, string>();
            lru.Add(1,"abc");
            lru.Add(2,"aaa");
            lru.RemoveLastRecentlyUsed();
            Assert.AreEqual(1, lru.Count);
            Assert.AreEqual("aaa", lru.Get(2));
        }
        
        [Test]
        public void Test2()
        {
            var lru = new LRU<int, string>();
            lru.Add(1,"abc");
            lru.Add(2,"aaa");
            lru.Get(1);
            lru.RemoveLastRecentlyUsed();
            Assert.AreEqual(1, lru.Count);
            Assert.AreEqual("abc", lru.Get(1));
        }
    }
}