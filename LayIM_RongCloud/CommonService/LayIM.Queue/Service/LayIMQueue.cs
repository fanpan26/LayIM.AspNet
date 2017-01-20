using EasyNetQ;
using LayIM.Container;
using LayIM.Queue.Config;
using LayIM.Queue.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Queue.Service
{
    public class LayIMQueue : ILayIMQueue
    {
        private IBus _bus;
        private object _lock = new object();
        private IBus GetBus()
        {
            if (_bus == null)
            {
                lock (_lock)
                {
                    if (_bus == null)
                    {
                        var queueConfig = LayIMGlobalServiceContainer.GlobalContainer.Resolve<IQueueConfig>();
                        var bus = RabbitHutch.CreateBus(queueConfig.Config, x => x.Register<IEasyNetQLogger, EasyNetQ.Loggers.NullLogger>());
                        _bus = bus;
                    }
                }
            }
            return _bus;
        }
        public void Publish<T>(T message) where T : class
        {
            var bus = GetBus();
            //发送到队列
            bus.PublishAsync(message);

        }

        public void Subscribe<T>(string subscribeId, Action<T> messageHandler) where T : class
        {
            var bus = GetBus();
            //发送到队列
            bus.Subscribe(subscribeId, messageHandler);
        }
    }
}
