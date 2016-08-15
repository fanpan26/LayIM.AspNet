using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.RabbitMQ.Server.Customer
{
    public interface IMessageCustomer
    {
        Func<string, bool> ReceiveMessageCallback { get; set; }
        uint MessageCount { get; }
        void StartListening();
    }
}
