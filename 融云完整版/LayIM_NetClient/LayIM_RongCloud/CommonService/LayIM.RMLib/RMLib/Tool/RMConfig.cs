using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.IMServer.RMLib.Tool
{
    internal class RMConfig
    {
        public static string RM_AppKey => ConfigurationManager.AppSettings["IM_APPKEY"];
        public static string RM_AppSecret => ConfigurationManager.AppSettings["IM_APPSECRET"];

    }
}
