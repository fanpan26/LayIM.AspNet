using LayIM.Lib;
using LayIM.Model.Message.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LayIM_RongCloud_Chat.Utils
{
    public sealed class UserProvider
    {
        public static int? GetUserId()
        {
            string token = CookieHelper.GetCookieValue("LAYIM_USER");
            var user = token?.ToObject<LoginUser>();
            return user?.UserId;
        }
    }
}