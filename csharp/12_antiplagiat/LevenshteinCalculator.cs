using System;
using System.Collections.Generic;
using System.Linq;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var docPairs = new List<ComparisonResult>();
            for (var i = 0; i < documents.Count - 1; i++)
            for (var j = i + 1; j < documents.Count; j++)
            {
                var doc1 = documents[i];
                var doc2 = documents[j];
                docPairs.Add(new ComparisonResult(doc1, doc2, CalcLevenshteinDistance(doc1, doc2)));
            }

            return docPairs;
        }


        private static double CalcLevenshteinDistance(DocumentTokens first, DocumentTokens second)
        {
            var opt = new double[first.Count + 1, second.Count + 1];
            for (var i = 0; i <= first.Count; ++i)
                opt[i, 0] = i;
            for (var i = 0; i <= second.Count; ++i)
                opt[0, i] = i;
            for (var i = 1; i <= first.Count; ++i)
            for (var j = 1; j <= second.Count; ++j)
            {
                var token1 = first[i - 1];
                var token2 = second[j - 1];
                if (token1 == token2)
                    opt[i, j] = opt[i - 1, j - 1];
                else
                {
                    var replaceCost = TokenDistanceCalculator.GetTokenDistance(token1, token2);
                    opt[i, j] = Helper.GetMinValue(1 + opt[i - 1, j],
                        replaceCost + opt[i - 1, j - 1], 1 + opt[i, j - 1]); 
                }
            }

            return opt[first.Count, second.Count];
        }
    }

    public static class Helper
    {
        public static T GetMinValue<T>(params T[] args) => args.Min();
    }
}