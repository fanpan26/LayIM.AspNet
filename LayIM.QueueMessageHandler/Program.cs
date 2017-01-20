using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayIM.Container;
using LayIM.Queue.Service;
using LayIM.Model.Message;
using LayIM.Queue.Config;
using LayIM.Logger;

namespace LayIM.QueueMessageHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("正在注册日志配置...");
            var logConfigPath = AppDomain.CurrentDomain.BaseDirectory + "log4net.config";
            LayIMLogger.RegisterLogger(logConfigPath);

            Console.WriteLine("正在注册服务信息...");
            LayIMGlobalServiceContainer.GlobalContainer.Register<ILayIMQueue, LayIMQueue>().Register<IQueueConfig, LayIMQueueConfig>();

            Console.WriteLine("正在注册日志监听服务...");
            Log.LogQueueHandler.RegisterLogListener();

            Console.WriteLine("监听中...");
            Console.Read();
        }
    }
}
