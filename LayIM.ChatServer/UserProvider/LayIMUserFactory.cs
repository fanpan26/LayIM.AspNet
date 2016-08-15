using LayIM.Utils.Consts;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LayIM.ChatServer.UserProvider
{
    public class LayIMUserFactory : IUserIdProvider
    {
        /// <summary>
        /// 自定义获取用户ID方法
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetUserId(IRequest request)
        {
            //直接读取Cookie中的userid，然后将userid返回，否则返回空，未登录
            if (request.GetHttpContext().Request.Cookies[LayIMConst.LayIM_SignalR_UserId] != null)
            {
                return request.GetHttpContext().Request.Cookies[LayIMConst.LayIM_SignalR_UserId].Value;
            }
            return "";
        }
    }
}