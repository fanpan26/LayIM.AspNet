using Macrosage.RabbitMQ.Server.Config;
using Macrosage.RabbitMQ.Server.Customer;
using Macrosage.RabbitMQ.Server.Product;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.RabbitMQ.Server
{
    public class RabbitMQFactory
    {
        #region 单例
        private RabbitMQFactory() {
            Customers = new Dictionary<string, IMessageCustomer>();
            Products = new Dictionary<string, IMessageProduct>();
        }
        private static RabbitMQFactory _instance;

        private static object _lock = new object();
        public static RabbitMQFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RabbitMQFactory();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region 私有变量
        private ConnectionFactory _factory { get; set; }

        private Dictionary<string, IMessageCustomer> Customers { get; set; }

        private Dictionary<string, IMessageProduct> Products { get; set; }
        #endregion

        #region 创建工厂
        public ConnectionFactory CreateFactory()
        {
            if (_factory == null) {

                const ushort heartbeat = 0;
                //主机地址
                Uri uri = new Uri(RabbitMQConfig.HostUri);

                _factory = new ConnectionFactory();
                //_factory.HostName = RabbitMQConfig.HostName;
                //用户名
                _factory.UserName = RabbitMQConfig.UserName;
                //密码
                _factory.Password = RabbitMQConfig.PassWord;
                //虚拟主机名
                _factory.VirtualHost = RabbitMQConfig.VirtualHost;
                //连接终端
                _factory.Endpoint = new AmqpTcpEndpoint(uri);

                _factory.RequestedHeartbeat = heartbeat;
                //自动重连
                _factory.AutomaticRecoveryEnabled = true;
            }
            return _factory;
        }
        #endregion

        #region 创建消费者

        public IMessageCustomer CreateCustomer(string queueName)
        {
            IMessageCustomer customer;
            if (!Customers.ContainsKey(queueName))
            {
                customer = new MessageCustomer(queueName);
                Customers.Add(queueName, customer);
            }
            else
            {
                customer = Customers[queueName];
            }
            return customer;
        }

        public IMessageProduct CreateProduct(string queueName)
        {
            IMessageProduct product;
            if (!Products.ContainsKey(queueName))
            {
                product = new MessageProduct(queueName);
                Products.Add(queueName, product);
            }
            else
            {
                product = Products[queueName];
            }
            return product;
        }

        public IDictionary<string, object> ClientProperties()
        {
            var factory = CreateFactory();
            return factory.ClientProperties;
        }

        #endregion
    }
}
