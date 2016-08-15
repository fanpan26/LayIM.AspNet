using LayIM.BLL.Messages;
using LayIM.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.BLL
{
    /// <summary>
    /// 消息处理工厂
    /// </summary>
    public sealed class MessageFactory
    {
        public static IBaseMessage CreateInstance(ChatMessageSaveType type)
        {
            IBaseMessage msg;
            switch (type) {
                case ChatMessageSaveType.SearchEngine:
                    msg = ElasticMessage.Instance;
                    break;
                case ChatMessageSaveType.SQL:
                    msg = new SQLMessage();
                    break;
                case ChatMessageSaveType.Queue:
                    msg = new QueueMessage();
                    break;
                case ChatMessageSaveType.Main:
                    msg = new QueueMessage();
                    break;
                default:
                    msg = new SQLMessage();
                    break;
            }
            return msg;

        }
    }
}
