using System.ComponentModel;

namespace WoodCutterCalculator.Models.Enums
{
    public enum StockDetailsEnum
    {
        //MultipleConst * MultipleMinorStockUnit + StockClassEnum
        [Description("I - 20")]
        FirstClassStock20 = 10 * 4 + 1,
        [Description("I - 30")]
        FirstClassStock30 = 10 * 6 + 1,
        [Description("I - 40")]
        FirstClassStock40 = 10 * 8 + 1,
        [Description("II - 20")]
        SecondClassStock20 = 10 * 4 + 2,
        [Description("II - 30")]
        SecondClassStock30 = 10 * 6 + 2,
        [Description("II - 40")]
        SecondClassStock40 = 10 * 8 + 2,
        [Description("III - 20")]
        ThirdClassStock20 = 10 * 4 + 3,
        [Description("III - 30")]
        ThirdClassStock30 = 10 * 6 + 3,
        [Description("III - 40")]
        ThirdClassStock40 = 10 * 8 + 3,
        [Description("Odpad")]
        Useless = 0
    }
}
