namespace WoodCutterCalculator.Models.Enums
{
    public enum StockDetailsEnum
    {
        //MultipleConst * CoefficientCut * MultipleMinorStockUnit + StockClassEnum
        Useless = 0,
        FirstClassStock40 = 10 * 2 * 4 + 1, 
        FirstClassStock30 = 10 * 2 * 3 + 1,
        FirstClassStock20 = 10 * 2 * 2 + 1,
        SecondClassStock40 = 10 * 2 * 4 + 2,
        SecondClassStock30 = 10 * 2 * 3 + 2,
        SecondClassStock20 = 10 * 2 * 2 + 2,
        ThirdClassStock40 = 10 * 2 * 4 + 3,
        ThirdClassStock30 = 10 * 2 * 3 + 3,
        ThirdClassStock20 = 10 * 2 * 2 + 3,
    }
}
