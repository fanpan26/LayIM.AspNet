using LayIM.Utils.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using LayIM.Cache.RedisUtil;
using LayIM.Utils.Extension;
using LayIM.Utils.Config;
using StackExchange.Redis.Extensions.Newtonsoft;
using StackExchange.Redis.Extensions.Core;
using LayIM.Utils.Consts;
using LayIM.Utils.Random;
using LayIM.Utils.Cookie;
using LayIM.Model.Online;
using System.Web;

namespace LayIM.Cache
{
    public class LayIMCache
    {
        #region 变量
        public static LayIMCache Instance
        {
            get
            {
                return SingleHelper<LayIMCache>.Instance;
            }
        }

        static NewtonsoftSerializer serializer = new NewtonsoftSerializer();
        StackExchangeRedisCacheClient cacheClient = new StackExchangeRedisCacheClient(serializer);
        #endregion

        #region 缓存用户的token
        public async Task CacheUserAfterLogin(int userid)
        {
            var key = LayIMConst.LayIM_Cache_UserLoginToken;
            var token = RandomHelper.GetUserToken();
            //存redis
            bool result = await cacheClient.HashSetAsync(key, token, userid);
            if (result)
            {
                //写cookie
                CookieHelper.SetCookie(key, token);
            }
            else
            {
                //记录日志
            }
        }
        #endregion

        #region 获取当前登录用户的用户id

        public string GetCurrentUserId(HttpContextBase contextBase = null)
        {
            var key = LayIMConst.LayIM_Cache_UserLoginToken;
            string token = CookieHelper.GetCookieValue(key, contextBase);
            var userid = cacheClient.HashGet<string>(key, token);
            return userid;
        }
        #endregion

        #region 在线用户处理
        public void OperateOnlineUser(OnlineUser user, bool isDelete = false)
        {
            if (isDelete)
            {
                cacheClient.HashDelete(LayIMConst.LayIM_All_OnlineUsers, user.userid);
            }
            else
            {
                cacheClient.HashSetAsync(LayIMConst.LayIM_All_OnlineUsers, user.userid, user.connectionid);
            }
        }
        #endregion

        #region 根据用户ID判断某个用户是否在线

        public bool IsOnline(int userid)
        {
            string result = cacheClient.HashGet<string>(LayIMConst.LayIM_All_OnlineUsers, userid.ToString());
            return !string.IsNullOrEmpty(result);
        }
        #endregion

        #region 缓存用户好友列表
        /// <summary>
        /// 缓存用户好友列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="list">好友列表 1,2,3,4 （不知道好友列表长度会不会超过限制，如果超过限制，就不能用string存储了）</param>
        /// <returns>返回是否成功 true  false</returns>
        public bool SetUserFriendList(int userId, string list)
        {
            if (string.IsNullOrEmpty(list))
            {
                return true;
            }
            //用户好友列表key
            var key = string.Format(LayIMConst.LayIM_All_UserFriends, userId);
            //如果key已经存在，先remove掉
            if (cacheClient.Exists(key))
            {
                cacheClient.Remove(key);
            }
            //一天过期
            return cacheClient.Add<string>(key, list, DateTimeOffset.Now.AddDays(1));
        }
        #endregion

        #region 获取用户好友列表
        public string GetUserFriendList(int userId)
        {
            //用户好友列表key
            var key = string.Format(LayIMConst.LayIM_All_UserFriends, userId);
            //一天过期
            string list = cacheClient.Get<string>(key);
            if (string.IsNullOrEmpty(list))
            {
                return "";
            }
            return list;
        }
        #endregion
    }
}
