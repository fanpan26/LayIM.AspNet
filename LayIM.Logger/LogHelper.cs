using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Logger
{
    public sealed class LogHelper
    {
        private static ILog logger = LogManager.GetLogger(typeof(LogHelper));

        /// <summary>
        /// 简单的写一下日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteLog(string format, params object[] args)
        {
            logger.DebugFormat(format, args);
        }
        /// <summary>
        /// 异常记录
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteLog(Exception ex)
        {
            logger.Debug(ex);
        }
    }
}
