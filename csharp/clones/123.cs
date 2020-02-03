using NUnit.Framework;

namespace Clones
{
    [TestFixture]
    public class gfg
    {
        [Test]
        public void test()
        {
            var s= new LinkedListStack<int>(null);
            s.Push(5);
            s.Push(10);
            Assert.AreEqual(10, s.Pop());
        }
        
        [Test]
        public void test1()
        {
            var s= new LinkedListStack<int>(null);
            s.Push(5);
            s.Push(10);
            s.Pop();
            Assert.AreEqual(1, s.Count);
        }
    }
}