using log4net.Config;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Logger
{
    public class LayIMLogger
    {
        public static void RegisterLogger(string configPath)
        {
            var logCfg = new FileInfo(configPath);
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }
    }
}
