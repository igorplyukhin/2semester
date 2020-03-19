using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class ParsingTask
    {
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            var slides = new Dictionary<int, SlideRecord>();
            foreach (var e in lines)
            {
                var splittedLine = e.Split(';');
                if (splittedLine.Length < 2)
                    continue;
                if (splittedLine[0] == "SlideId")
                    continue;
                if (!int.TryParse(splittedLine[0], out var slideId))
                    continue;
                if (!Enum.TryParse(splittedLine[1], true, out SlideType slideType))
                    continue;
                var unitTitle = splittedLine[2];
                slides.Add(slideId, new SlideRecord(slideId, slideType, unitTitle));
            }

            return slides;
        }

        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            var visits = new List<VisitRecord>();
            foreach (var e in lines)
            {
                try
                {
                    var splittedLine = e.Split(';');
                    if (splittedLine[0] == "UserId")
                        continue;
                    var userId = int.Parse(splittedLine[0]);
                    var slideId = int.Parse(splittedLine[1]);
                    var dateTime = DateTime.Parse($"{splittedLine[2]} {splittedLine[3]}");
                    var slideType = slides[slideId].SlideType;
                    visits.Add(new VisitRecord(userId, slideId, dateTime, slideType));
                }
                catch
                {
                    throw new FormatException($"Wrong line [{e}]");
                }
            }

            return visits;
        }
    }
}