using System;
using WoodCutterCalculator.Models.Mongo;

namespace WoodCutterCalculator.Models.Managers
{
    public interface ISettingsManager
    {
        MongoSettings GetSettings();
    }
}
