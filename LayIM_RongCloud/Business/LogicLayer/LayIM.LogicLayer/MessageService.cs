using LayIM.Container;
using LayIM.DataAccessLayer.Interface;
using LayIM.Model.Message.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.LogicLayer
{
    public class MessageService
    {
        private IChatMessage _message => LayIMDataAccessLayerContainer.GlobalContainer.Resolve<IChatMessage>();
        /// <summary>
        /// 将聊天消息加入到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddMessage(ChatMessage model)
        {
            return _message.AddMessage(model);
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
}
