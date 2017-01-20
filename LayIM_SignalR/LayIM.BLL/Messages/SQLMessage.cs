using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayIM.Model.Result;
using Macrosage.ElasticSearch.Models;
using LayIM.Model.Message;

namespace LayIM.BLL.Messages
{
    class SQLMessage : IBaseMessage
    {
        public SendMessageResult Send(ClientBaseMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
