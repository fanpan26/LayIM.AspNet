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
            string token = CookieHelper.GetCookieValue(key,contextBase);
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
    }
}
