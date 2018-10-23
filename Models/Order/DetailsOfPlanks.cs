using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodCutterCalculator.Models.Order
{
    public class DetailsOfPlanks
    {
        public static Dictionary<string, double> Prices()
        {
            return new Dictionary<string, double>
            {
                { "40_FirstClassPlankPrice", 25.0 },
                { "30_FirstClassPlankPrice", 17.0 },
                { "20_FirstClassPlankPrice", 15.0 },
                { "40_SecondClassPlankPrice", 18.0 },
                { "30_SecondClassPlankPrice", 11.0 },
                { "20_SecondClassPlankPrice", 10.0 },
                { "40_ThirdClassPlankPrice", 13.0 },
                { "30_ThirdClassPlankPrice", 7.0 },
                { "20_ThirdClassPlankPrice", 5.0 },
            };
        }

        public static Dictionary<string, double> LossesPer5Cm()
        {
            return new Dictionary<string, double>
            {
                { "FirstClassLoss", 3.0 },
                { "SecondClassLoss", 2.0 },
                { "ThirdClassLoss", 1.0 },
            };
        }
    }
}
