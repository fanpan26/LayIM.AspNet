using LayIM.Utils.Extension;
using Macrosage.ElasticSearch.Core;
using Macrosage.ElasticSearch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.ElasticSearch.Log
{
    public static class ESLog
    {
        static Elastic<LogInfo> log
        {

            get
            {
                var l = new Elastic<LogInfo>();
                l.SetIndexInfo("Log", "Info");
                return l;
            }
        }

        #region 私有方法
        private static void WriteLog(LogInfo info)
        {
            try
            {
                log.Index(info);
            }
            catch(Exception ex)
            {
                FileStream fs = new FileStream("D:\\ak.txt", FileMode.Create);
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes(ex.Message);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
            }
        }

        private static void WriteLog(MethodBase method,string title,string detail,LogType type)
        {
            var info = new LogInfo
            {
                id = Guid.NewGuid().ToString(),
                detail = detail,
                logtitle = title,
                logtype = type,
                methodname = method.Name,
                methodparameters = method.DeclaringType.FullName
            };
            Task.Run(() =>
            {
                WriteLog(info);
            });

        }
        #endregion

        #region 添加日志
        public static void WriteLogException(MethodBase method,string detail, string title="程序异常")
        {
            WriteLog(method, title, detail, LogType.Exception);
        }

        public static void WriteLogException(MethodBase method, Exception ex,string title="程序异常")
        {
            WriteLogException(method, ex.ToJson(), title);
        }
        public static void WriteLog(MethodBase method, string detail, string title = "普通日志")
        {
            WriteLog(method, title, detail, LogType.Info);
        }

        public static void WriteLogQuartz(MethodBase method, string detail, string title = "任务调度日志")
        {
            WriteLog(method, title, detail, LogType.Quartz);
        }
        #endregion
    }
}
