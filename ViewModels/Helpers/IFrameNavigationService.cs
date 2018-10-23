using GalaSoft.MvvmLight.Views;

namespace WoodCutterCalculator.ViewModels.Helpers
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
    }
}
