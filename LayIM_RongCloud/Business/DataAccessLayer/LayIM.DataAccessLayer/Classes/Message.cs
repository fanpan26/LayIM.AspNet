using LayIM.DataAccessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayIM.Model.Message.DB;
using LayIM.DataAccessLayer.Helper;

namespace LayIM.DataAccessLayer.Classes
{
    public class Message : IChatMessage
    {
        /// <summary>
        /// 往数据库里添加一条消息记录
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool AddMessage(ChatMessage message)
        {
            string sql = "INSERT INTO dbo.layim_msg_history (fromuser,gid,msg,chattype,addtime,msgtype) VALUES(@user,@gid,@msg,@ctype,@time,@mtype)";
            return DBHelper.Execute(sql, new
            {
                user = message.FromUserId,
                gid = message.ChatGroupId,
                msg = message.Msg,
                ctype = message.ChatType,
                time = message.AddTime,
                mtype = message.MsgType
            });
        }
    }
}
