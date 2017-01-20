using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Queue.Service
{
    /// <summary>
    /// 简单队列调用
    /// </summary>
    public interface ILayIMQueue
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        void Publish<T>(T message) where T : class;
        /// <summary>
        /// 接收消息 
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="messageHandler">处理消息逻辑</param>
        void Subscribe<T>(string subscribeId, Action<T> messageHandler) where T : class;
    }
}
