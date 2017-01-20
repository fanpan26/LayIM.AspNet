using LayIM.Container;
using LayIM.Logger;
using LayIM.Queue.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.QueueMessageHandler.Log
{
    public static class LogQueueHandler
    {
        /// <summary>
        /// 注册日志队列处理逻辑
        /// </summary>
        public static void RegisterLogListener()
        {
            var queue = LayIMGlobalServiceContainer.GlobalContainer.Resolve<ILayIMQueue>();
            //监听收到日志信息
            queue.Subscribe<Model.Log.LayIMLogModel>("layim_log", log =>
            {
                Console.WriteLine($"{DateTime.Now} 收到日志消息，正在处理...");
                //记录日志即可1
                var msg = $"[{ log.CreateTime.ToString()}]{(log.IsExceptionLog ? log.Exception.Message : log.Message)}";
                LogHelper.WriteLog(msg);
            });
        }
    }
}
