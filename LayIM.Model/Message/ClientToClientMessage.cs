using LayIM.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Model.Message
{

    public class ClientBaseMessage
    {
        public ClientBaseMessage() {
            addtime = DateTime.Now;
        }
        public FromMessage mine { get; set; }
        public ChatToClientType type { get; set; }
        public DateTime addtime { get; set; }
        public string roomid { get; set; }
        public int timestamp { get; set; }
    }
    public class ClientToClientMessage : ClientBaseMessage
    {

        public ToMessage to { get; set; }
    }
    public class ClientToGroupMessage : ClientBaseMessage
    {
        public ToGroupMessage to { get; set; }
    }


    public class FromMessage : UserEntity
    {
        /// <summary>
        /// 聊天内容
        /// </summary>
        public string content { get; set; }
        public bool mine { get { return true; } }
    }

    public class ToMessage : GroupUserEntity
    {
        public string type { get; set; }
    }

    public class ToGroupMessage : GroupEntity
    {
        public string type { get; set; }
    }

    /// <summary>
    ///   客户端消息不需要那么多字段，这里越少越好，最好和接口的一致
    /// </summary>
    public class ClientToClientReceivedMessage : AvatarEntity
    {
        public int fromid { get; set; }
        public string content { get; set; }
        public string username { get; set; }
        public string type { get; set; }
    }

    public class ToClientMessageResult
    {
        public ChatToClientType msgtype { get; set; }
        public object msg { get; set; }
        /// <summary>
        /// other为了更灵活的定义消息类型，除了layim需要的消息，还可能需要其他消息信息
        /// </summary>
        public object other { get; set;}
    }

    /// <summary>
    /// 加群消息
    /// </summary>
    public class UserGroupCreatedMessage : GroupEntity
    {
        public int memebers { get; set; }
        public string type { get; set; }
    }
    public class UserFriendAddedMessage : GroupUserEntity
    {
        public string type { get; set; }
    }
    /*
        layim.addList({
      type: 'group'
      ,avatar: "http://tva3.sinaimg.cn/crop.64.106.361.361.50/7181dbb3jw8evfbtem8edj20ci0dpq3a.jpg"
      ,groupname: 'Angular开发'
      ,id: "12333333"
      ,members: 0
    });
    layim.addList({
      type: 'friend'
      ,avatar: "http://tp2.sinaimg.cn/2386568184/180/40050524279/0"
      ,username: '冲田杏梨'
      ,groupid: 2
      ,id: "1233333312121212"
      ,remark: "本人冲田杏梨将结束AV女优的工作"
    });
        */

    /// <summary>
    /// 用户发送申请请求，对方收到消息提示
    /// </summary>
    public class ApplySendedMessage
    {
        public string msg { get; set; }
    }

    /// <summary>
    /// 消息被处理
    /// </summary>
    public class ApplyHandledMessgae
    {
        //id,applytype,targetid,userid
        public int id { get; set; }
        public short applytype { get; set; }
        public int applyuid { get; set; }
        public int targetid { get; set; }
        public short result { get; set; }
        //附加群组消息
        public UserGroupCreatedMessage group { get; set; }
        //附加好友消息
        public UserFriendAddedMessage friend { get; set; }
        public UserFriendAddedMessage mine { get; set; }
    }
}
