using LayIM.Utils.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.ElasticSearch
{
    /*
        ElasticSearch 配置读取类
    */
    public static class ElasticConfig
    {
        #region 私有方法
        private static string GetValue(string key)
        {
            return AppSettings.GetValue(key);
        }
        private static int GetValueInt(string key)
        {
            int result = 0;
            bool isInt = int.TryParse(GetValue(key), out result);
            return isInt ? result : 0;
        }
        #endregion

        
        /*
            HostName  default value is localhost
            */
        public static string HostName
        {
            get
            {
                const string HOST_NAME_KEY = "Elastic_HostName";
                return GetValue(HOST_NAME_KEY);
            }
        }

        /*
            Port default value is 9200
            */
        public static int Port
        {
            get
            {
                const string HOST_PORT_KEY = "Elastic_Port";
                return GetValueInt(HOST_PORT_KEY);
            }
        }

        public static int TimeOutSeconds
        {
            get
            {
                return 600 * 1000;
            }
        }
    }
}
