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
    }
}
