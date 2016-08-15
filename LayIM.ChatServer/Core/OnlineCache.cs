using LayIM.Cache;
using LayIM.Model.Online;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.ChatServer.Core
{
   public class OnlineCache
    {
        /// <summary>
        /// 添加或者更新状态（connectionid在刷新后会变，所以需要更新）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool SetUserOnline(OnlineUser user)
        {
            return LayIMCache.Instance.SetHash<OnlineUser>(user.userid, user);
        }
        /// <summary>
        /// 用户下线
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool SetUserOffline(OnlineUser user)
        {
            return LayIMCache.Instance.RemoveHash(user.userid);
        }

        /// <summary>
        /// 用户是否在线
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static bool IsUserOnLine(string userid)
        {
            return LayIMCache.Instance.GetHash<OnlineUser>(userid).userid != "0";
        }
    }
}
