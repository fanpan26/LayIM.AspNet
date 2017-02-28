using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LayIM_RongCloud_Chat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //获取token
            routes.MapRoute(
               name: "gettoken",
               url: "token",
               defaults: new { controller = "IM", action = "getToken" }
           );
            //获取用户基础书
            routes.MapRoute(
               name: "base",
               url: "base",
               defaults: new { controller = "Data", action = "GetUserBaseList" }
           );
            //消息数据处理
            routes.MapRoute(
              name: "msg",
              url: "msg",
              defaults: new { controller = "Data", action = "AddMsg" }
          );
            //用户登录
            routes.MapRoute(
              name: "login",
              url: "login",
              defaults: new { controller = "Data", action = "Login" }
          );
            //用户搜索
            routes.MapRoute(
              name: "searchuser",
              url: "search/users",
              defaults: new { controller = "Data", action = "SearchUser" }
          );
            //组搜索
            routes.MapRoute(
              name: "searchgroup",
              url: "search/groups",
              defaults: new { controller = "Data", action = "SearchGroup" }
          );
            //申请加好友
            routes.MapRoute(
              name: "applyfriend",
              url: "apply/friend",
              defaults: new { controller = "Data", action = "ApplyFriend" }
          );
            //申请加群
            routes.MapRoute(
              name: "applygroup",
              url: "apply/group",
              defaults: new { controller = "Data", action = "ApplyGroup" }
          );
            //获取申请列表
            routes.MapRoute(
              name: "applylist",
              url: "apply/list",
              defaults: new { controller = "Data", action = "GetApplyList" }
          );
            routes.MapRoute(
             name: "handle",
             url: "handlefriend",
             defaults: new { controller = "Data", action = "HandleFriendApply" }
         );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
