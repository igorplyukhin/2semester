using System;
using System.Text;

namespace hashes
{
    public class GhostsTask :
        IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
        IMagic
    {
        private Cat cat = new Cat("barsik", "usual", DateTime.MaxValue);
        private Vector vector =new Vector(0, 0);
        private Segment segment = new Segment(new Vector(0, 0), new Vector(1, 1));
        private Robot robot = new Robot("a");
        private static byte[] docContent = new byte[10];
        private Document document = new Document("a", Encoding.Unicode, docContent);

        public void DoMagic()
        {
            cat.Rename("abc");
            vector.Add(new Vector(1, 1));
            segment.End.Add(new Vector(1, 1));
            Robot.BatteryCapacity += 10;
            docContent[0] += 101;
        }

        Vector IFactory<Vector>.Create() => vector;

        Segment IFactory<Segment>.Create() => segment;

        Document IFactory<Document>.Create() => document;

        Cat IFactory<Cat>.Create() => cat;

        Robot IFactory<Robot>.Create() => robot;
    }
}