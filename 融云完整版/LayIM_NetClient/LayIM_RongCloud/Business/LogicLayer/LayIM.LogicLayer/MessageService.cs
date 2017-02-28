using LayIM.Container;
using LayIM.DataAccessLayer.Interface;
using LayIM.Lib;
using LayIM.Model.Message.DB;
using LayIM.Queue.Service;
using LayIM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.LogicLayer
{
    public class MessageService : IMessageService
    {
        private IChatMessage _message => LayIMDataAccessLayerContainer.GlobalContainer.Resolve<IChatMessage>();

        public bool AddMsg(ChatMessage message)
        {
            return _message.AddMessage(message);
        }

        /// <summary>
        /// 添加消息到数据库
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="to"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool AddMsg(int userId, int to, string type, string content)
        {
            var msg = MessageCreator.MakeMessage(userId, to, type, content);
            return AddMsg(msg);
        }
    }

    /// <summary>
    /// 消息队列添加消息
    /// </summary>
    public class MessageQueueService : IMessageService
    {
        public bool AddMsg(ChatMessage message)
        {
            var queue = LayIMGlobalServiceContainer.GlobalContainer.Resolve<ILayIMQueue>();
            //将消息发送到队列处理
            queue.Publish(message);
            return true;
        }

        public bool AddMsg(int userId, int to, string type, string content)
        {
            var msg = MessageCreator.MakeMessage(userId, to, type, content);

            return AddMsg(msg);
        }
    }

    /// <summary>
    /// 组创建类
    /// </summary>
    internal sealed class GroupCreator
    {

        /// <summary>
        /// 生成群组名称
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static string CreateName(int gid)
        {
            return string.Format("GROUP_{0}", gid);
        }

        /// <summary>
        /// 采用比大小的形式生成groupid 例如  from 10000  to  10001 那么groupid 为 10001_10000 (from 10001,to 10000 同理，生成的组名一致)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string CreateName(int from, int to)
        {
            int temp = 0;
            if (from < to)
            {
                temp = from;
                from = to;
                to = temp;
            }
            return string.Format("FRIEND_{0}_{1}", from, to);
        }
    }

    internal sealed class MessageCreator
    {
        /// <summary>
        /// 组织消息格式
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="to"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static ChatMessage MakeMessage(int userId, int to, string type, string content)
        {
            bool isFriend = type == "friend";
            return new ChatMessage
            {
                AddTime = DateTime.Now.ToTimeStamp(),
                FromUserId = userId,
                Msg = content,
                ChatType = isFriend ? (short)1 : (short)2,
                MsgType = 0,
                ToUserId = to,
                ChatGroupId = isFriend ? GroupCreator.CreateName(userId, to) : GroupCreator.CreateName(to)
            };
        }
    }
}
