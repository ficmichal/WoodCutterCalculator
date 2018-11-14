using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodCutterCalculator.Models.Mongo;

namespace WoodCutterCalculator.Models.Managers
{
    public class SettingsManager : ISettingsManager
    {
        JObject _appSettings;

        public SettingsManager()
        {
            // Read AppSettings File
            try
            {
                _appSettings = JObject.Parse(File.ReadAllText(MongoSettings.GetSettingsFile()));
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
            }
        }

        public MongoSettings GetSettings()
        {
            // Get All Settings
            if (_appSettings.HasValues && (string)_appSettings["MongoConnection"]["ConnectionString"] != null && (string)_appSettings["MongoConnection"]["Database"] != null)
            {
                return new MongoSettings { ConnectionString = (string)_appSettings["MongoConnection"]["ConnectionString"], Database = (string)_appSettings["MongoConnection"]["Database"] };
            }
            else
                throw new Exception("Empty App Setings File !");
        }
    }
}
