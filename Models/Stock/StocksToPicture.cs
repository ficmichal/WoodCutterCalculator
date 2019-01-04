using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.GeneticAlgorithm;

namespace WoodCutterCalculator.Models.Stock
{
    public class StocksToPicture
    {
        public List<byte[]> Genotypes { get; set; }
        public int[][] Planks { get; set; }
        public List<double> Values { get; set; }
        public List<int[]> CuttedStocks { get; set; }

        public StocksToPicture(byte[] genotype, int[][] planks, int[] cuttedStocks, double value)
        {
            Genotypes = new List<byte[]> { genotype };
            Planks = planks;
            CuttedStocks = new List<int[]> { cuttedStocks };
            Values = new List<double> { value };
        }

        public void Add(byte[] genotype,  int[] cuttedStocks, double value)
        {
            Genotypes.Add(genotype);
            CuttedStocks.Add(cuttedStocks);
            Values.Add(value);
        }
    }
}
