using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLApp;

namespace WoodCutterCalculator.Models.Matlab
{
    public class MatlabClient
    {
        private static MLApp.MLApp _matlab;

        public static MLApp.MLApp Create(string path)
        {
            _matlab = new MLApp.MLApp();
            _matlab.Execute($"cd {path}");

            return _matlab;
        }
    }
}
