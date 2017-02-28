using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Queue.Config
{
    public class LayIMQueueConfig : IQueueConfig
    {
        private string GetConfig(string key) => ConfigurationManager.AppSettings[key];

        private string ServerName => GetConfig("Queue_ServerName");
        private string VirtualHost => GetConfig("Queue_VritualHost");
        private string UserName => GetConfig("Queue_UserName");
        private string UserPassword => GetConfig("Queue_Password");
        public string Config => $"host={ServerName};virtualHost={VirtualHost};username={UserName};password={UserPassword}";
    }
}
