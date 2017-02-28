using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayIM.Container;
using LayIM.Queue.Service;
using LayIM.Queue.Config;
using LayIM.Logger;
using LayIM.Service;
using LayIM.LogicLayer;

namespace LayIM.QueueMessageHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("正在注册日志配置...");
            var logConfigPath = AppDomain.CurrentDomain.BaseDirectory + "log4net.config";
            LayIMLogger.RegisterLogger(logConfigPath);

            Console.WriteLine("正在注册公共服务信息...");
            LayIMGlobalServiceContainer.GlobalContainer.Register<ILayIMQueue, LayIMQueue>()
                                                       .Register<IMessageService,MessageService>()
                                                       .Register<IQueueConfig, LayIMQueueConfig>();
            //注册数据层服务
            Console.WriteLine("正在注册数据服务信息...");
            LogicLayerService.RegisterDataService();

            Console.WriteLine("正在注册日志监听服务...");
            Handler.LogQueueHandler.RegisterListener();
            Handler.MessageQueueHandler.RegisterListener();

            Console.WriteLine("监听中...");
            Console.Read();
        }
    }
}
