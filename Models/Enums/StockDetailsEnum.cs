namespace WoodCutterCalculator.Models.Enums
{
    public enum StockDetailsEnum
    {
        Useless = 0,
        //MultipleConst * MultipleMinorStockUnit + StockClassEnum
        FirstClassStock40 = 10 * 8 + 1, 
        FirstClassStock30 = 10 * 6 + 1,
        FirstClassStock20 = 10 * 4 + 1,
        SecondClassStock40 = 10 * 8 + 2,
        SecondClassStock30 = 10 * 6 + 2,
        SecondClassStock20 = 10 * 4 + 2,
        ThirdClassStock40 = 10 * 8 + 3,
        ThirdClassStock30 = 10 * 6 + 3,
        ThirdClassStock20 = 10 * 4 + 3,
    }
}
