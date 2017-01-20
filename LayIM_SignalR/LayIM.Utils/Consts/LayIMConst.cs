using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.Utils.Consts
{
    public class LayIMConst
    {
        /// <summary>
        /// 用户登录成功后的cookie ，userid
        /// </summary>
        public const string LayIM_SignalR_UserId = "layim_userid";
        public const string LayIMGroupType = "group";
        public const string LayIMFriendType = "friend";

        /// <summary>
        /// 保存用户登录token的hashkey
        /// </summary>
        public const string LayIM_Cache_UserLoginToken = "user_login_token";
        /// <summary>
        /// 某个聊天室用户key
        /// 由于1v1聊天也是用group，发送消息时需要验证，两个人是否在同一聊天室，否则不能发送
        /// 群组同理
        /// </summary>
        public const string LayIM_Group_OnLineUsers = "user_online_{0}";
        /// <summary>
        /// 所有在线用户key
        /// </summary>
        public const string LayIM_All_OnlineUsers = "user_online_all";
        public const string LayIM_All_UserFriends = "friendlist:user_{0}";
    }
}
