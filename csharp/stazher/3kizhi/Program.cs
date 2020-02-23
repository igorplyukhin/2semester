using System.Collections.Generic;
using System.IO;
using KizhiPart3;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace yield
{
    public class Program
    {
        public static void Main()
        {
            var i = new Debugger(new StringWriter());
            var s = new List<string>{"set code\n", "def test\n    rem a\nset a 12\ncall test\nprint a\n","end set code\n", "run"};
        foreach (var e in s)
            {
                i.ExecuteLine(e);
            }
            Assert.AreEqual("Переменная отсутствует в памяти\r\n", i);
        }
        
    }
}