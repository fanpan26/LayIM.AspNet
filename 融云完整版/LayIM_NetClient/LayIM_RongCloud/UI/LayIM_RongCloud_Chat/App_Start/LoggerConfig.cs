using LayIM.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LayIM_RongCloud_Chat.App_Start
{
    public class LoggerConfig
    {

        /// <summary>
        /// 注册log4net服务配置
        /// </summary>
        public static void RegisterLogger()
        {
            string path = HttpContext.Current.Server.MapPath("/App_Data/log4net.config");
            LayIMLogger.RegisterLogger(path);
        }
    }
}