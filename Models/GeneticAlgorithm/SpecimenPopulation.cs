using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodCutterCalculator.Models.GeneticAlgorithm
{
    public class SpecimenPopulation
    {
        private static readonly Random _randomNumber = new Random();
        private byte _numberOfPossibleCutsPerPlank = 9;
        public byte[] PackOfPlanks { get; set; } //Genom - One plank is 10-bit vector. This is pack of planks.
        public byte[] Population { get; set; }

        public SpecimenPopulation(GeneticAlgorithmParameters algorithmParameters)
        {
            var sizeOfPopulation = algorithmParameters.SizeOfPopulation;
            var sizeOfOnePopulation = algorithmParameters.NumberOfPlanksPerPack * _numberOfPossibleCutsPerPlank;

            PackOfPlanks = new byte[sizeOfOnePopulation];
            Population = new byte[sizeOfPopulation * sizeOfOnePopulation];

            CreateFirstPopulation(sizeOfPopulation, sizeOfOnePopulation);
        }

        private void CreateFirstPopulation(int sizeOfPopulation, int sizeOfOnePopulation)
        {
            for (int i = 0; i < sizeOfPopulation; i++)
            {
                for (int j = 0; j < sizeOfOnePopulation; j++)
                {
                    //66% - 0 (no cuts), 33% - 1 (cuts)
                    PackOfPlanks[j] = _randomNumber.Next(3) < 2 ? (byte)0 : (byte)1;
                }
                PackOfPlanks.CopyTo(Population, i * sizeOfOnePopulation);
            }
        }
    }
}
