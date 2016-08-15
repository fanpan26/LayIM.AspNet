using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.RabbitMQ.Server.Product
{
    public interface IMessageProduct
    {
        void Publish(string message, string queueName=null);
        void Publish(List<string> message, string queueName=null);
    }
}
