using System;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class GhostsTask :
        IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
        IMagic
    {
        private List<Cat> cat = new List<Cat> {new Cat("barsik", "usual", DateTime.MaxValue)};
        private List<Vector> vector = new List<Vector> {new Vector(0, 0)};
        private List<Segment> segment = new List<Segment> {new Segment(new Vector(0, 0), new Vector(1, 1))};
        private List<Robot> robot = new List<Robot> {new Robot("a")};
        private static byte[] docContent = new byte[10];
        private List<Document> document = new List<Document> {new Document("a", Encoding.Unicode, docContent)};

        public void DoMagic()
        {
            cat[0].Rename("abc");
            vector[0].Add(new Vector(1, 1));
            segment[0].End.Add(new Vector(1, 1));
            Robot.BatteryCapacity += 10;
            docContent[0] += 101;
        }

        Vector IFactory<Vector>.Create() => vector[0];

        Segment IFactory<Segment>.Create() => segment[0];

        Document IFactory<Document>.Create() => document[0];

        Cat IFactory<Cat>.Create() => cat[0];

        Robot IFactory<Robot>.Create() => robot[0];
    }
}