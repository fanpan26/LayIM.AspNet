using LayIM.Model.Message;
using LayIM.Model.Result;
using Macrosage.ElasticSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.BLL
{
   public interface IBaseMessage
    {
        /// <summary>
        /// 发送消息，处理逻辑由详细继承类实现，直接保存数据库，或者走队列，或者走ES
        /// </summary>
        /// <returns></returns>
        SendMessageResult Send(ClientBaseMessage message);

    }
}
