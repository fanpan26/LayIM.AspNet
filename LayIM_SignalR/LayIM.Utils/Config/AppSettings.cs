using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Utils.Config
{
    public class AppSettings
    {
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
