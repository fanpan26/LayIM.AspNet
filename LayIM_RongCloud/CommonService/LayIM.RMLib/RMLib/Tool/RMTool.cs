using donet.io.rong;
using donet.io.rong.messages;
using donet.io.rong.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.IMServer.RMLib.Tool
{
    public class RMTool
    {
        private RongCloud Instance => RongCloud.getInstance(RMConfig.RM_AppKey, RMConfig.RM_AppSecret);


        #region 获取用户融云token
        /// <summary>
        /// 获取用户融云token
        /// </summary>
        /// <param name="cvNumber"></param>
        /// <param name="userName"></param>
        /// <param name="headPhoto"></param>
        /// <returns></returns>
        public string GetToken(long userId, string userName="", string headPhoto="",bool refresh=false)
        {
            //return "r6nQO3adGsVbdnpGALw0v2DKl0hBQxzyXluzzuM1s56SjtnhPpO/0L9oiIHoBniHmQ3cMV1Re8ngYLFDCHH0Sg==";
           
            var key = "RM_TOKEN_" + userId;
            ////先从cache里面获取
            //var token = //Utility.Cache.Base.MemcachedBase.Get<string>(key);
            //if (!refresh)
            //{
            //    if (token != null && token.Length > 0)
            //    {
            //        return token;
            //    }
            //}
            TokenReslut result = Instance.user.getToken(userId.ToString(), userName, headPhoto);

            //if (result.getCode() == 200)
            //{
            //    Utility.Cache.Base.MemcachedBase.Set(key, result.getToken());
            //    return result.getToken();
            //}
            //Utility.LogHelper.Error(MethodBase.GetCurrentMethod(), result.getErrorMessage());
            return result.getToken(); ;
        }
        #endregion


        /// <summary>
        /// 发送电脑端命令
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="cmd">openqr 打开二维码  closeqr  关闭二维码  clear 清空弹幕</param>
        /// <param name="cvNumber"></param>
        public void SendPCCommand(long activityId, string cmd, long cvNumber)
        {
        //    var roomId = AdminRoomId(activityId);
        //    String[] roomIds = { roomId };
        //    var obj = new { cmd = cmd };
        //    SendIMMessage(cvNumber.ToString(), roomIds, obj, RMMessageType.typeActivityCommand);
        }
      
    }
}
