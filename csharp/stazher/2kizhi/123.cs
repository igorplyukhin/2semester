using System.IO;
using KizhiPart2;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace yield
{
    public class Program
    {
        public static void Main()
        {
            var i = new Interpreter(new StringWriter());
            var s = "set code\ndef test\n    rem a\nset a 12\ncall test\nprint a\nend set code\nrun";
            var s1 = s.Split('\n');
            foreach (var e in s1)
            {
                i.ExecuteLine(e);
            }
            Assert.AreEqual("Переменная отсутствует в памяти\r\n", i);
        }
        
    }
}