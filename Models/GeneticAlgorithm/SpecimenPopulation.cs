using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Extensions;

namespace WoodCutterCalculator.Models.GeneticAlgorithm
{
    public class SpecimenPopulation
    {
        private static readonly Random _randomNumber = new Random();
        private int _sizeOfPopulation;
        private int _sizeOfSpecimen;
        private double _mutationRate;
        private double _percentageOfChildrenFromPreviousGeneration;
        private double _percentageOfParentsChosenToSelection;

        public byte[] PackOfPlanks { get; set; } //Genom - One plank is 10-bit vector. This is pack of planks.
        public byte[] Population { get; set; }

        public SpecimenPopulation(GeneticAlgorithmParameters algorithmParameters)
        {
            _percentageOfChildrenFromPreviousGeneration = algorithmParameters.PercentageOfChildrenFromPreviousGeneration;
            _percentageOfParentsChosenToSelection = algorithmParameters.PercentageOfParentsChosenToSelection;
            _sizeOfPopulation = algorithmParameters.SizeOfPopulation;
            _sizeOfSpecimen = algorithmParameters.NumberOfPlanksPerPack * (algorithmParameters.LengthOfPlank - 1);
            _mutationRate = algorithmParameters.MutationRate;

            PackOfPlanks = new byte[_sizeOfSpecimen];
            Population = new byte[_sizeOfPopulation * _sizeOfSpecimen];

            CreateFirstPopulation(_sizeOfPopulation, _sizeOfSpecimen);
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

        public void UpdatePopulation(SortedDictionary<double, int> specimenClassification)
        {
            var newPopulation = new byte[_sizeOfPopulation * _sizeOfSpecimen];
            int numberOfChildrenFromParents = (int)(_sizeOfPopulation * _percentageOfChildrenFromPreviousGeneration) / 2;
            int numberOfSurivorSpecimens = (int)((1.0 - _percentageOfChildrenFromPreviousGeneration) * _sizeOfPopulation);

            newPopulation = AddBestSpecimensFromPreviousPopulationToNextPopulation(newPopulation, specimenClassification, numberOfSurivorSpecimens);
            //crossover
            for (int i = 0; i < numberOfChildrenFromParents; i++)
            {
                Array.Copy(CrossoverTwoSpecimens(specimenClassification), 0, newPopulation, 
                    (numberOfSurivorSpecimens + 2 * i) * _sizeOfSpecimen, 2 * _sizeOfSpecimen);
            }
            //mutation
            newPopulation = Mutate(newPopulation);

            Population = newPopulation;
        }

        public byte[] AddBestSpecimensFromPreviousPopulationToNextPopulation
            (byte[] newPopulation, SortedDictionary<double, int> specimenClassification, int numberOfSurivorSpecimens)
        {
            for (int i = 0; i < numberOfSurivorSpecimens; i++)
            {
                Array.Copy(Population, specimenClassification.ElementAt(i).Value * _sizeOfSpecimen,
                    newPopulation, i * _sizeOfSpecimen, _sizeOfSpecimen);
            }
            return newPopulation;
        }

        public byte[] CrossoverTwoSpecimens(SortedDictionary<double, int> specimenClassification)
        {
            //Get accepted specimens to crossover from previous generation.
            int numberOfSpecimensChosenToSelection = (int)(_sizeOfPopulation * _percentageOfParentsChosenToSelection);
            var indexesOfSpecimensChosenToSelection = specimenClassification.Values.Take(numberOfSpecimensChosenToSelection).ToArray();

            //Take two parents from population
            var firstParent = SplitPopulationToSpecimens
                (indexesOfSpecimensChosenToSelection[_randomNumber.Next(0, numberOfSpecimensChosenToSelection - 1)]);
            var secondParent = SplitPopulationToSpecimens
                (indexesOfSpecimensChosenToSelection[_randomNumber.Next(0, numberOfSpecimensChosenToSelection - 1)]);

            //First cut in index range 10% - 40%
            var firstIndexOfCutToCrossover = _randomNumber.Next((int)(_sizeOfSpecimen * 0.1), (int)(_sizeOfSpecimen * 0.4));
            //Second cut in index range 60% - 90%
            var secondIndexOfCutToCrossover = _randomNumber.Next((int)(_sizeOfSpecimen * 0.6), (int)(_sizeOfSpecimen * 0.9));

            //Crossover two parents
            return Crossover(firstParent, secondParent, firstIndexOfCutToCrossover, secondIndexOfCutToCrossover);
        }

        public byte[] Crossover(byte[] firstParent, byte[] secondParent, int firstIndexOfCutToCrossover, int secondIndexOfCutToCrossover)
        {
            //Assignment
            byte[] crossoveredFirstSpecimen = new byte[firstParent.Length];
            Array.Copy(firstParent, crossoveredFirstSpecimen, firstParent.Length);
            byte[] crossoveredSecondSpecimen = new byte[firstParent.Length];
            Array.Copy(secondParent, crossoveredSecondSpecimen, secondParent.Length);

            //First crossover
            Array.Copy(secondParent, firstIndexOfCutToCrossover, crossoveredFirstSpecimen, 
                firstIndexOfCutToCrossover, secondIndexOfCutToCrossover - firstIndexOfCutToCrossover);
            //Second crossover
            Array.Copy(firstParent, firstIndexOfCutToCrossover, crossoveredSecondSpecimen,
                  firstIndexOfCutToCrossover, secondIndexOfCutToCrossover - firstIndexOfCutToCrossover);

            return crossoveredFirstSpecimen.Concat(crossoveredSecondSpecimen).ToArray();
        }

        public byte[] Mutate(byte[] newPopulation)
        {
            for (int i = 0; i < _sizeOfPopulation; i++)
            {
                byte[] mutateSpecimen;
                if (_randomNumber.NextDouble() < _mutationRate)
                {
                    mutateSpecimen = MutateSpecimen(SplitPopulationToSpecimens(i), _randomNumber.Next(_sizeOfSpecimen));
                    Array.Copy(mutateSpecimen, 0, newPopulation,
                        i, _sizeOfSpecimen);
                }
            }

            return newPopulation;
        }

        private byte[] MutateSpecimen(byte[] specimen, int index)
        {
            specimen[index] = (byte)(1 - specimen[index]);
            return specimen;
        }

        public byte[] SplitPopulationToSpecimens(int index)
            => Population.SubArray(index * _sizeOfSpecimen, _sizeOfSpecimen);
    }
}
