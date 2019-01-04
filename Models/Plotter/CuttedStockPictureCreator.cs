using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Enums;
using WoodCutterCalculator.Models.PlotDatas;
using WoodCutterCalculator.Models.Stock;

namespace WoodCutterCalculator.Models.Plotter
{
    public static class CuttedStockPictureCreator
    {
        private const int _sizeOfRect = 24;
        private const int _spaceBetweenPlanks = 25;

        public static ObservableCollection<MinorElementOfStock> CreatePicture(PicturedDatas picturedDatas, IterationsEnum iteration)
        {
            (var startX, var startY) = GetStartedPoint(iteration);
            var index = GetIndexOfGenotypes(iteration);
            var collectionOfElements = new ObservableCollection<MinorElementOfStock>();

            for (int i = 0; i < picturedDatas.AlgorithmParameters.NumberOfPlanksPerPack; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    var minorElement = new MinorElementOfStock(startX + (i * (_spaceBetweenPlanks + _sizeOfRect)), 
                        startY + (j * _sizeOfRect), _sizeOfRect - 1, _sizeOfRect - 1,
                        RenderCut(picturedDatas.FirstAndLastStocksToPicture.Genotypes[index].ElementAt(i * 19 + j)),
                        RenderImage(picturedDatas.FirstAndLastStocksToPicture.Planks[i].ElementAt(j)));
                    collectionOfElements.Add(minorElement);
                }
                var minorlastElement = new MinorElementOfStock(startX + (i * (_spaceBetweenPlanks + _sizeOfRect)),
                    startY + (19 * _sizeOfRect), _sizeOfRect, _sizeOfRect,
                    string.Empty,
                    RenderImage(picturedDatas.FirstAndLastStocksToPicture.Planks[i].ElementAt(19)));
                collectionOfElements.Add(minorlastElement);
            }

            return collectionOfElements;
        }

        private static (int startX, int startY) GetStartedPoint(IterationsEnum iteration)
        {
            switch (iteration)
            {
                case IterationsEnum.FirstIteration:
                    return (78, 20);
                case IterationsEnum.LastIteration:
                default:
                    return (78, 600);
            }
        }

        private static int GetIndexOfGenotypes(IterationsEnum iteration)
        {
            switch (iteration)
            {
                case IterationsEnum.FirstIteration:
                    return 0;
                case IterationsEnum.LastIteration:
                default:
                    return 1;
            }
        }

        private static string RenderCut(byte possibleCut)
        {
            if(possibleCut == 1)
            {
                return "Red";
            }
            else
            {
                return string.Empty;
            }
        }

        private static string RenderImage(int @class)
        {
            switch (@class)
            {
                case 1:
                    return "images/Iclass.bmp";
                case 2:
                    return "images/IIclass.bmp";
                default:
                case 3:
                    return "images/IIIclass.bmp";
            }
        }
    }
}
