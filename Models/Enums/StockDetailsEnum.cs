using System.ComponentModel;

namespace WoodCutterCalculator.Models.Enums
{
    public enum StockDetailsEnum
    {
        //MultipleConst * MultipleMinorStockUnit + StockClassEnum
        [Description("I klasa - 20cm")]
        FirstClassStock20 = 10 * 4 + 1,
        [Description("I klasa - 30cm")]
        FirstClassStock30 = 10 * 6 + 1,
        [Description("I klasa - 40cm")]
        FirstClassStock40 = 10 * 8 + 1,
        [Description("II klasa - 20cm")]
        SecondClassStock20 = 10 * 4 + 2,
        [Description("II klasa - 30cm")]
        SecondClassStock30 = 10 * 6 + 2,
        [Description("II klasa - 40cm")]
        SecondClassStock40 = 10 * 8 + 2,
        [Description("III klasa - 20cm")]
        ThirdClassStock20 = 10 * 4 + 3,
        [Description("III klasa - 30cm")]
        ThirdClassStock30 = 10 * 6 + 3,
        [Description("III klasa - 40cm")]
        ThirdClassStock40 = 10 * 8 + 3,
        [Description("Odpad")]
        Useless = 0
    }
}
