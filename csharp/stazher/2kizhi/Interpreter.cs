using System.IO;

namespace KizhiPart2
{
    public class Interpreter
    {
        
        private TextWriter _writer;

        public Interpreter(TextWriter writer)
        {
            _writer = writer;
        }

        public void ExecuteLine(string command)
        {
        }
    }
}