using System.IO;

namespace WoodCutterCalculator.Models.Mongo
{
    public class MongoSettings
    {
        public static string GetSettingsFile()
        {
            var appSettings = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().IndexOf("WoodCutterCalculator\\")) + "WoodCutterCalculator\\WoodCutterCalculator\\AppSettings.json";

            return appSettings;        }

        internal string ConnectionString;
        internal string Database;
    }
}
