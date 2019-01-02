using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodCutterCalculator.Models.Extensions
{
    public static class EnumerableExtensions
    {
        public static double[] MultiAverage(this IEnumerable<double[]> collectionOfArrays)
        {
            var arrayLength = collectionOfArrays.First().Length;
            var result = new double[arrayLength];

            for (int i = 0; i < collectionOfArrays.Count(); i++)
            {
                var indexesResults = new List<double>();
                foreach (var indexResult in collectionOfArrays)
                {
                    indexesResults.Add(indexResult[i]);
                }
                result[i] = indexesResults.Average();
            }

            return result;
        }
    }
}
