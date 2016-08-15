using Macrosage.RabbitMQ.Server.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.RabbitMQ.Server.Customer {
    public class MessageCustomer : IMessageCustomer {
        public MessageCustomer() {
            _queueName = RabbitMQConfig.PUBLISH_GLOBAL_QUEUE;
        }
        public MessageCustomer(string queueName) {
            _queueName = queueName;
        }

        public Func<string, bool> ReceiveMessageCallback { get; set; }

        private IModel ListenChannel { get; set; }

        private string _queueName { get; set; }

        #region  开始监听消息
        /// <summary>
        /// 开始监听消息
        /// </summary>
        /// <param name="queueName"></param>
        public void StartListening() {
            Task.Factory.StartNew(() => {
                Consume();
            });

        }
        #endregion

        #region 消费消息
        /// <summary>
        /// 消费信息
        /// </summary>
        /// <param name="queueName"></param>
        public void Consume() {
            var factory = RabbitMQFactory.Instance.CreateFactory();

            var connection = factory.CreateConnection();

            connection.ConnectionShutdown += Connection_ConnectionShutdown;

            ListenChannel = connection.CreateModel();


            bool autoDeleteMessage = false;
            var queue = ListenChannel.QueueDeclare(_queueName, RabbitMQConfig.IsDurable, false, false, null);

            //公平分发,不要同一时间给一个工作者发送多于一个消息
            ListenChannel.BasicQos(0, 1, false);
            //创建事件驱动的消费者类型，不要用下边的死循环来消费消息
            var consumer = new EventingBasicConsumer(ListenChannel);
            consumer.Received += Consumer_Received;
            //消费消息
            ListenChannel.BasicConsume(_queueName, autoDeleteMessage, consumer);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e) {
            
        }

        /// <summary>
        /// 接收到消息触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Consumer_Received(object sender, BasicDeliverEventArgs args) {
            try {
                var body = args.Body;
                var message = Encoding.UTF8.GetString(body);
                //将消息业务处理交给外部业务
                bool result = ReceiveMessageCallback(message);
                if (result) {
                    if (ListenChannel != null && !ListenChannel.IsClosed) {
                        ListenChannel.BasicAck(args.DeliveryTag, false);
                    }
                }
                else {

                }

            }
            catch (Exception ex) {
                throw ex;
            }
        }

        #endregion

        #region 获取某个队列的未处理消息个数
        public uint MessageCount {
            get {

                if (ListenChannel == null)
                {
                    var factory = RabbitMQFactory.Instance.CreateFactory();
                    var connection = factory.CreateConnection();
                    ListenChannel = connection.CreateModel();
                }
                var queue = ListenChannel.QueueDeclare(_queueName, RabbitMQConfig.IsDurable, false, false, null);
                return queue.MessageCount;
            }
        }
        #endregion

    }
}
