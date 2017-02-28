using LayIM.Container;
using LayIM.LogicLayer;
using LayIM.Queue.Service;
using LayIM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.QueueMessageHandler.Handler
{
    public class MessageQueueHandler
    {
        /// <summary>
        /// 注册消息队列处理逻辑
        /// </summary>
        public static void RegisterListener()
        {
            var queue = LayIMGlobalServiceContainer.GlobalContainer.Resolve<ILayIMQueue>();
            var service = LayIMGlobalServiceContainer.GlobalContainer.Resolve<IMessageService>();
            //监听收到日志信息
            queue.Subscribe<Model.Message.DB.ChatMessage>("layim_message", msg =>
            {
                Console.WriteLine($"{DateTime.Now} 收到通讯消息，正在处理...");

                service.AddMsg(msg);

                Console.WriteLine($"{DateTime.Now} 通讯消息处理完毕.");
            });
        }
    }
}
