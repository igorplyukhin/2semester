using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class ParsingTask
    {
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines
                .Skip(1)
                .Select(x => x.Split(';'))
                .Where(x => x.Length == 3
                            && int.TryParse(x[0], out var slideId)
                            && Enum.TryParse(x[1], true, out SlideType slideType))
                .Select(x =>
                {
                    var slideId = int.Parse(x[0]);
                    var slideType = (SlideType) Enum.Parse(typeof(SlideType), x[1], true);
                    var unitTitle = x[2];
                    return new SlideRecord(slideId, slideType, unitTitle);
                })
                .ToDictionary(x => x.SlideId, x => x);
        }

        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            return lines
                .Skip(1)
                .Select(x =>
                {
                    try
                    {
                        var splittedLine = x.Split(';');
                        var userId = int.Parse(splittedLine[0]);
                        var slideId = int.Parse(splittedLine[1]);
                        var dateTime = DateTime.Parse($"{splittedLine[2]} {splittedLine[3]}");
                        var slideType = slides[slideId].SlideType;
                        return new VisitRecord(userId, slideId, dateTime, slideType);
                    }
                    catch
                    {
                        throw new FormatException($"Wrong line [{x}]");
                    }
                });
        }
    }
}